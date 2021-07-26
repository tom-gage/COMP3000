﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.SearchParameters
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchParametersPage : ContentPage
    {
        SearchParametersPageViewModel viewModel;
        public SearchParametersPage()
        {
            InitializeComponent();

            viewModel = new SearchParametersPageViewModel();
            BindingContext = viewModel;
        }

        public SearchParametersPage(string location, string time, string[] eateryTypes)
        {
            InitializeComponent();

            viewModel = new SearchParametersPageViewModel(location, time, eateryTypes);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}