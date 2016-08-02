namespace ConsulKvCacheClient.Response
{
    public class ConfigResponse
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsEnabled { get; set; }
        public string Domain { get; set; }
    }
}
