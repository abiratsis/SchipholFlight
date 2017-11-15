using System.Collections.Generic;
using System.Threading.Tasks;

namespace Schiphol.FlightAPI
{
    public interface IWebProxy
    {
        string APP_ID { get; }
        string APP_KEY { get; }
        string SchipholApiUrl { get; }

        Task<FlightApiResponse> Get(string route, string resourceVersion, int page, Dictionary<string, string> moreParams = null);
    }
}
