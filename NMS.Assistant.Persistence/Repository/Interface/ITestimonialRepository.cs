using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ITestimonialRepository
    {
        Task<ResultWithValue<List<Testimonial>>> GetAllTestimonials();
        Task<Result> AddTestimonial(Testimonial addTestimonial);
        Task<Result> EditTestimonial(Testimonial editTestimonial);
        Task<Result> DeleteTestimonial(Guid guid);
    }
}