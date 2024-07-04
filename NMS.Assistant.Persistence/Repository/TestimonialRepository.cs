using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class TestimonialRepository : ITestimonialRepository
    {
        private readonly NmsAssistantContext _db;

        public TestimonialRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<Testimonial>>> GetAllTestimonials()
        {
            List<Testimonial> testimonials = await _db.Testimonials.OrderBy(f => f.SortRank).ToListAsync();
            if (testimonials == null) return new ResultWithValue<List<Testimonial>>(false, new List<Testimonial>(), "Could not load Testimonials");

            return new ResultWithValue<List<Testimonial>>(true, testimonials, string.Empty);
        }

        public async Task<Result> AddTestimonial(Testimonial addTestimonial)
        {
            try
            {
                await _db.Testimonials.AddAsync(new Testimonial
                {
                    Guid = Guid.NewGuid(),
                    Name = addTestimonial.Name,
                    ImageUrl = addTestimonial.ImageUrl,
                    SortRank = addTestimonial.SortRank,
                    Source = addTestimonial.Source,
                    Review = addTestimonial.Review
                });
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditTestimonial(Testimonial editTestimonial)
        {
            try
            {
                _db.Testimonials.Update(editTestimonial);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteTestimonial(Guid guid)
        {
            try
            {
                Testimonial toDelete = await _db.Testimonials.FirstAsync(d => d.Guid.Equals(guid));
                if (toDelete == null) return new Result(false, "Could not find the specified Guid");

                _db.Testimonials.Remove(toDelete);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
