using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using COMP3000Project.Droid;
using COMP3000Project.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]

namespace COMP3000Project.Droid
{
    public class Toast_Android : Interfaces.Toast
    {

        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}