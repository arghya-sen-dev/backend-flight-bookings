namespace flight.Models
{
    public record BookingRm(
        Guid FlightId,
        string AirLine,
        string Price,
        TimePlaceRm Arrival,
        TimePlaceRm Departure,
        int NumberOfBookedSeats,
        string PassengerEmail);
    
    
}
