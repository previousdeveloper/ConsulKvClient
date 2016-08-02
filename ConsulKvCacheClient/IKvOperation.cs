using System.Collections.Generic;

using ConsulKvCacheClient.Request;
using ConsulKvCacheClient.Response;

namespace ConsulKvCacheClient
{
    public interface IKvOperation
    {
        ConfigResponse Get(string domainName, string key);
        List<ConfigResponse> GetList(string domainName, string token);
        bool Write(ConfigRequest request, string token);
    }
}