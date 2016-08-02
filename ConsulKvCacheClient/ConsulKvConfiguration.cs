namespace ConsulKvCacheClient
{
    public class ConsulKvConfiguration : IConsulKvConfiguration
    {
        public double? Interval { get; set; }
        public string Domain { get; set; }
        public string Token { get; set; }
    }
}