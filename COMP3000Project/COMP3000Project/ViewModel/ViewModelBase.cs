using COMP3000Project.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace COMP3000Project.ViewModel
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		string _title = "";
		public string PageTitle
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		protected INavigation Navigation => Application.Current.MainPage.Navigation;

		public void ShowToast(string message)//shows a toast message, used to provide feedback to the user
        {
			DependencyService.Get<Toast>().Show(message);
		}


        //prefered text sizes 
        private double veryLargeText;
        public double VeryLarge
        {
            get { return veryLargeText; }
            set
            {
                if (veryLargeText != value)
                {
                    SetProperty(ref veryLargeText, value);
                }
            }
        }


        private double largeText;
        public double Large
        {
            get { return largeText; }
            set
            {
                if (largeText != value)
                {
                    SetProperty(ref largeText, value);
                }
            }
        }


        private double mediumText;
        public double Medium
        {
            get { return mediumText; }
            set
            {
                if (mediumText != value)
                {
                    SetProperty(ref mediumText, value);
                }
            }
        }


        private double smallText;
        public double Small
        {
            get { return smallText; }
            set
            {
                if (smallText != value)
                {
                    SetProperty(ref smallText, value);
                }
            }
        }


        //informs view of changes made to properties
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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
