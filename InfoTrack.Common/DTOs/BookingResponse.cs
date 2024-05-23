using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTrack.Common.DTOs
{
    public class BookingResponse
    {
        public BookingResponse() { }

        public BookingResponse(string bookingId) 
        {
            BookingId = bookingId;
        }

        public string BookingId {get;set;}
    }
}
