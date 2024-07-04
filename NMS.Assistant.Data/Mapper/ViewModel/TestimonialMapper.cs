using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Testimonial;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class TestimonialMapper
    {
        public static TestimonialViewModel ToViewModel(this Testimonial domain)
        {
            TestimonialViewModel vm = new TestimonialViewModel
            {
                Guid = domain.Guid,
                Name = domain.Name,
                ImageUrl = domain.ImageUrl,
                Review = domain.Review,
                Source = domain.Source,
            };
            return vm;
        }
        public static List<TestimonialViewModel> ToViewModel(this List<Testimonial> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static AdminTestimonialViewModel ToAdminViewModel(this Testimonial domain)
        {
            AdminTestimonialViewModel vm = new AdminTestimonialViewModel
            {
                Guid = domain.Guid,
                Name = domain.Name,
                ImageUrl = domain.ImageUrl,
                Review = domain.Review,
                SortRank = domain.SortRank,
                Source = domain.Source,
            };
            return vm;
        }
        public static List<AdminTestimonialViewModel> ToAdminViewModel(this List<Testimonial> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();

        public static Testimonial ToDatabaseModel(this AdminTestimonialViewModel vm)
        {
            Testimonial domain = new Testimonial
            {
                Guid = vm.Guid,
                Name = vm.Name,
                ImageUrl = vm.ImageUrl,
                Review = vm.Review,
                SortRank = vm.SortRank,
                Source = vm.Source,
            };
            return domain;
        }
        public static Testimonial ToDatabaseModel(this AddTestimonialViewModel vm)
        {
            Testimonial domain = new Testimonial
            {
                Guid = Guid.NewGuid(),
                Name = vm.Name,
                ImageUrl = vm.ImageUrl,
                Review = vm.Review,
                SortRank = int.Parse(vm.SortRank),
                Source = vm.Source,
            };
            return domain;
        }
    }
}
