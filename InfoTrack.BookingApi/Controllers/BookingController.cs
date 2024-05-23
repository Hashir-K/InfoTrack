using BookingApi.Services;
using InfoTrack.Common.DTOs;
using InfoTrack.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookingRequest bookingRequest)
        {
            try
            {
                string validationError = bookingRequest.Validate();

                if (!string.IsNullOrEmpty(validationError))
                {
                    return BadRequest(validationError);
                }

                var booking = _bookingService.SaveBookingDetails(bookingRequest);

                return Ok(booking);
            }
            catch (ConflictException cEx)
            {
                return Conflict(cEx.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
