using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Data.UnitOfWorks;
using nicenice.Server.Models;
using nicenice.Server.NiceNiceDb;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using nicenice.Server.Hubs;


namespace nicenice.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RidesController : ControllerBase
    {
        private readonly NiceNiceDbContext _context;
        private readonly RideMatchingService _rideMatcher;
        private readonly IHubContext<RideHub> _hubContext;

        public RidesController(NiceNiceDbContext context, RideMatchingService rideMatcher, IHubContext<RideHub> hubContext)
        {
            _context = context;
            _rideMatcher = rideMatcher;
            _hubContext = hubContext;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookRide([FromBody] CreateRideRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (request.PassengerId == Guid.Empty || string.IsNullOrEmpty(request.PickupLocation) || string.IsNullOrEmpty(request.DropoffLocation))
                    return BadRequest("Invalid ride request data.");

                var passenger = await _context.Passengers
                    .FirstOrDefaultAsync(p => p.UserId == request.PassengerId);

                if (passenger == null)
                    return NotFound("Passenger profile not found for this user.");

                var driver = await _context.PassengerDrivers.FirstOrDefaultAsync();

                if (driver == null)
                    return NotFound("No available drivers.");

                var ride = new Ride
                {
                    Id = Guid.NewGuid(),
                    PassengerId = passenger.Id,
                    PickupLocation = request.PickupLocation,
                    DropoffLocation = request.DropoffLocation,
                    EstimatedFare = request.EstimatedFare,
                    Status = RideStatus.Requested,
                    RequestedAt = DateTime.UtcNow,
                };

                _context.Rides.Add(ride);
                await _context.SaveChangesAsync();

                var result = new RideDto
                {
                    RideId = ride.Id,
                    PickupLocation = ride.PickupLocation,
                    DropoffLocation = ride.DropoffLocation,
                    EstimatedFare = ride.EstimatedFare,
                    Status = ride.Status,
                    RequestedAt = ride.RequestedAt
                };

                return CreatedAtAction(nameof(GetRideDetails), new { rideId = ride.Id }, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BookRide error:", ex.Message);
                return StatusCode(500, "Something went wrong while booking the ride.");
            }
        }

        [HttpGet("{rideId}")]
        public async Task<IActionResult> GetRideDetails(Guid rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null)
                return NotFound($"Ride with ID {rideId} not found.");

            var result = new RideDto
            {
                RideId = ride.Id,
                PickupLocation = ride.PickupLocation,
                DropoffLocation = ride.DropoffLocation,
                EstimatedFare = ride.EstimatedFare,
                Status = ride.Status,
                RequestedAt = ride.RequestedAt
            };

            return Ok(result);
        }

        [HttpPost("start/{rideId}")]
        public async Task<IActionResult> StartRide(Guid rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null) return NotFound();

            ride.Status = RideStatus.Started;
            ride.StartedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Ride started.");
        }

        [HttpPost("complete/{rideId}")]
        public async Task<IActionResult> CompleteRide(Guid rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null) return NotFound();

            ride.Status = RideStatus.Completed;
            ride.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok("Ride completed.");
        }

        [HttpPost("cancel/{rideId}")]
        public async Task<IActionResult> CancelRide(Guid rideId)
        {
            var ride = await _context.Rides.FindAsync(rideId);
            if (ride == null) return NotFound();

            ride.Status = RideStatus.Cancelled;
            await _context.SaveChangesAsync();

            await _hubContext.Clients.Group(rideId.ToString()).SendAsync("ReceiveRideUpdate", new
            {
                RideId = rideId,
                Status = "cancelled"
            });

            return Ok("Ride cancelled.");
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRides()
        {
            var pendingRides = await _context.Rides
                .Where(r => r.Status == RideStatus.Requested)
                .Select(r => new RideDto
                {
                    RideId = r.Id,
                    PickupLocation = r.PickupLocation,
                    DropoffLocation = r.DropoffLocation,
                    EstimatedFare = r.EstimatedFare,
                    Status = r.Status,
                    RequestedAt = r.RequestedAt
                })
                .ToListAsync();

            return Ok(pendingRides);
        }

        [HttpPost("respond")]
        public async Task<IActionResult> RespondToRide([FromBody] RideResponseDto dto)
        {
            var ride = await _context.Rides
                .Include(r => r.Passenger)
                .FirstOrDefaultAsync(r => r.Id == dto.RideId);
            if (ride == null) return NotFound();

            if (dto.Accept)
            {
                ride.DriverId = dto.DriverId;
                ride.Status = RideStatus.Matched;

                var passengerId = ride.Passenger?.UserId.ToString();

                var driver = await _context.PassengerDrivers
                    .FirstOrDefaultAsync(d => d.Id == dto.DriverId);
                
                var car = await _context.PassengersCars
                    .Where(c => c.DriverId == dto.DriverId)
                    .OrderByDescending(c => c.CreatedAt)
                    .FirstOrDefaultAsync();

                if (driver == null)
                {
                    return NotFound("Driver profile not found.");
                }

                if (!string.IsNullOrEmpty(passengerId))
                {
                    await _hubContext.Clients.Group(ride.Id.ToString())
                        .SendAsync("ReceiveRideAssigned", new
                        {
                            rideId = ride.Id,
                            status = "matched",
                            pickupLocation = ride.PickupLocation,
                            dropoffLocation = ride.DropoffLocation,
                            estimatedFare = ride.EstimatedFare,
                            driver = new
                            {
                                name = $"{driver.FirstName} {driver.LastName}",
                                gender = driver.Gender,
                                plate = car?.PlateNumber,
                                make = car?.Make,
                                model = car?.Model,
                                vehicleType = car?.VehicleType,
                                profileImageUrl = string.IsNullOrEmpty(driver.ProfileImageUrl)
                                    ? null
                                    : $"{Request.Scheme}://{Request.Host}{driver.ProfileImageUrl}",
                                contact = driver.ContactNumber 
                            }
                        });

                    Console.WriteLine("RideAssigned SignalR event sent successfully.");
                }
                else
                {
                    Console.WriteLine("Cannot send SignalR event: passengerId is null or empty.");
                }
            }
            else
            {
                ride.Status = RideStatus.Cancelled;
                Console.WriteLine($"Ride {ride.Id} was rejected by driver {dto.DriverId}.");
            }

            await _context.SaveChangesAsync();
            return Ok("Response recorded.");
        }

        [HttpGet("offer-next")]
        public async Task<IActionResult> GetNextRideOffer(Guid driverId)
        {
            var offer = await _context.Rides
                .Where(r => r.Status == RideStatus.Requested && r.OfferSentToDriverId == null)
                .OrderBy(r => r.RequestedAt)
                .FirstOrDefaultAsync();

            if (offer == null)
                return NoContent();

            offer.OfferSentToDriverId = driverId;
            offer.OfferedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new RideDto
            {
                RideId = offer.Id,
                PickupLocation = offer.PickupLocation,
                DropoffLocation = offer.DropoffLocation,
                EstimatedFare = offer.EstimatedFare,
                Status = offer.Status,
                RequestedAt = offer.RequestedAt
            });
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetRideStatus(Guid rideId)
        {
            var ride = await _context.Rides
                .Include(r => r.Driver)
                .Include(r => r.Passenger)
                .FirstOrDefaultAsync(r => r.Id == rideId);
            if (ride == null) return NotFound();

            object? driverInfo = null;

            if (ride.DriverId != null)
            {
                var driver = await _context.PassengerDrivers
                    .FirstOrDefaultAsync(d => d.Id == ride.DriverId);

                if (driver != null)
                {
                    var car = await _context.PassengersCars
                        .Where(c => c.DriverId == driver.Id)
                        .OrderByDescending(c => c.CreatedAt)
                        .FirstOrDefaultAsync();

                    driverInfo = new
                    {
                        name = $"{driver.FirstName} {driver.LastName}",
                        gender = driver.Gender,
                        plate = car?.PlateNumber,
                        make = car?.Make,
                        model = car?.Model,
                        vehicleType = car?.VehicleType,
                        profileImageUrl = string.IsNullOrEmpty(driver.ProfileImageUrl)
                            ? null
                            : $"{Request.Scheme}://{Request.Host}{driver.ProfileImageUrl}",
                        contact = driver.ContactNumber 
                    };
                }
            }

            return Ok(new
            {
                status = ride.Status,
                driver = driverInfo,
                pickupLocation = ride.PickupLocation,
                dropoffLocation = ride.DropoffLocation
            });
        }

        [HttpPost("location")]
        public async Task<IActionResult> UpdateDriverLocation([FromBody] DriverLocationDto location)
        {
            if (location == null || location.RideId == Guid.Empty)
                return BadRequest("Invalid location data.");

            var ride = await _context.Rides
                .Include(r => r.Passenger)
                .FirstOrDefaultAsync(r => r.Id == location.RideId);

            if (ride == null || ride.Passenger == null)
                return NotFound("Ride or passenger not found.");

            var passengerId = ride.Passenger.UserId.ToString();

            await _hubContext.Clients.Group(passengerId).SendAsync("ReceiveDriverLocation", new
            {
                latitude = location.Latitude,
                longitude = location.Longitude
            });

            return Ok("Location broadcasted.");
        }

        public class RideResponseDto
        {
            public Guid RideId { get; set; }
            public Guid DriverId { get; set; }
            public bool Accept { get; set; }
        }
        public class DriverLocationDto
        {
            public Guid RideId { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}