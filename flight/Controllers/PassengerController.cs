using flight.Domain.Entities;
using flight.Dtos;
using flight.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using flight.Data;

namespace flight.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly Entities _entities;
        public PassengerController(Entities entities)
        {
            _entities = entities;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register(NewPassengerDto dto)
        {
            _entities.Passengers.Add(new Passenger(
                dto.Email,
                dto.FirstName,
                dto.LastName,
                dto.Gender));
            _entities.SaveChanges();
            return CreatedAtAction(nameof(Find), new { email = dto.Email }, dto);
        }

        [HttpGet("{email}")]
        public ActionResult<PassengerRm> Find(string email)
        {
            var passenger = _entities.Passengers.SingleOrDefault(p => p.Email == email);
            if (passenger == null)
            {
                return NotFound($"Passenger with email {email} not found.");
            }
            var rm = new PassengerRm(
                passenger.Email,
                passenger.FirstName,
                passenger.LastName,
                passenger.Gender
                );
            return Ok(rm);
        }
    }
}
