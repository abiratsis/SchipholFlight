using System;

namespace Schiphol.FlightAPI.Models
{
    public class Arrival : Flight
    {
        public DateTime? EstimatedLandingTime { get; set; }
        public DateTime? ActualLandingTime { get; set; }

        public FlightStateList PublicFlightState{ get; set; }
    }
}
