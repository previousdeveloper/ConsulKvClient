using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsulKvCacheClient.Models;
using ConsulKvCacheClient.Request;
using ConsulKvCacheClient.Response;
using Httwrap;
using Httwrap.Interface;
using Newtonsoft.Json;

namespace ConsulKvCacheClient
{
    public class KvOperation : IKvOperation
    {
        private readonly IHttwrapClient _httwrapClient;
        public KvOperation(IHttwrapClient httwrapClient)
        {
            _httwrapClient = httwrapClient;
        }

        public ConfigResponse Get(string domainName, string key)
        {
            ConfigResponse result = null;
            try
            {
                IHttwrapResponse httwrapResponse = _httwrapClient.Get(string.Format(Paths.GetUrl, domainName, key), new object());
                ConsulConfigModel consulResult = httwrapResponse.ReadAs<List<ConsulConfigModel>>().First();

                if (consulResult != null && !string.IsNullOrEmpty(consulResult.Value))
                {
                    string decodedKeyValue = DecodeValue(consulResult.Value);

                    result = JsonConvert.DeserializeObject<ConfigResponse>(decodedKeyValue);

                    if (!result.IsEnabled)
                    {
                        result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return result;
        }

        public List<ConfigResponse> GetList(string domainName, string token)
        {
            List<ConfigResponse> result = new List<ConfigResponse>();

            try
            {
                IHttwrapResponse httwrapResponse = _httwrapClient.GetAsync(string.Format(Paths.GetAllUrl, domainName, token)).Result;
                List<ConsulConfigModel> consulResult = httwrapResponse.ReadAs<List<ConsulConfigModel>>();

                if (consulResult.Count > default(int))
                {
                    foreach (ConsulConfigModel consulItem in consulResult)
                    {
                        if (consulItem.Value == null)
                        {
                            continue;
                        }
                        string decodedKeyValue = DecodeValue(consulItem.Value);

                        ConfigResponse consulResponse = JsonConvert.DeserializeObject<ConfigResponse>(decodedKeyValue);

                        if (consulResponse.IsEnabled)
                        {
                            result.Add(consulResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public bool Write(ConfigRequest request, string token)
        {
            try
            {
                IHttwrapResponse httwrapResponse = _httwrapClient.Put(string.Format(Paths.WriteUrl, request.Domain, request.Key, token), request);
                string consulResult = httwrapResponse.ReadAs<string>();

                bool result;
                bool.TryParse(consulResult, out result);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private string DecodeValue(string value)
        {
            byte[] data = Convert.FromBase64String(value);
            string result = Encoding.UTF8.GetString(data);

            return result;
        }
    }
}