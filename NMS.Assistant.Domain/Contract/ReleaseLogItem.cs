namespace NMS.Assistant.Domain.Contract
{
    public class ReleaseLogItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public bool IsPc { get; set; }
        public bool IsPs4 { get; set; }
        public bool IsPs5 { get; set; }
        public bool IsXb1 { get; set; }
        public bool IsXbsx { get; set; }
        public bool IsNsw { get; set; }
        public bool IsMac { get; set; }
    }
}
