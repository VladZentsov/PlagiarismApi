﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Services.Interfaces;
using PlagiarismApi.Security;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace PlagiarismApi.Controllers
{
    [ApiController]
    public class WorkController : ControllerBase
    {
        private readonly IWorkService _workService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public WorkController(IWorkService workService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _workService = workService;
            _mapper = mapper;
        }
        [HttpPost("UploadWork")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                    if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
                    {
                        var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                        var user = JwtHelper.GetUserFromToken(token);
                        var code = await reader.ReadToEndAsync();
                        var work = await _workService.UploadWork(user.Id, file.FileName, Plagiarism_BLL.Enums.WorkType.cs, code);
                        return Ok(work);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Compare/{currentWork}/{workToCompare}")]
        public async Task<IActionResult> CompareWorks(Guid currentWork, Guid workToCompare)
        {
            return Ok(await _workService.CompareWorks(currentWork, workToCompare));
        }

        [HttpGet("CompareToAll/{currentWork}")]
        public async Task<IActionResult> CompareToAll(Guid currentWork)
        {
            return Ok(await _workService.CompareToAllWorks(currentWork));
        }

        [HttpDelete("DeleteWork/{workId}")]
        public async Task<IActionResult> DeleteWork(Guid workId)
        {
            await _workService.DeleteWork(workId);
            return Ok();
        }
    }
}
