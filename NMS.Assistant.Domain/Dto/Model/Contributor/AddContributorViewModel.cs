namespace NMS.Assistant.Domain.Dto.Model.Contributor
{
    public class AddContributorViewModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public int SortRank { get; set; }
        public string ImageUrl { get; set; }
    }
}