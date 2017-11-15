namespace Schiphol.FlightAPI.Models
{
    public abstract class Flight
    {
        public string FlightName { get; set; }
        public int? Terminal { get; set; }
    }
}
