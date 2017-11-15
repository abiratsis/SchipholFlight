using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Schiphol.FlightAPI.Models;

namespace Schiphol.FlightAPI.Services
{
    public class AirlinesService : IAirlinesService
    {
        private readonly IWebProxy _webProxy;

        public AirlinesService(IWebProxy webProxy)
        {
            if(webProxy == null)
                throw new ArgumentNullException(nameof(webProxy));

            _webProxy = webProxy;
        }

        public async Task<FlightApiResponse> GetAirlines(int page)
        {
            if (page < 0)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Page should be higher or equal to 0.");

            var response = await _webProxy.Get("airlines", ResourseVersion.V1, page);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.Content = JsonConvert.DeserializeObject<AirlineList>(response.Content.ToString());
            }

            return response;
        }
    }
}
