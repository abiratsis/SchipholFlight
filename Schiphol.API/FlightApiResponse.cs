using System.Net;

namespace Schiphol.FlightAPI
{
    public class FlightApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Content { get; set; }
        public string ErrorMessage { get; set; }
    }
}
