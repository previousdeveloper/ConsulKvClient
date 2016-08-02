using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ConsulKvCacheClient.Request;
using ConsulKvCacheClient.Response;
using Httwrap;
using Httwrap.Interface;

namespace ConsulKvCacheClient
{
    public class ConsulKvClient : IConsulKvClient
    {
        private readonly IHttwrapClient _httwrapClient;
        private readonly IHttwrapConfiguration _httwrapConfiguration;
        private readonly IKvOperation _kvOperation;
        private readonly Timer _timer;
        private static List<ConfigResponse> configListResponse;
        private readonly IConsulKvConfiguration _consulClientConfiguration;
        public ConsulKvClient(IConsulKvConfiguration consulClientConfiguration)
        {
            _consulClientConfiguration = consulClientConfiguration;

            _httwrapConfiguration = new HttwrapConfiguration(Constants.BaseUrl);
            _httwrapClient = new HttwrapClient(_httwrapConfiguration);
            _kvOperation = new KvOperation(_httwrapClient);
            configListResponse = new List<ConfigResponse>();
            _timer = new Timer((double)(_consulClientConfiguration.Interval.HasValue ? _consulClientConfiguration.Interval : Constants.DefaultTimerInterval));

            GetList(_consulClientConfiguration.Domain);

            _timer.Elapsed += TimeElapsed;

            _timer.Start();
        }

        public bool Write(ConfigRequest request)
        {
            bool result = _kvOperation.Write(request, _consulClientConfiguration.Token);

            return result;
        }

        public T GetValue<T>(string key)
        {
            string value = null;

            if (configListResponse.Count > default(int))
            {
                ConfigResponse configResponse = configListResponse.First(item => item.Key == key);

                if (configResponse != null)
                {
                    value = configResponse.Value;
                }
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        private void GetList(string domainName)
        {
            List<ConfigResponse> configResponses = _kvOperation.GetList(domainName, _consulClientConfiguration.Token);

            if (configResponses.Count > default(int))
            {
                configListResponse = configResponses;
            }
        }

        private void TimeElapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            GetList(_consulClientConfiguration.Domain);

            _timer.Start();
        }

        private void Dispose()
        {
            _timer.Dispose();
        }
    }
}