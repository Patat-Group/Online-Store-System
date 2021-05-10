using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.ReportDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IReportRepository _reportRepo;
        private readonly IMapper _mapper;

        public ReportController(IUserRepository userRepo,IReportRepository reportRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _reportRepo = reportRepo;
            _mapper = mapper;
        }
        [HttpPost("report_user")]
        [Authorize]
        public async Task<ActionResult>ReportUser([FromQuery] string username,[FromForm]string report)
        {
            var userReporter = await _userRepo.GetByUserClaims(HttpContext.User);
            if (userReporter == null) return BadRequest("Bad Token");
            var userReported= await _userRepo.GetByUsername(username);
            if (userReported == null) return BadRequest("User Not Found");
            if (userReporter.Id == userReported.Id) return BadRequest("You Can't Report Yourself");
            var lastReportDate = await _reportRepo.GetLastReportDate(userReporter.Id, userReported.Id);
            if (DateTime.Now < lastReportDate.AddDays(1))
                return BadRequest("User Can't Report Same User more than once in a day");
            var newReport = new Report
            {
                UserSourceReportId = userReporter.Id,
                UserDestinationReportId = userReported.Id,
                ReportString = report,
                ReportDate = DateTime.Now
            };
            var result = await _reportRepo.AddReport(newReport);
            if (result)
                return Ok("Report done successfully");
            return BadRequest("Error Occured While Reporting User");
        }
        [HttpPost("get_all")]
        public async Task<ActionResult<IReadOnlyList<ReportToReturnDto>>>GetAll()
        {
            var result = await _reportRepo.GetAll();
            var reportsToReturn=_mapper.Map<IReadOnlyList<Report>, IReadOnlyList<ReportToReturnDto>>(result);
            if (result.Count>0)
                return Ok(reportsToReturn);
            return NotFound("No Reports");
        }
        [HttpPost("get_all_reports_to_user")]
        public async Task<ActionResult<IReadOnlyList<ReportToReturnDto>>>GetReportsToUser([FromQuery] string username)
        {
            var user= await _userRepo.GetByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var result = await _reportRepo.GetAllReportsByUserId(user.Id);
            var reportsToReturn=_mapper.Map<IReadOnlyList<Report>, IReadOnlyList<ReportToReturnDto>>(result);
            if (result.Count>0)
                return Ok(reportsToReturn);
            return NotFound("No Reports");
        }
        [HttpPost("delete")]
        public async Task<ActionResult<IReadOnlyList<ReportToReturnDto>>>GetReportsToUser([FromQuery]ReportToDelete reportToDelete)
        {
            var result = await _reportRepo.DeleteReport(reportToDelete.Id);
            if (result)
                return Ok("Report Deleting Succeeded");
            return BadRequest("Report Not Found");
        }
    }
}