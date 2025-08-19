# backend-flight-bookings

The **flight-booking-system** Nuget package contains API endpoint for Flight,Booking,Passenger records.

**.NET Platform**
The **flight-booking-system** package targets .NET core 9.0.

**Dependecies**

**flight-booking-system** has following packages.

| Package/Coponent                       | Version | Used for                                                           |
| -------------------------------------- | ------- | ------------------------------------------------------------------ |
| Microsoft.EntityFrameworkCore          | 9.0.8   | Object-Relational Mapping (ORM) in .NET applications.              |
| Microsoft.EntityFrameworkCore.SqlServer | 9.0.8   | Provides an sql database provider for Entity Framework Core. |
| Swashbuckle.AspNetCore                 | 9.0.3   | Generating and serving interactive API documentation.              |

**flight-booking-system.Controllers**

The `flight.Controller` namespace provides the logic to handle api calls for flight and passenger information.

**flight-booking-system.Data**

The `flight.Data` namespace contains Entities DbContext which is a common naming convention for a custom DbContext class in applications utilizing Entity Framework Core (EF Core) in C#. It serves as the primary gateway for interacting with a database within an application.

**flight-booking-system.Domain**

The `flight.Domain` namespace contains a specific concept or entity within the business domain of the application.

**flight-booking-system.Models**

The `flight.Models` namespace provides a class for different datatypes that are commonly used throughout the solution.
