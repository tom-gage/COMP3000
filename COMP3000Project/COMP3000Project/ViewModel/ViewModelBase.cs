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

		public void ShowToast(string message)
        {
			DependencyService.Get<Toast>().Show(message);
		}


		string veryLargeText;
		public string VeryLargeText
		{
			get => veryLargeText;
			set => SetProperty(ref veryLargeText, value);
		}

		string largeText;
		public string LargeText
		{
			get => largeText;
			set => SetProperty(ref largeText, value);
		}

		string mediumText;
		public string MediumText
		{
			get => mediumText;
			set => SetProperty(ref mediumText, value);
		}

		string smallText;
		public string SmallText
		{
			get => smallText;
			set => SetProperty(ref smallText, value);
		}


		//magic
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
