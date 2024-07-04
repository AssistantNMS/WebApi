using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Testimonial;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestimonialController : Controller
    {
        private readonly ITestimonialRepository _testimonialRepo;

        public TestimonialController(ITestimonialRepository testimonialRepo)
        {
            _testimonialRepo = testimonialRepo;
        }

        /// <summary>
        /// Get All saved Testimonials.
        /// </summary>
        [HttpGet]
        [CacheHeader]
        [CacheFilter(CacheType.Testimonials)]
        public async Task<ActionResult<List<TestimonialViewModel>>> GetAllTestimonials()
        {
            ResultWithValue<List<Testimonial>> testimonialsResult = await _testimonialRepo.GetAllTestimonials();
            if (testimonialsResult.HasFailed) return NoContent();

            return Ok(testimonialsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All saved Testimonials with Admin properties.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: TestimonialsView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.TestimonialsView)]
        public async Task<ActionResult<List<AdminTestimonialViewModel>>> GetAllTestimonialsForAdmin()
        {
            ResultWithValue<List<Testimonial>> testimonialsResult = await _testimonialRepo.GetAllTestimonials();
            if (testimonialsResult.HasFailed) return NoContent();

            return Ok(testimonialsResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add Testimonial.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: TestimonialsManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.TestimonialsManage)]
        public async Task<IActionResult> AddTestimonial(AddTestimonialViewModel addTestimonial)
        {
            Result addResult = await _testimonialRepo.AddTestimonial(addTestimonial.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit Testimonial.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: TestimonialsView, TestimonialsManage
        /// </remarks>
        /// <param name="editTestimonial">
        /// Testimonial viewModel.
        /// </param>   
        [HttpPut]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.TestimonialsView, PermissionType.TestimonialsManage)]
        public async Task<IActionResult> EditTestimonial(AdminTestimonialViewModel editTestimonial)
        {
            Result addResult = await _testimonialRepo.EditTestimonial(editTestimonial.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete Testimonial.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: TestimonialsManage
        /// </remarks>
        /// <param name="guid">
        /// Testimonial Guid, available from /Testimonial/Admin.
        /// </param>  
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.TestimonialsManage)]
        public async Task<IActionResult> DeleteTestimonial(Guid guid)
        {
            Result addResult = await _testimonialRepo.DeleteTestimonial(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}