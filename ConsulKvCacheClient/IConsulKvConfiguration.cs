namespace ConsulKvCacheClient
{
    public interface IConsulKvConfiguration
    {
        double? Interval { get; set; }
        string Domain { get; set; }
        string Token { get; set; }
    }
}