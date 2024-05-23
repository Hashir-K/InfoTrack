using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InfoTrack.Common.DTOs;
using InfoTrack.Common.Helpers;
using System.Configuration;
using System.Windows;

namespace InfoTrack.Client.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        public MainWindowVM()
        {
            BookingRequest = new BookingRequest();

            BookingVisibility = Visibility.Hidden;
        }

        [ObservableProperty] public BookingRequest bookingRequest;

        [ObservableProperty] public string information;

        [ObservableProperty] public Visibility bookingVisibility;

        public Window VMWindow { get; set; }

        [RelayCommand]
        private async Task Book()
        {
            try
            {
                //reset previous info
                Information = String.Empty;

                //validate search params
                ValidateBookingRequest();

                //show information
                BookingVisibility = Visibility.Visible;

                //performs search
                var apiClient = new ApiClient();                
                var response = await apiClient.Post<BookingResponse>($"{BookingEndpoint}/booking", BookingRequest);

                //display results
                Information += $"Booking Id: {response.BookingId}{Environment.NewLine}";

            }
            catch (Exception ex)
            {
                //show user friendly message instead of technical one
                MessageBox.Show(ex.Message); 
            }
            finally
            {
                //hide searching message
                BookingVisibility = Visibility.Hidden;
            }
        }

        private string BookingEndpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["BookingEndpoint"].ToString();
            }
        }

        private void ValidateBookingRequest()
        {
            if (String.IsNullOrEmpty(BookingRequest.Name))
            {
                throw new Exception("Please enter a booking name.");
            }

            if (String.IsNullOrEmpty(BookingRequest.BookingTime))
            {
                throw new Exception("Please a booking time.");
            }

            try
            {
                var time = BookingRequest.BookingTime.Split(":".ToCharArray());

                if (time[0].Length != 2 || time[1].Length != 2)
                {
                    throw new Exception();
                }

                if (!DateTime.TryParse(BookingRequest.BookingTime, out DateTime t))
                {
                    throw new Exception();
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Please enter booking time in a 24 hour (HH:mm) format");
            }
        }

    }
}
