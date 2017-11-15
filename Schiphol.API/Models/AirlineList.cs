using System.Collections.Generic;
namespace Schiphol.FlightAPI.Models
{
    public class AirlineList
    {
        public List<Airline> AirLines { get; set; }
        public string SchemaVersion { get; set; }
    }
}
