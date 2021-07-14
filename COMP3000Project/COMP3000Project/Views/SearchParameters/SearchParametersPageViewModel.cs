using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace COMP3000Project.Views.SearchParameters
{
    class SearchParametersPageViewModel : ViewModelBase, Subscriber
    {

        //VARIABLES
        private string _selectedLocation;
        public string SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                if (_selectedLocation != value)
                {
                    SetProperty(ref _selectedLocation, value);//informs view of change
                }
            }
        }

        private TimeSpan _selectedTime;
        public TimeSpan SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                if (_selectedTime != value)
                {
                    SetProperty(ref _selectedTime, value);//informs view of change
                }
            }
        }

        private object _selectedEateryOption;
        public object SelectedEateryOption
        {
            get { return _selectedEateryOption; }
            set
            {
                if (_selectedEateryOption != value)
                {
                    SetProperty(ref _selectedEateryOption, value);//informs view of change
                }
            }
        }

        private ObservableCollection<string> _eateryTypeOptions;
        public ObservableCollection<string> EateryTypeOptions
        {
            get
            {
                return _eateryTypeOptions;
            }
            set
            {
                if (_eateryTypeOptions != value)
                {
                    SetProperty(ref _eateryTypeOptions, value);//informs view of change
                }
            }
        }

        private ObservableCollection<object> _selectedEateryTypeOptions;
        public ObservableCollection<object> SelectedEateryTypeOptions
        {
            get
            {
                return _selectedEateryTypeOptions;
            }
            set
            {
                if (_selectedEateryTypeOptions != value)
                {
                    SetProperty(ref _selectedEateryTypeOptions, value);//informs view of change
                }
            }
        }

        //COMMANDS
        public ICommand GoToStartSearch { get; }

        //CONSTRUCTOR
        public SearchParametersPageViewModel()
        {
            //set vars
            SelectedLocation = "plymouth, uk";
            EateryTypeOptions = new ObservableCollection<string>();
            SelectedEateryTypeOptions = new ObservableCollection<object>();

            EateryTypeOptions.Add("bar");
            EateryTypeOptions.Add("cafe");
            EateryTypeOptions.Add("restaurant");
            EateryTypeOptions.Add("meal_takeaway");
            EateryTypeOptions.Add("bakery");
            EateryTypeOptions.Add("meal_delivery");

            //set commands
            GoToStartSearch = new Command(async () => await ExecuteGoToSearchPage());

            WebsocketHandler.registerSubscriber(this);
            //WebsocketHandler.HandleMessages();
        }

        //FUNCTIONS
        public bool ValidateSearchCode(string searchCode)
        {
            if (searchCode == null)
            {
                return false;
            }
            else if (searchCode == "")
            {
                return false;
            }
            else if (searchCode.Length < 1)
            {
                return false;
            }

            return true;
        }

        async Task<object> ExecuteGoToSearchPage()
        {
            List<string> eateryTypeOptionsArray = new List<string>();

            for (int i = 0; i < SelectedEateryTypeOptions.Count; i++)
            {
                eateryTypeOptionsArray.Add(SelectedEateryTypeOptions[i].ToString());
            }

            Console.WriteLine("BEGIN TEST:");
            Console.WriteLine(SelectedLocation);
            Console.WriteLine(SelectedTime);
            Console.WriteLine(JsonConvert.SerializeObject(SelectedEateryOption));
            Console.WriteLine(SelectedEateryOption);

            //SearchPage nextPage = new SearchPage(SelectedLocation, SelectedTime, eateryTypeOptionsArray.ToArray());

            //await Navigation.PushAsync(nextPage, true);

            return null;
        }

        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                default:
                    Console.WriteLine("[MSG] searchpage viewmodel  recieved unknown message");
                    break;
            }
        }
    }
}
