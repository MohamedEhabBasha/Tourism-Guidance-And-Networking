using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs;
using Tourism_Guidance_And_Networking.Core.DTOs.Booking;
using Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs;
using Tourism_Guidance_And_Networking.Core.Helper;
using Tourism_Guidance_And_Networking.Core.Models;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Web.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly StripeSettings _stripeSettings;

        public ReservationController(IUnitOfWork unitOfWork, IOptions<StripeSettings> stripeSettings)
        {
            _unitOfWork = unitOfWork;
            _stripeSettings = stripeSettings.Value;
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetReservations()
        {
            var reservationsFromDb = await _unitOfWork.Reservations.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            double total = 0;
            foreach (var reservation in reservationsFromDb)
            {
                total += reservation.Price;
            }
            var reservationsDto = new AllReservationDTO()
            {
                ReservationList = reservationsFromDb,
                TotalPrice= total
            };
            return Ok(reservationsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetReservationById(int id)
        {

            var reservation = await _unitOfWork.Reservations.GetByIdAsync(id);

            if (reservation == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reservation);
        }

        [HttpGet("getReservationOfUser")]
        public async Task<IActionResult> GetReservationByUserName()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var reservationsFromDb = await _unitOfWork.Reservations.FindAllAsync(r => r.ApplicationUserId == applicationUser.Id,new string[] {"Room", "Accommodation" });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            double total = 0;
            foreach (var reservation in reservationsFromDb)
            {
                total += reservation.Price;
            }
            var reservationsDto = new AllReservationDTO()
            {
                ReservationList = reservationsFromDb,
                TotalPrice = total
            };
            return Ok(reservationsDto);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            SummaryDto summaryDto = new SummaryDto();
            var reservationsFromDb = await _unitOfWork.Reservations.FindAllAsync(r => r.ApplicationUserId == applicationUser.Id,new string[] {"Room", "Accommodation" });
            summaryDto.ReservationList = reservationsFromDb;
            summaryDto.BookingHeader = new();

            summaryDto.BookingHeader.ApplicationUserId = applicationUser.Id;
            summaryDto.BookingHeader.ApplicationUser = applicationUser;
            summaryDto.BookingHeader.Name = applicationUser.FirstName + " " + applicationUser.LastName;
            summaryDto.BookingHeader.Email = applicationUser.Email;
            summaryDto.BookingHeader.PhoneNumber = applicationUser.PhoneNumber;
            summaryDto.BookingHeader.Address = applicationUser.Address;

            foreach (var reservation in summaryDto.ReservationList)
            {
                //reservation.Price = reservation.Room.Price;
                summaryDto.BookingHeader.BookingTotalPrice += reservation.Price;
            }
            return Ok(summaryDto);
        }
        [HttpPost("room")]
        public async Task<IActionResult> MakeRoomReservation(CreateReservationDTO reservationDTO)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var room = await _unitOfWork.Rooms.GetByIdAsync(reservationDTO.AccommodationId);

            if (room is null)
                return NotFound();

            if (!(reservationDTO.Count <= (room.Count - room.CountOfReserved)))
                return BadRequest($"There is no enough number of chosen room, availble number is {room.Count - room.CountOfReserved}");
   
           var reservationFromDb = await _unitOfWork.Reservations.FindAsync(r => r.RoomId == reservationDTO.AccommodationId &&
           r.ApplicationUserId == applicationUser.Id);


            Reservation reservation = new();
            if (reservationFromDb is null)
            {
                reservation.RoomId = reservationDTO.AccommodationId;
                reservation.AccommodationId = null;
                reservation.ApplicationUserId = applicationUser.Id;
                reservation.Room = room;
                reservation.Count = reservationDTO.Count;
                reservation.Price = room.Price * reservationDTO.Count;
                await _unitOfWork.Reservations.AddAsync(reservation);

                if (!(_unitOfWork.Complete() > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok(reservation);
                }
            }
            else
            {
                if (!((reservationDTO.Count + reservationFromDb.Count) <= (room.Count - room.CountOfReserved)))
                    return BadRequest($"There is no enough number of chosen room, availble number is {room.Count - room.CountOfReserved}");
                var resut = await _unitOfWork.Reservations.Increment(reservationFromDb, reservationDTO.Count);

                if (!(resut > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok(reservationFromDb);
                }
            }
        }


        [HttpPost("accommodation")]
        public async Task<IActionResult> MakeAccommodationReservation(CreateReservationDTO reservationDTO)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var accommdation = await _unitOfWork.Accommodations.GetByIdAsync(reservationDTO.AccommodationId);

            if(accommdation is null)
                return NotFound();

            if (!(reservationDTO.Count <= (accommdation.Count - accommdation.CountOfReserved)))
                return BadRequest($"There is no enough number of chosen Accomdation, availble number is {accommdation.Count - accommdation.CountOfReserved}");

            var reservationFromDb = await _unitOfWork.Reservations.FindAsync(r => r.AccommodationId == reservationDTO.AccommodationId &&
            r.ApplicationUserId == applicationUser.Id);


            Reservation reservation = new();
            if (reservationFromDb is null)
            {
                reservation.RoomId = null;
                reservation.AccommodationId = reservationDTO.AccommodationId;
                reservation.ApplicationUserId = applicationUser.Id;
                reservation.Accommodation = accommdation;
                reservation.Count = reservationDTO.Count;
                reservation.Price = accommdation.Price * reservationDTO.Count;
                await _unitOfWork.Reservations.AddAsync(reservation);

                if (!(_unitOfWork.Complete() > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok(reservation);
                }
            }
            else
            {
                if (!((reservationDTO.Count + reservationFromDb.Count) <= (accommdation.Count - accommdation.CountOfReserved)))
                    return BadRequest($"There is no enough number of chosen room, availble number is {accommdation.Count - accommdation.CountOfReserved}");
                var resut = await _unitOfWork.Reservations.Increment(reservationFromDb, reservationDTO.Count);

                if (!(resut > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok(reservationFromDb);
                }
            }
        }



        [HttpPost("plus/{reservationId}")]
        public async Task<IActionResult> Plus(int reservationId)
        {
            if(!(_unitOfWork.Reservations.Exist(reservationId)))
                return NotFound();

            var reservationDb = await _unitOfWork.Reservations.FindAsync(r=>r.Id==reservationId,new string[] {"Room", "Accommodation" });

            if(reservationDb.RoomId is not null)
            {
                // Room Increment
                if (((reservationDb.Room.Count - reservationDb.Room.CountOfReserved) > 1) && ((reservationDb.Count+1)<= (reservationDb.Room.Count - reservationDb.Room.CountOfReserved)))
                {
                    if (await _unitOfWork.Reservations.Increment(reservationDb, 1) > 0)
                        return Ok(reservationDb);
                    else
                    {
                        ModelState.AddModelError("", "Something Went Wrong While Saving");
                        return StatusCode(500, ModelState);
                    }
                }
                else
                {
                    return BadRequest($"There is no enough number of chosen room, availble number is {reservationDb.Room.Count - reservationDb.Room.CountOfReserved}");
                }
            }
            else
            {
                // Accommodation Increment 
                if (((reservationDb.Accommodation.Count - reservationDb.Accommodation.CountOfReserved) > 1) && ((reservationDb.Count + 1) <= (reservationDb.Accommodation.Count - reservationDb.Accommodation.CountOfReserved)))
                {
                    if (await _unitOfWork.Reservations.Increment(reservationDb, 1) > 0)
                        return Ok(reservationDb);
                    else
                    {
                        ModelState.AddModelError("", "Something Went Wrong While Saving");
                        return StatusCode(500, ModelState);
                    }
                }
                else
                {
                    return BadRequest($"There is no enough number of chosen Accommodation, availble number is {reservationDb.Accommodation.Count - reservationDb.Accommodation.CountOfReserved}");
                }
            }

            
        }
        [HttpPost("minus/{reservationId}")]
        public async Task<IActionResult> Minus(int reservationId)
        {
            if (!(_unitOfWork.Reservations.Exist(reservationId)))
                return NotFound();

            var reservationDb = await _unitOfWork.Reservations.FindAsync(r => r.Id == reservationId, new string[] { "Room", "Accommodation" });

            if (reservationDb.Count <= 1)
            {
                _unitOfWork.Reservations.Delete(reservationDb);
                if (_unitOfWork.Complete() > 0)
                    return Ok("Reservation Deleted as count now is zero");
                else
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
            }
            else
            {
                if (await _unitOfWork.Reservations.Decrement(reservationDb, 1) > 0)
                    return Ok(reservationDb);
                else
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }

            }
        }

        [HttpPut("{reservationId:int}")]
        public IActionResult UpdateReservation([FromRoute] int reservationId, [FromBody] UpdateReservationDTo updatedReservation)
        {
            if (updatedReservation == null)
                return BadRequest(ModelState);

            if (!_unitOfWork.Reservations.Exist(reservationId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reservationDb = _unitOfWork.Reservations.GetById(reservationId);

        
            reservationDb.RoomId = updatedReservation.RoomId;

            reservationDb.AccommodationId = updatedReservation.AccommodationId;

            reservationDb.Count = updatedReservation.Count;

            reservationDb.Price = updatedReservation.Price;

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while Deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Updated Successfully");
        }

        [HttpDelete("{reservationId:int}")]
        public IActionResult DeleteReservation([FromRoute] int reservationId)
        {

            if (!_unitOfWork.Reservations.Exist(reservationId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reservationDb = _unitOfWork.Reservations.GetById(reservationId);
            _unitOfWork.Reservations.Delete(reservationDb);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while Deleting");
                return StatusCode(500, ModelState);
            }
            return Ok("Deleted Successfully");
        }
    }
}
