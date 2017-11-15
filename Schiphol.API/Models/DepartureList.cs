using System.Collections.Generic;

namespace Schiphol.FlightAPI.Models
{
    public class DepartureList
    {
        public List<Departure> Flights { get; set; }
        public string SchemaVersion { get; set; }
    }
}
