using System;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Testimonial
{
    public class TestimonialViewModel
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Review { get; set; }

        public ReviewSourceType Source { get; set; }

        public string ImageUrl { get; set; }
    }
}
