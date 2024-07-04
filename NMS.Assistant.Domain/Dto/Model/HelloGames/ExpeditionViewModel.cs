using System;

namespace NMS.Assistant.Domain.Dto.Model.HelloGames
{
    public class ExpeditionViewModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
