using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using COMP3000Project.ViewModel;
using COMP3000Project.WS;
using COMP3000Project.Views.SignUp;
using COMP3000Project.Interfaces;
using Newtonsoft.Json;
using COMP3000Project.TestObjects;
using COMP3000Project.Views.MainMenu;
using COMP3000Project.UserDetailsSingleton;

namespace COMP3000Project.Views.FavouriteEateries
{
    class FavouriteEateriesViewModel : ViewModelBase, Subscriber
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


        //COMMANDS
        public ICommand Login { get; }

        //CONSTRUCTOR
        public FavouriteEateriesViewModel()
        {
            //set commands
            //Login = new Command(async () => await ExecuteLogin());



            WebsocketHandler.registerSubscriber(this);
        }

        //FUNCTIONS
        public bool UandPAreValid(string username, string password)
        {


            return true;
        }




        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":

                    break;

                default:
                    Console.WriteLine("[MSG] favs page recieved unknown message");
                    break;
            }
        }
    }
}

