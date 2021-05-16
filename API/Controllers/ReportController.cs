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

        public ReportController(IUserRepository userRepo, IReportRepository reportRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _reportRepo = reportRepo;
            _mapper = mapper;
        }
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> ReportUser([FromBody] ReportForCreationDto reportForCreationDto)
        {
            var userReporter = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (userReporter == null) return Unauthorized("User is Unauthorized");
            
            var userReported = await _userRepo.GetUserByUsername(reportForCreationDto.Username);
            if (userReported == null) return BadRequest("User Not Found");
            if (userReporter.Id == userReported.Id) return BadRequest("You Can't Report Yourself");
            var lastReportDate = await _reportRepo.GetLastReportDate(userReporter.Id, userReported.Id);
            if (DateTime.Now < lastReportDate.AddDays(1))
                return BadRequest("User Can't Report Same User more than once in a day");
            var newReport = new Report
            {
                UserSourceReportId = userReporter.Id,
                UserDestinationReportId = userReported.Id,
                ReportString = reportForCreationDto.ReportString,
                ReportDate = DateTime.Now
            };
            var result = await _reportRepo.AddReport(newReport);
            if (result)
                return Ok("Report done successfully");

            throw new Exception("Error Occured While Reporting User, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpGet("All")]
        public async Task<ActionResult<IReadOnlyList<ReportToReturnDto>>> GetAll()
        {
            var result = await _reportRepo.GetAll();
            var reportsToReturn = _mapper.Map<IReadOnlyList<Report>, IReadOnlyList<ReportToReturnDto>>(result);
            if (result.Count > 0)
                return Ok(reportsToReturn);
            return NotFound("No Reports");
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IReadOnlyList<ReportToReturnDto>>> GetReportsToUser(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var result = await _reportRepo.GetAllReportsByUserId(user.Id);
            var reportsToReturn = _mapper.Map<IReadOnlyList<Report>, IReadOnlyList<ReportToReturnDto>>(result);
            if (result.Count > 0)
                return Ok(reportsToReturn);
            return NotFound("No Reports");
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<ReportToReturnDto>>> GetReportsToUser([FromBody] ReportToDeleteDto reportToDeleteDto)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var result = await _reportRepo.DeleteReport(reportToDeleteDto.Id);
            if (result)
                return Ok("Report Deleting Succeeded");
            return BadRequest("Report Not Found");
        }
    }
}