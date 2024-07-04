using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Donation;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DonationController : ControllerBase
    {
        private readonly IDonationRepository _donationRepo;

        public DonationController(IDonationRepository donationRepo)
        {
            _donationRepo = donationRepo;
        }

        /// <summary>
        /// Get All Donations.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.Donators)]
        public async Task<ActionResult<List<DonationViewModel>>> GetAllDonations()
        {
            ResultWithValue<List<Donation>> donationsResult = await _donationRepo.GetAllDonations();
            if (donationsResult.HasFailed) return NoContent();

            return Ok(donationsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All Donations with Admin properties.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DonationsView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.DonationsView)]
        public async Task<ActionResult<List<AdminDonationViewModel>>> GetAllDonationsForAdmin()
        {
            ResultWithValue<List<Donation>> donationsResult = await _donationRepo.GetAllDonationsDesc();
            if (donationsResult.HasFailed) return NoContent();

            return Ok(donationsResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add new Donation.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DonationsManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.DonationsManage)]
        public async Task<IActionResult> AddDonation(AddDonationViewModel addDonation)
        {
            Result addResult = await _donationRepo.AddDonation(addDonation.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit Donation.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DonationsView, DonationsManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.DonationsView, PermissionType.DonationsManage)]
        public async Task<IActionResult> EditDonation(AdminDonationViewModel editDonation)
        {
            Result addResult = await _donationRepo.EditDonation(editDonation.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete Donation.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DonationsManage
        /// </remarks>
        /// <param name="guid">
        /// Donation Guid, available from /Donation/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.DonationsManage)]
        public async Task<IActionResult> DeleteDonation(Guid guid)
        {
            Result addResult = await _donationRepo.DeleteDonation(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}