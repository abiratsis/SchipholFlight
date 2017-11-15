using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Schiphol.FlightAPI.Models;

namespace Schiphol.FlightAPI.Services
{
    public class FlightsService : IFlightsService
    {
        private readonly IWebProxy _webProxy;

        public FlightsService(IWebProxy webProxy) { _webProxy = webProxy; }

        public async Task<FlightApiResponse> GetArrivals(int page, DayFilter filter = DayFilter.Today)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be higher or equal to 0.");

            DateTime from = DateTime.MinValue, to = DateTime.MinValue;
            switch (filter)
            {

                case DayFilter.Yesterday:
                    from = DateTime.Today.AddDays(-1); to = from;
                    break;
                case DayFilter.Today:
                    from = DateTime.Today; to = from;
                    break;
                case DayFilter.Tomorrow:
                    from = DateTime.Today.AddDays(1); to = from;
                    break;
            }

            var otherParams = new Dictionary<string, string>
            {
                {"fromdate", from.ToString("yyyy-MM-dd") },
                {"todate", to.ToString("yyyy-MM-dd") },
                {"flightdirection", "A" }
            };
            var response = await _webProxy.Get("flights", ResourseVersion.V3, page, otherParams);

            if (response.StatusCode == HttpStatusCode.OK)
                response.Content = JsonConvert.DeserializeObject<ArrivalList>(response.Content.ToString());

            return response;
        }

        public async Task<FlightApiResponse> GetDepartures(int page, string airline)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be higher or equal to 0.");

            if (string.IsNullOrEmpty(airline))
                throw new ArgumentNullException(nameof(airline));

            var otherParams = new Dictionary<string, string>
            {
                {"fromdate", DateTime.Today.ToString("yyyy-MM-dd") },
                {"todate", DateTime.Today.ToString("yyyy-MM-dd") },
                {"flightdirection", "D" },
                {"airline", airline}
            };
            var response = await _webProxy.Get("flights", ResourseVersion.V3, page, otherParams);

            if (response.StatusCode == HttpStatusCode.OK)
                response.Content = JsonConvert.DeserializeObject<DepartureList>(response.Content.ToString());

            return response;
        }
    }
}
