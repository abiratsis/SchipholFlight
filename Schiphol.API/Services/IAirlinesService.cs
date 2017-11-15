using System.Collections.Generic;
using System.Threading.Tasks;
using Schiphol.FlightAPI.Models;

namespace Schiphol.FlightAPI.Services
{
    public interface IAirlinesService
    {
        Task<FlightApiResponse> GetAirlines(int page);
    }
}
