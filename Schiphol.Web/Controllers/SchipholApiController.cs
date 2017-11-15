using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Schiphol.FlightAPI;
using Schiphol.FlightAPI.Models;
using Schiphol.FlightAPI.Services;

namespace Schiphol.Web.Controllers
{
    [Route("schiphol-api")]
    public class SchipholApiController : Controller
    {
        private readonly IFlightsService _flightService;

        private readonly IAirlinesService _airlinesService;
        private readonly ILogger<SchipholApiController> _logger;
        public SchipholApiController(IFlightsService flightService, IAirlinesService airlinesService, ILogger<SchipholApiController> logger)
        {
            _flightService = flightService;
            _airlinesService = airlinesService;
            _logger = logger;
        }

        // GET airlines?page=0
        [HttpGet("airlines")]
        public async Task<IActionResult> GetAirlines(int page = 0)
        {
            string error;
            try
            {
                var response = await _airlinesService.GetAirlines(page);

                return response.StatusCode == HttpStatusCode.OK ? (IActionResult) Json(response.Content) : StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
            catch (ArgumentNullException e)
            {
                error = $"Null argument for parameter {e.ParamName}";
                _logger.LogError(error);

                return BadRequest(error);
            }
            catch (Exception e)
            {
                error = "Aknown error, please contact the system administrator.";
                _logger.LogError(e.StackTrace);

                return StatusCode(500, error);
            }
        }

        // GET arrivals?page=0&filter=tomorrow
        [HttpGet("arrivals")]
        public async Task<IActionResult> GetArrivals(int page, DayFilter? filter = DayFilter.Today)
        {
            string error;
            try
            {
                if(!filter.HasValue)
                    filter = DayFilter.Today;

                var response = await _flightService.GetArrivals(page, filter.Value);

                return response.StatusCode == HttpStatusCode.OK ? (IActionResult)Json(response.Content) : StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
            catch (ArgumentNullException e)
            {
                error = $"Null argument for parameter {e.ParamName}";
                _logger.LogError(error);

                return BadRequest(error);
            }
            catch (Exception e)
            {
                error = "Aknown error, please contact the system administrator.";
                _logger.LogError(e.StackTrace);

                return StatusCode(500, error);
            }
        }

        // GET departures?page=0&airline=KL
        [HttpGet("departures")]
        public async Task<IActionResult> GetDepartures(int page, string airline)
        {
            string error;
            try
            {
                var response = await _flightService.GetDepartures(page, airline);

                return response.StatusCode == HttpStatusCode.OK ? (IActionResult)Json(response.Content) : StatusCode((int)response.StatusCode, response.ErrorMessage);
            }
            catch (ArgumentNullException e)
            {
                error = $"Null argument for parameter {e.ParamName}";
                _logger.LogError(error);

                return BadRequest(error);
            }
            catch (Exception e)
            {
                error = "Aknown error, please contact the system administrator.";
                _logger.LogError(e.StackTrace);

                return StatusCode(500, error);
            }
        }
    }
}
