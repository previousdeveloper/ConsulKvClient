namespace ConsulKvCacheClient
{
    public static class Paths
    {
        public static readonly string GetUrl = "/v1/kv/{0}/{1}?token={2}";
        public static readonly string GetAllUrl = "/v1/kv/{0}/?recurse&token={1}";
        public static readonly string WriteUrl = "/v1/kv/{0}/{1}?token={2}";
    }
}
