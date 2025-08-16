using System.ComponentModel;

namespace flight.Dtos
{
    public record FlightSearchParameters(
        [DefaultValue("08/16/2025 10:30:00 AM")]
        DateTime? FromDate,

        [DefaultValue("08/17/2025 10:30:00 AM")]
        DateTime? ToDate,

        [DefaultValue("Los Angeles")]
        string? From,

        [DefaultValue("Berlin")]
        string? Destination,

        [DefaultValue(1)]
        int NumberOfPassengers);
    
    
}
