using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Hangfire.UI.Dtos;
using Hangfire.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Hangfire.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchivesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IArchiveService _archiveService;

        public ArchivesController(IConfiguration configuration,
            IArchiveService archiveService
        )
        {
            _configuration = configuration;
            _archiveService = archiveService;
        }

        [HttpGet("{id}")]
        public ActionResult Get([NotNull] string id)
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            return Ok(new
            {
                status = $"{monitoringApi.JobDetails(id).History.Last().StateName}"
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody] ScheduleArchiveJobDto dto)
        {
            var storePath = _configuration.GetSection("ArchiveConfig")["StorePath"];
            return Ok(new
                {
                    id = _archiveService.ScheduleZipArchiveJob(dto.WhatToCompress, storePath)
                }
            );
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            BackgroundJob.Delete(id);
        }

        [HttpGet]
        [Route("/api/archives/formats")]
        public IActionResult ListFormats()
        {
            return Ok(Enum.GetNames<ArchiveFormat>());
        }
    }
}