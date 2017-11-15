using System.Threading.Tasks;
using Schiphol.FlightAPI.Models;

namespace Schiphol.FlightAPI.Services
{
    public interface IFlightsService
    {
        Task<FlightApiResponse> GetArrivals(int page, DayFilter filter = DayFilter.Today);

        Task<FlightApiResponse> GetDepartures(int page, string airline);
    }
}
