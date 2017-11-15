using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Schiphol.FlightAPI.Models;
using Schiphol.FlightAPI.Services;

namespace Schiphol.FlightAPI.Tests
{
    [TestFixture]
    public class FlightServiceTest
    {
        private IWebProxy _schipholWebProxy;
        private IFlightsService _flightService;

        [OneTimeSetUp]
        public void IntializeSettings()
        {
            _schipholWebProxy = new SchipholWebProxy
            {
                SchipholApiUrl = "https://api.schiphol.nl",
                APP_KEY = "9e912d9870bab7494f9045f35f059819",
                APP_ID = "9096ccc8"
            };

            _flightService = new FlightsService(_schipholWebProxy);
        }

        [Test]
        public async Task Get_arrivals_page_0_returns_flightlist()
        {
            var response = await _flightService.GetArrivals(0);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Content != null);

            var arrivalList = response.Content as ArrivalList;

            Assert.True(arrivalList != null);
            Assert.True(arrivalList.Flights != null);
            Assert.True(arrivalList.Flights.Count > 0);
        }

        [Test]
        public async Task Get_arrivals_filter_today_returns_flightlist()
        {
            var response = await _flightService.GetArrivals(0, DayFilter.Today);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Content!= null);

            var arrivalList = response.Content as ArrivalList;

            Assert.True(arrivalList != null);
            Assert.True(arrivalList.Flights != null);
            Assert.True(arrivalList.Flights.Count > 0);

            //ensure that the list contains only flights for today
            var yesterday = DateTime.Today.AddDays(-1);
            Assert.True(arrivalList.Flights.Where(f => f.ActualLandingTime.HasValue && f.EstimatedLandingTime.HasValue)
                                                       .ToList()
                                                       .TrueForAll(f =>
                                                                    f.ActualLandingTime.HasValue &&
                                                                    (f.ActualLandingTime.Value.Date == DateTime.Today || f.ActualLandingTime.Value.Date == yesterday) &&
                                                                    f.EstimatedLandingTime.HasValue &&
                                                                    (f.EstimatedLandingTime.Value.Date == DateTime.Today || f.EstimatedLandingTime.Value.Date == yesterday)));
        }

        [Test]
        public async Task Get_arrivals_filter_tomorrow_returns_flightlist()
        {
            var response = await _flightService.GetArrivals(0, DayFilter.Tomorrow);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Content != null);

            var arrivalList = response.Content as ArrivalList;

            Assert.True(arrivalList != null);
            Assert.True(arrivalList.Flights != null);
            Assert.True(arrivalList.Flights.Count > 0);

            var tomorrow = DateTime.Today.AddDays(1);
            //ensure that the list contains only flights for tomorrow
            Assert.True(arrivalList.Flights.Where(f => f.ActualLandingTime.HasValue && f.EstimatedLandingTime.HasValue)
                                                    .ToList()
                                                    .TrueForAll(f =>
                                                                f.ActualLandingTime.HasValue &&
                                                                (f.ActualLandingTime.Value.Date == tomorrow || f.ActualLandingTime.Value.Date == DateTime.Today) &&
                                                                f.EstimatedLandingTime.HasValue &&
                                                                (f.EstimatedLandingTime.Value.Date == tomorrow || f.EstimatedLandingTime.Value.Date == DateTime.Today)));
        }

        [Test]
        public async Task Get_arrivals_filter_yesterday_returns_flightlist()
        {
            var response = await _flightService.GetArrivals(0, DayFilter.Yesterday);

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Content != null);

            var arrivalList = response.Content as ArrivalList;

            Assert.True(arrivalList != null);
            Assert.True(arrivalList.Flights != null);
            Assert.True(arrivalList.Flights.Count > 0);

            var yesterday = DateTime.Today.AddDays(-1);
            //ensure that the list contains only flights from yesterday
            var beforeYesterday = DateTime.Today.AddDays(-2);
            Assert.True(arrivalList.Flights.Where(f => f.ActualLandingTime.HasValue && f.EstimatedLandingTime.HasValue)
                                                    .ToList()
                                                    .TrueForAll(f =>
                                                        f.ActualLandingTime.HasValue &&
                                                        (f.ActualLandingTime.Value.Date == yesterday || f.ActualLandingTime.Value.Date == beforeYesterday) &&
                                                        f.EstimatedLandingTime.HasValue &&
                                                        (f.EstimatedLandingTime.Value.Date == yesterday || f.EstimatedLandingTime.Value.Date == beforeYesterday)));
        }

        [Test]
        public async Task Get_arrivals_page_negative_throws_exception()
        {
            var result = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _flightService.GetArrivals(-1));

            Assert.AreEqual(result.ParamName, "page");
            Assert.AreEqual(result.ActualValue, -1);
        }

        [Test]
        public async Task Get_arrivals_huge_page_returns_HTTP_bad_request()
        {
            var response = await _flightService.GetArrivals(100000);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Get_departures_page_0_filter_KL_returns_flightlist()
        {
            var response = await _flightService.GetDepartures(0, "KL");

            Assert.True(response.StatusCode == HttpStatusCode.OK);
            Assert.True(response.Content != null);

            var departureList = response.Content as DepartureList;

            Assert.True(departureList != null);
            Assert.True(departureList.Flights != null);
            Assert.True(departureList.Flights.Count > 0);

            //ensure all flight have expected boarding time today
            Assert.True(departureList.Flights.Where(f => f.ExpectedTimeBoarding.HasValue)
                .ToList()
                .TrueForAll(f => f.ExpectedTimeBoarding.HasValue && f.ExpectedTimeBoarding.Value.Date == DateTime.Today));
        }

        [Test]
        public async Task Get_departures_empty_airline_throw_exception()
        {
            var result = Assert.ThrowsAsync<ArgumentNullException>(async () => await _flightService.GetDepartures(0, null));

            Assert.AreEqual(result.ParamName, "airline");
        }

        [Test]
        public async Task Get_departures_invalid_airline_returns_HTTP_nocontent()
        {
            var response = await _flightService.GetDepartures(0, "KLAS");

            Assert.True(response.StatusCode == HttpStatusCode.NoContent);
        }
    }
}
