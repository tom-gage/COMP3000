using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace COMP3000Project.Views.Search
{
    class SearchPageViewModel
    {
        public ObservableCollection<sampleEatery> sampleEateries = new ObservableCollection<sampleEatery>();

        public class sampleEatery
        {
            public string EateryName { get; set; }
        }



        public SearchPageViewModel()
        {
            sampleEateries.Add(new sampleEatery { EateryName = "Eatery 0" });
            sampleEateries.Add(new sampleEatery { EateryName = "Eatery 1" });
            sampleEateries.Add(new sampleEatery { EateryName = "Eatery 2" });
        }
    }
}
