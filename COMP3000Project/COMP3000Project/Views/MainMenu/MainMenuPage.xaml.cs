﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COMP3000Project.Views.MainMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        MainMenuPageViewModel viewModel;
        public MainMenuPage()
        {
            InitializeComponent();

            viewModel = new MainMenuPageViewModel();

            BindingContext = viewModel;
        }
    }
}