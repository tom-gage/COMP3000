using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Net.WebSockets;
using System.Threading;
using System.Linq;
using System.ComponentModel;
using System.Text.Json;
using COMP3000Project.TestObjects;
using COMP3000Project.WS;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Essentials;

namespace COMP3000Project.Views.Testing
{
    class TestingPageViewModel : INotifyPropertyChanged
    {
        //COMMANDS
        public ICommand OnSwipeCommand { get; }
        public ICommand OnButtonClick { get; }

        //VARS
        private WebsocketHandler WSHandler;

        public event PropertyChangedEventHandler PropertyChanged;

        //public EateryOption eateryOption;
        public class testData
        {
            public string title { get; set; }
            public string description { get; set; }
        }

        string testText;
        public string TestText
        {
            set
            {
                if (testText != value)
                {
                    testText = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TestText"));
                    }
                }
            }
            get
            {
                return testText;
            }
        }


        ObservableCollection<EateryOption> _testCollection;
        public ObservableCollection<EateryOption> TestCollection { get => _testCollection; set => SetProperty(ref _testCollection, value); }

        //CONSTRUCTOR
        public TestingPageViewModel()
        {
            OnSwipeCommand = new Command(onSwipe);
            OnButtonClick = new Command(SendWSMessage);

            TestCollection = new ObservableCollection<EateryOption>();

            WSHandler = new WebsocketHandler();

            WSHandler.InitialiseConnectionAsync();
        }

        //FUNCTIONS
        public async void SendWSMessage()
        {
            //asks server for eateries
            //Location location = await getLocation();
            //TestCollection = await WSHandler.RequestEateriesList("{ \"type\": \"getEateries\", \"body\": \"\", \"latitude\": \"" + location.Latitude + "\", \"longitude\": \"" + location.Longitude + "\" }");//TEMP SHIT

            WSHandler.SendMessage("testMessage");
        }
        public async Task<Location> getLocation()
        {
            Location location;
            try
            {
                location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

                    return location;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Console.WriteLine(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                Console.WriteLine(fneEx);
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                Console.WriteLine(pEx);
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine(ex);
            }

            Console.WriteLine("LOCATION NOT FOUND");
            return null;
        }
        public async void onSwipe()
        {
            Console.WriteLine("----- DEBUG FLAG -----");
            //string test = await MakeGooglePhotosCall();
        }

        public async void connectToWS()
        {
            ClientWebSocket ws = new ClientWebSocket();

            await ws.ConnectAsync(new Uri("ws://10.0.2.2:9000"), CancellationToken.None);

            var data = "please send me some restaurant data";

            var encodedData = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encodedData, 0, encodedData.Length);
            await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);


            while (true)
            {
                WebSocketReceiveResult result;
                var message = new ArraySegment<byte>(new byte[4096]);
                do
                {
                    result = await ws.ReceiveAsync(message, CancellationToken.None);
                    var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                    string serialisedMessage = Encoding.UTF8.GetString(messageBytes);

                    TestText = serialisedMessage;

                    TestCollection = JsonSerializer.Deserialize<ObservableCollection<EateryOption>>(TestText);

                    //TestCollection.Add(eateryOption);

                } while (!result.EndOfMessage);
            }
        }



        public async Task<string> MakeGooglePhotosCall()//dont think this does anything
        {
            string result = null;
            using (var httpClient = CreateClient())
            {
                var response = await httpClient.GetAsync($"api/place/photo?maxwidth=400&photoreference=CnRtAAAATLZNl354RwP_9UKbQ_5Psy40texXePv4oAlgP4qNEkdIrkyse7rPXYGd9D_Uj1rVsQdWT4oRz4QrYAJNpFX7rzqqMlZw2h2E2y5IKMUZ7ouD_SlcHxYq1yL4KbKUv3qtWgTK0A6QbGh87GB3sscrHRIQiG2RrmU_jF4tENr9wGS_YxoUSSDrYjWmrNfeEHSGSc3FyhNLlBU&key=AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    //var image = await response.Content.

                    if (!string.IsNullOrWhiteSpace(json) && json != "ERROR")
                    {
                        result = json;
                    }
                }
            }
            //AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw
            return result;

        }

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://maps.googleapis.com/maps/")
            };

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }


        //BLACK MAGIC - THOU SHALT NOT TOUCH
        protected void SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            HandlePropertyChanged(propertyName);
        }

        protected void HandlePropertyChanged(string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
