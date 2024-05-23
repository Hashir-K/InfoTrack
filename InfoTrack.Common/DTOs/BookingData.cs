using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTrack.Common.DTOs
{
    public class BookingData : BookingRequest
    {
        public BookingData() { }

        public BookingData(BookingRequest bookingRequest) 
        { 
            this.BookingTime = bookingRequest.BookingTime;
            this.Name = bookingRequest.Name;
            this.DateAddedUtc = DateTime.UtcNow;
            this.BookingId = Guid.NewGuid().ToString();

            if (DateTime.TryParse(bookingRequest.BookingTime, out DateTime expiryTime))
            {
                BookingExpiry = expiryTime.TimeOfDay.Add(new TimeSpan(0,59,0));
            }
        }

        public string BookingId { get; set; }

        public DateTime DateAddedUtc { get; set; }

        public TimeSpan BookingExpiry { get; set; }
    }
}
