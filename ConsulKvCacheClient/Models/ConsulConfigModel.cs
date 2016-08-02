namespace ConsulKvCacheClient.Models
{
    public class ConsulConfigModel
    {
        public int LockIndex { get; set; }
        public string Key { get; set; }
        public int Flags { get; set; }
        public string Value { get; set; }
        public int CreateIndex { get; set; }
        public int ModifyIndex { get; set; }
    }
}
