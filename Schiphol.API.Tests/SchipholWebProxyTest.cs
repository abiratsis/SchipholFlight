using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Schiphol.FlightAPI;
using Schiphol.FlightAPI.Models;

namespace Schiphol.FlightAPI.Tests
{
    [TestFixture]
    public class SchipholWebProxyTest
    {
        private SchipholWebProxy _schipholWebProxy;

        [OneTimeSetUp]
        public void IntializeSettings()
        {
            _schipholWebProxy = new SchipholWebProxy
            {
                SchipholApiUrl = "https://api.schiphol.nl",
                APP_KEY = "9e912d9870bab7494f9045f35f059819",
                APP_ID = "9096ccc8"
            };
        }

        [Test]
        public async Task Get_airlines_page_0_returns_airlinelist()
        {
            var result = await _schipholWebProxy.Get("airlines", ResourseVersion.V1, 0);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Content != null);

            var airlines = JsonConvert.DeserializeObject<AirlineList>(result.Content.ToString());

            Assert.True(airlines != null);
            Assert.True(airlines.AirLines.Count > 0);
        }

        [Test]
        public async Task Get_airlines_page_minus_one_throws_exception()
        {
            var result = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => 
                await _schipholWebProxy.Get("airlines", ResourseVersion.V1, -1));

            Assert.AreEqual(result.ParamName, "page");
            Assert.AreEqual(result.ActualValue, -1);
        }

        [Test]
        public async Task Get_airlines_huge_page_num_returns_bad_request()
        {
            var result = await _schipholWebProxy.Get("flights", ResourseVersion.V3, 100000);

            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(result.ErrorMessage == "Bad Request");
        }

        [Test]
        public async Task Get_airlines_invalid_route_returns_HTTP_not_found()
        {
            var response = await _schipholWebProxy.Get("flightz", ResourseVersion.V3, 0);

            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.True(response.ErrorMessage == "Not Found");
        }

        [Test]
        public async Task Get_arrivals_page_0_returns_flightslist()
        {
            var otherParams = new Dictionary<string, string>
            {
                {"fromdate", "2017-11-14" },
                {"todate", "2017-11-15" },
                {"flightdirection", "A" }
            };

            var result = await _schipholWebProxy.Get("flights", ResourseVersion.V3, 0, otherParams);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Content != null);

            var flights = JsonConvert.DeserializeObject<ArrivalList>(result.Content.ToString());

            Assert.True(flights != null);
            Assert.True(flights.Flights.Count > 0);

            //check that all the states returned exist on StatesDetails dictionary
            Assert.True(flights.Flights.TrueForAll(f => f.PublicFlightState.FlightStates.TrueForAll(fs => FlightStateList.StatesDetails.ContainsKey(fs))));
        }

        [Test]
        public async Task Get_departures_invalid_airline_returns_no_content()
        {
            var otherParams = new Dictionary<string, string>
            {
                {"fromdate", DateTime.Today.ToString("yyyy-MM-dd") },
                {"todate", DateTime.Today.ToString("yyyy-MM-dd") },
                {"flightdirection", "D" },
                {"airline", "KJJK"}
            };

            var result = await _schipholWebProxy.Get("flights", ResourseVersion.V3, 0, otherParams);
            Assert.True(result.StatusCode == HttpStatusCode.NoContent);
            Assert.True(result.Content == string.Empty);
        }

    }
}
