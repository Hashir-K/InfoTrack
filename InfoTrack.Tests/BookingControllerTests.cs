using BookingApi.Services;
using InfoTrack.Common.DTOs;
using InfoTrack.Common.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace BookingSystem.Tests
{
    public class BookingServiceTests
    {
        private readonly IMemoryCache _memoryCache;
        private readonly BookingService _service;

        public BookingServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _service = new BookingService(_memoryCache);
        }

        [Fact]
        public void SaveBookingDetails_NewBooking_SavesSuccessfully()
        {
            // Arrange
            var bookingRequest = new BookingRequest { Name = "John", BookingTime = "2023-05-24T15:00:00" };

            // Act
            var response = _service.SaveBookingDetails(bookingRequest);

            // Assert
            Assert.NotNull(response);
            Assert.True(_memoryCache.TryGetValue("repo", out Dictionary<string, BookingData> repository));
            Assert.Single(repository);
        }

        [Fact]
        public void SaveBookingDetails_UpdateBooking_SavesSuccessfully()
        {
            // Arrange
            var bookingRequest = new BookingRequest { Name = "John", BookingTime = "2023-05-24T15:00:00" };
            _service.SaveBookingDetails(bookingRequest);

            // Act
            var updatedResponse = _service.SaveBookingDetails(bookingRequest);

            // Assert
            Assert.NotNull(updatedResponse);
            Assert.True(_memoryCache.TryGetValue("repo", out Dictionary<string, BookingData> repository));
            Assert.Single(repository);
        }

        [Fact]
        public void SaveBookingDetails_MaxSettlementsReached_ThrowsConflictException()
        {
            // Arrange
            for (int i = 1; i <= 4; i++)
            {
                var bookingRequest = new BookingRequest { Name = $"User{i}", BookingTime = "2023-05-24T15:00:00" };
                _service.SaveBookingDetails(bookingRequest);
            }

            var newBookingRequest = new BookingRequest { Name = "John", BookingTime = "2023-05-24T15:00:00" };

            // Act & Assert
            var exception = Assert.Throws<ConflictException>(() => _service.SaveBookingDetails(newBookingRequest));
            Assert.Equal("All settlements at this time are reserved", exception.Message);
        }
    }
}
