using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using COMP3000Project.ViewModel;
using COMP3000Project.WS;
using Xamarin.Forms;

namespace COMP3000Project.Views.SignUp
{
    class SignUpPageViewModel : ViewModelBase
    {
        //VARIABLES
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    SetProperty(ref _username, value);//informs view of change
                }
            }
        }

        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    SetProperty(ref _password, value);//informs view of change
                }
            }
        }

        //COMMANDS
        public ICommand RegisterNewUser { get; }

        //CONSTRUCTOR
        public SignUpPageViewModel()
        {
            Username = "martin";
            Password = "slime man";

            RegisterNewUser = new Command(async () => await ExecuteRegisterUser());
        }

        //FUNCTIONS
        async Task<object> ExecuteRegisterUser()
        {
            WebsocketHandler.RequestRegisterNewUser(Username, Password);

            return null;
        }
    }
}
