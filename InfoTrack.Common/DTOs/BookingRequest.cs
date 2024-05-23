using System.Xml.Linq;

namespace InfoTrack.Common.DTOs
{
    public class BookingRequest
    {
        public string BookingTime { get; set; }
        public string Name { get; set; }

        private readonly TimeSpan BookingStartTime = new TimeSpan(9, 0, 0);
        private readonly TimeSpan BookingEndTime = new TimeSpan(16, 0, 0);

        public string Validate()
        {
            string validationError = string.Empty;
            
            //validate name
            bool isValid = !string.IsNullOrWhiteSpace(Name);
            if (!isValid) validationError = "Invalid name.";

            //validate acceptance time
            if (!TimeSpan.TryParse(BookingTime, out TimeSpan bookingTime))
            {
                validationError = "Invalid booking time.";
            }

            if (bookingTime < BookingStartTime || bookingTime > BookingEndTime)
            {
                validationError = "Booking time is between 9:00 AM to 4:00 PM.";
            }

            return validationError;
        }

    }
}
