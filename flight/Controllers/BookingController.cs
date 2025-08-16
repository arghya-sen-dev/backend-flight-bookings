using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using flight.Data;
using flight.Models;
using flight.Dtos;
using flight.Domain.Errors;
namespace flight.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly Entities _entities;
        public BookingController(Entities entities)
        {
            _entities = entities;
        }
        [HttpGet("{email}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<BookingRm>), StatusCodes.Status200OK)]

        public ActionResult<IEnumerable<BookingRm>> List(string email)
        {
            var bookings = _entities.Flights.ToArray()
                 .SelectMany(f => f.Bookings
                 .Where(b => b.PassengerEmail == email)
                 .Select(b=>new BookingRm(
                     f.Id,
                     f.Airline,
                     f.Price.ToString(),
                     new TimePlaceRm(f.Arrival.Place, f.Arrival.Time),
                     new TimePlaceRm(f.Departure.Place, f.Departure.Time),
                     b.NumberOfSeats,
                     email
                 )));
            return Ok(bookings);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Cancel(BookDto dto)
        {
            var flight = _entities.Flights.Find(dto.FlightId);
            var error =flight?.CancelBooking(dto.PassengerEmail, dto.NumberOfSeats);


            if(error==null)
            {
                _entities.SaveChanges();
                return NoContent();
            }
            if (error is NotFoundError)
            {
                return NotFound(new { message = "Booking not found." });
            }
            throw new Exception($"The error of type:{error.GetType().Name} occured while canceling the booking made by {dto.PassengerEmail}");
        }
    }
}
