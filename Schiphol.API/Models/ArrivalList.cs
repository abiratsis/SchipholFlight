using System.Collections.Generic;

namespace Schiphol.FlightAPI.Models
{
    public class ArrivalList
    {
        public List<Arrival> Flights { get; set; }
        public string SchemaVersion { get; set; }
    }
}
