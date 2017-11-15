using System;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NUnit.Framework;
using Schiphol.FlightAPI.Models;
using Schiphol.FlightAPI.Services;

namespace Schiphol.FlightAPI.Tests
{
    [TestFixture]
    public class AirlinesServiceTest
    {
        private IWebProxy _schipholWebProxy;
        private IAirlinesService _airlinesService;

        [OneTimeSetUp]
        public void IntializeSettings()
        {
            _schipholWebProxy = new SchipholWebProxy
            {
                SchipholApiUrl = "https://api.schiphol.nl",
                APP_KEY = "9e912d9870bab7494f9045f35f059819",
                APP_ID = "9096ccc8"
            };

            _airlinesService = new AirlinesService(_schipholWebProxy);
        }

        [Test]
        public async Task List_airlines_page_0_returns_airlinelist()
        {
            var response = await _airlinesService.GetAirlines(0);

            Assert.True(response.StatusCode == HttpStatusCode.OK);

            var airlineList = response.Content as AirlineList;

            Assert.True(airlineList != null);
            Assert.True(airlineList.AirLines != null);
            Assert.True(airlineList.AirLines.Count > 0);
        }

        [Test]
        public async Task List_airlines_page_negative_throws_exception()
        {
            var result = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _airlinesService.GetAirlines(-1));

            Assert.AreEqual(result.ParamName, "page");
            Assert.AreEqual(result.ActualValue, -1);
        }

        [Test]
        public async Task List_airlines_huge_page_returns_HTTP_bad_request()
        {
            var response = await _airlinesService.GetAirlines(100000);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }
    }
}
