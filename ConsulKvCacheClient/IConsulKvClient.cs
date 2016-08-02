using ConsulKvCacheClient.Request;

namespace ConsulKvCacheClient
{
    public interface IConsulKvClient
    {
        bool Write(ConfigRequest request);
        T GetValue<T>(string key);
    }
}
