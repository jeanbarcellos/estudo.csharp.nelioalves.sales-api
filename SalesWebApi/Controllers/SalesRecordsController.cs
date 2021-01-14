using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebApi.Services;

namespace SalesWebApi.Controllers
{
    [ApiController]
    [Route("salles-records")]
    public class SalesRecordsController : ControllerBase
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var options = new { options = "/simple-search or /grouping-search" };

            return Ok(options);
        }


        [HttpGet]
        [Route("simple-search")]
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);

            return Ok(result);
        }

        [HttpGet]
        [Route("grouping-search")]

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);

            return Ok(result);
        }
    }
}