﻿using COMP3000Project.Interfaces;
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

        private object _selectedEateryTypeOption;
        public object SelectedEateryTypeOption
        {
            get { return _selectedEateryTypeOption; }
            set
            {
                if (_selectedEateryTypeOption != value)
                {
                    SetProperty(ref _selectedEateryTypeOption, value);//informs view of change
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

            SelectedTime = DateTime.Now.TimeOfDay;

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
            //List<string> eateryTypeOptionsArray = new List<string>();

            //for (int i = 0; i < SelectedEateryTypeOptions.Count; i++)
            //{
            //    eateryTypeOptionsArray.Add(SelectedEateryTypeOptions[i].ToString());
            //}

            if (SelectedLocation == null)
            {
                SelectedLocation = "plymouth, uk";
            }

            if (SelectedEateryTypeOption == null)
            {
                SelectedEateryTypeOption = "restaurant";
            }

            string[] strArr = { SelectedEateryTypeOption.ToString() };

            SearchPage nextPage = new SearchPage(SelectedLocation, SelectedTime.ToString("hhmm"), strArr);

            await Navigation.PushAsync(nextPage, true);

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
