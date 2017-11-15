using System.Collections.Generic;
using System.Linq;

namespace Schiphol.FlightAPI.Models
{
    public class FlightStateList
    {
        public static readonly Dictionary<string, string> StatesDetails = new Dictionary<string, string>
        {
            {"SCH", "Flight scheduled Indicates when a Flight (created by the aircraft operator) is scheduled to take place" },
            {"AIR","Airborne Airborne is a flight state at which the flight is airborne. The flight is en route" },
            {"EXP","Expected landing"},
            {"FIR","Flight in Dutch airspace" },
            {"LND","Landed When an aircraft has landed on the runway and is taxiing to the gate" },
            {"FIB","Is a flight state which indicates that the First Bagage of an arriving flight will be on the(luggage) belt very soon" },
            {"ARR","Arrived Flight has been completely handeled" },
            {"DIV", "When the flight is diverted from Amsterdam Airport Schiphol. For those flights the flight state will be changed to DIV and the flight will not arrive at EHAM (Amsterdam Airport Schiphol)" },
            {"CNX","Cancelled A scheduled flight that actually will not be operated" },
            {"TOM","Tomorrow When the date of an expected landing exceeds the initial date" }
        };
        public List<string> FlightStates { get; set; }

        public Dictionary<string, string> FlightStateDetails
        {
            get
            {
                return (Dictionary<string, string>)StatesDetails.Where(sd => FlightStates.Contains(sd.Key));
            }
        }
    }
}
