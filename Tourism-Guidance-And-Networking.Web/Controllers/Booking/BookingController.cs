using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Security.Claims;
using Tourism_Guidance_And_Networking.Core.Consts;
using Tourism_Guidance_And_Networking.Core.DTOs.Booking;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.Web.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAnyOrigin")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetBookings([FromQuery]string status)
        {
            var bookingsDb = await _unitOfWork.BookingHeaders.GetAllAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            switch (status)
            {
                case "pending":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.Pending);
                    break;
                case "approved":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.Approved);
                    break;
                case "inprocess":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.InProcess);
                    break;
                case "completed":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.Completed);
                    break;
                default:
                    break;
            }
            return Ok(bookingsDb);
        }

        [HttpGet("getBookingsOfUser")]
        public async Task<IActionResult> GetBookingsOfUser([FromQuery] string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            var bookingsDb = await _unitOfWork.BookingHeaders.FindAllAsync(r => r.ApplicationUserId == applicationUser.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            switch (status)
            {
                case "pending":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.Pending);
                    break;
                case "approved":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.Approved);
                    break;
                case "inprocess":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.InProcess);
                    break;
                case "completed":
                    bookingsDb = bookingsDb.Where(u => u.BookingStatus == BookingStatus.Completed);
                    break;
                default:
                    break;
            }
            return Ok(bookingsDb);
        }


        [HttpGet("BookingDetails/{bookingId}")]
        public async Task<IActionResult> BookingDetails(int bookingId)
        {
            if (!_unitOfWork.BookingHeaders.Exist(bookingId))
                return NotFound();

            BookingDetailsDTO bookingDetailsDTO = new BookingDetailsDTO()
            {
                BookingHeader = await _unitOfWork.BookingHeaders.FindAsync(bh => bh.Id == bookingId, new string[] { "ApplicationUser" }),
                BookingDetails = await _unitOfWork.BookingDetails.FindAllAsync(bd => bd.BookingHeaderId == bookingId, new string[] { "Room", "Accommodation" })
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(bookingDetailsDTO);
        }


        [HttpPost("StartProcessing/{bookingId:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> StartProcessing(int bookingId)
        {
            var bookingDb = await _unitOfWork.BookingHeaders.GetByIdAsync(bookingId);

            if(bookingDb == null)
                return NotFound();

            if((bookingDb.BookingStatus!=BookingStatus.Approved))
                return BadRequest("Booking is not approved yet!, you must make payment first");


            await _unitOfWork.BookingHeaders.UpdateStatus(bookingId, BookingStatus.InProcess);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok(bookingDb);
            }
               
        }
        [HttpPost("CompleteBooking/{bookingId:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CompleteBooking(int bookingId)
        {
            var bookingDb = await _unitOfWork.BookingHeaders.GetByIdAsync(bookingId);

            if (bookingDb == null)
                return NotFound();

            if ((bookingDb.BookingStatus != BookingStatus.InProcess))
                return BadRequest("Booking is not inprocessing yet!, Admin must process the booking first");

            await _unitOfWork.BookingHeaders.UpdateStatus(bookingId, BookingStatus.Completed);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }
            else
            {
                return Ok(bookingDb);
            }
        }


        [HttpPost("CancelBooking/{bookingId:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CancelOrder(int bookingId)
        {
            var bookingDb = await _unitOfWork.BookingHeaders.GetByIdAsync(bookingId);

            if (bookingDb == null)
                return NotFound();

            if (bookingDb.PaymentStatus == PaymentStatus.Approved)
            {
                var options = new RefundCreateOptions()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = bookingDb.PaymentIntentId // this will refund the exact amount in the payment
                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                await _unitOfWork.BookingHeaders.UpdateStatus(bookingDb.Id, BookingStatus.Cancelled, PaymentStatus.Refunded);

                if (!(_unitOfWork.Complete() > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok($"Booking Number {bookingDb.Id} is Deleted and Money is refunded successfully");
                }
            }
            else
            {
                await  _unitOfWork.BookingHeaders.UpdateStatus(bookingDb.Id, BookingStatus.Cancelled, PaymentStatus.Cancelled);

                if (!(_unitOfWork.Complete() > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                else
                {
                    return Ok($"Booking Number {bookingDb.Id} is Deleted");
                }
            }

            
        }


        [HttpPost("MakeBooking")]
        public async Task<IActionResult> MakeBooking([FromQuery]string successUrl, [FromQuery] string cancelUrl)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.GetApplicationUserByUserName(userName);

            SummaryDto summaryDto = new SummaryDto();
            var reservationsFromDb = await _unitOfWork.Reservations.FindAllAsync(r => r.ApplicationUserId == applicationUser.Id, new string[] { "Room", "Accommodation" });

            if(!(reservationsFromDb.Count() >0) )
            {
                return NotFound("You Must Make a reservation first");
            }

            summaryDto.ReservationList = reservationsFromDb;
            summaryDto.BookingHeader = new();


            summaryDto.BookingHeader.ApplicationUserId = applicationUser.Id;
            summaryDto.BookingHeader.ApplicationUser = applicationUser;
            summaryDto.BookingHeader.Name = applicationUser.FirstName + " " + applicationUser.LastName;
            summaryDto.BookingHeader.Email = applicationUser.Email;
            summaryDto.BookingHeader.PhoneNumber = applicationUser.PhoneNumber;
            summaryDto.BookingHeader.Address = applicationUser.Address;
            summaryDto.BookingHeader.BookingDate = DateTime.Now;
            summaryDto.BookingHeader.PaymentDate = null;
            summaryDto.BookingHeader.CompleteDate = null;

            summaryDto.BookingHeader.PaymentStatus = PaymentStatus.Pending;
            summaryDto.BookingHeader.BookingStatus = BookingStatus.Pending;


            foreach (var reservation in summaryDto.ReservationList)
            {
                if (!(reservation.Count <= (reservation.Room.Count - reservation.Room.CountOfReserved)))
                    return BadRequest($"There is no enough number of chosen rooms");
                summaryDto.BookingHeader.BookingTotalPrice += reservation.Price;
            }
            await _unitOfWork.BookingHeaders.AddAsync(summaryDto.BookingHeader);
            _unitOfWork.Complete();

            foreach (var reservation in summaryDto.ReservationList)
            {
                BookingDetail orderDetail = new BookingDetail()
                {
                    BookingHeaderId = summaryDto.BookingHeader.Id,
                    RoomId = reservation.RoomId,
                    Price = reservation.Price,
                    Count = reservation.Count
                };
                await _unitOfWork.BookingDetails.AddAsync(orderDetail);
            }
            _unitOfWork.Complete();

            // Stripe Settings

            //var domain = "http://localhost:5248/";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            foreach (var item in summaryDto.ReservationList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Room.Price * 100),//20.00 -> 2000
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Room.Info,
                            Description = item.Room.Info,
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            await _unitOfWork.BookingHeaders.UpdateSessionAndPaymentId(summaryDto.BookingHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Complete();

            SummaryResultDTO summaryResultDTO = new()
            {
                SessionId = session.Id,
                SessionUrl = session.Url,
                BookingNumber = summaryDto.BookingHeader.Id
            };
            return Ok(summaryResultDTO);
        }

        [HttpPost("BookingConfirmation/{id:int}")]
        public async Task<IActionResult> BookingConfirmation(int id)
        {
            var BookingHeaderDb = await _unitOfWork.BookingHeaders.GetByIdAsync(id);
            var service = new SessionService();
            Session session = service.Get(BookingHeaderDb.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                await _unitOfWork.BookingHeaders.UpdateStatus(BookingHeaderDb.Id, BookingStatus.Approved, PaymentStatus.Approved);
                await _unitOfWork.BookingHeaders.UpdateSessionAndPaymentId(id, session.Id, session.PaymentIntentId);

                BookingHeaderDb.PaymentDate = DateTime.Now;

                _unitOfWork.Complete();

                IEnumerable<Reservation> reservations = await _unitOfWork.Reservations.FindAllAsync(r => r.ApplicationUserId == BookingHeaderDb.ApplicationUserId, new string[] { "Room", "Accommodation" });
                foreach (Reservation reservation in reservations)
                {
                    reservation.Room.CountOfReserved += reservation.Count;
                }
                List<Reservation> reservationList = reservations.ToList();
                _unitOfWork.Reservations.DeleteRange(reservations);
                _unitOfWork.Complete();
                return Ok(id);
            }
            else
            {
                return BadRequest("Payment Have not been done");
            }
        }

    }
}
