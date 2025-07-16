using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Speedometr
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }
      

        async void OnStartListening()
        {
            try
            {
                Geolocation.LocationChanged += Geolocation_LocationChanged;
                var request = new GeolocationListeningRequest(GeolocationAccuracy.Best);
                var success = await Geolocation.StartListeningForegroundAsync(request);
            }
            catch (Exception ex)
            {
                Poloha.Text = ex.Message;
            }
        }

        private void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
        {
            string speed = string.Format("{0:0.00}", e.Location.Speed * 3,6);


            Poloha.Text = $"{speed} KM/H";

        }
        private void StartMesuring_Clicked(object sender, EventArgs e)
        {
            OnStartListening();
        }

    }

}
