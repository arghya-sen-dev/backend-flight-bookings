using flight.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using flight.Dtos;
using flight.Domain.Entities;
using flight.Domain.Errors;
using flight.Data;
using Microsoft.EntityFrameworkCore;

namespace flight.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly Entities _entities ;
        private readonly ILogger<FlightController> _logger;

        
        public FlightController(ILogger<FlightController> logger,
            Entities entities)
        {
            _logger = logger;
            _entities = entities;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(IEnumerable<Flight>), StatusCodes.Status200OK)]
        [HttpGet]
        public IEnumerable<FlightRm> Search()
        {
            var flightRmLists=_entities.Flights.Select(flight=>new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.RemainingNumberOfSeats
            ));
            return flightRmLists;
        }
             
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Flight),StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public ActionResult<FlightRm> Find(Guid id)
        {
            var flight = _entities.Flights.SingleOrDefault(f => f.Id == id);
            if (flight == null)
                return NotFound();
            var readModel=new FlightRm(
                flight.Id,
                flight.Airline,
                flight.Price,
                new TimePlaceRm(flight.Departure.Place.ToString(), flight.Departure.Time),
                new TimePlaceRm(flight.Arrival.Place.ToString(), flight.Arrival.Time),
                flight.RemainingNumberOfSeats
            );
            return Ok(readModel);

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Book(BookDto dto)
        {
            System.Diagnostics.Debug.WriteLine($"Flight booked for {dto.PassengerEmail} with {dto.NumberOfSeats} seats.");
            var flight= _entities.Flights.SingleOrDefault(f => f.Id == dto.FlightId);
            if(flight==null)
            {
                return NotFound($"Flight with ID {dto.FlightId} not found.");
            }
            var error=flight.MakeBooking(dto.PassengerEmail, dto.NumberOfSeats);
            if(error is OverBookError)
            {
                return Conflict(new { message ="Not enough seats."});
            }
            try
            {
                _entities.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { message = "An error occured while save changes. please try again" });
            }

            return CreatedAtAction(nameof(Find), new { id = dto.FlightId }, dto);
        }
    }
}
