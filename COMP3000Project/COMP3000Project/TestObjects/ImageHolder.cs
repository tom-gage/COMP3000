using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace COMP3000Project.TestObjects
{
    class ImageHolder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ImageSource _image;

        public ImageSource Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_image == value)
                {
                    return;
                }
                _image = value;
                HandlePropertyChanged();
            }
        }

        public ImageHolder(ImageSource imageSource)
        {
            Image = imageSource;
        }

        void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);

            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
