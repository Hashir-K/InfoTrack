using InfoTrack.Common.DTOs;
using InfoTrack.Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace BookingApi.Services
{
    public class BookingService : IBookingService
    {
        private const int MAX_SETTLEMENTS_PER_BOOKING_TIME = 4;

        public BookingService(IMemoryCache memoryCache) 
        {
            SetupRepository(memoryCache);
        }

        public BookingResponse SaveBookingDetails(BookingRequest bookingRequest)
        {
            var dataKey = $"{bookingRequest.Name}-{bookingRequest.BookingTime}";

            //if already exists then we are assuming it is an update
            if (Repository.ContainsKey(dataKey))
            {
                Repository.Remove(dataKey);
            }

            //check for 4 simultaneous settlements at a booking time
            var bookingTimeSettlements = Repository.Keys.Where(k => k.EndsWith(bookingRequest.BookingTime)).Count();

            if (bookingTimeSettlements == MAX_SETTLEMENTS_PER_BOOKING_TIME)
            {
                throw new ConflictException("All settlements at this time are reserved"); //already 4 in this slot
            }

            var data = new BookingData(bookingRequest);

            Repository.Add(dataKey, data);

            return new BookingResponse(data.BookingId);
        }

        private void SetupRepository(IMemoryCache memoryCache)
        {
            _inMemRespository = memoryCache;

            if (_inMemRespository.Get(CACHE_KEY) == null)
            {
                _inMemRespository.Set(CACHE_KEY, new Dictionary<string, BookingData>());            
            }
        }

        private Dictionary<string, BookingData> Repository
        {
            get
            {
                return (Dictionary<string, BookingData>)_inMemRespository.Get(CACHE_KEY);
            }
        }

        private IMemoryCache _inMemRespository;
        private const string CACHE_KEY = "repo";
    }
}
