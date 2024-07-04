namespace NMS.Assistant.Domain.Contract
{
    public class NewsItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }

        public int GetHash => Link?.GetHashCode() ?? 0;
    }
}
