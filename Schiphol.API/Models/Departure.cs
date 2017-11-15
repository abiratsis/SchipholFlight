using System;
using System.Collections.Generic;

namespace Schiphol.FlightAPI.Models
{
    public class Departure: Flight
    {
        public DestinationList Route { get; set; }
        public string Gate { get; set; }
        public DateTime? ExpectedTimeBoarding { get; set; }
    }
}
