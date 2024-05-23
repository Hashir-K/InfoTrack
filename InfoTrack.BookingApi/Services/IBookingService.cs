using InfoTrack.Common.DTOs;

namespace BookingApi.Services
{
    public interface IBookingService
    {        
        BookingResponse SaveBookingDetails(BookingRequest bookingRequest);
    }
}
