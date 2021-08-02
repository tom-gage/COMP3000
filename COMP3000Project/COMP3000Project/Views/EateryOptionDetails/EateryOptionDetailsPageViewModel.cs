using COMP3000Project.Interfaces;
using COMP3000Project.TestObjects;
using COMP3000Project.ViewModel;
using COMP3000Project.Views.Settings;
using COMP3000Project.WS;
using COMP3000Project.Views.Search;
using COMP3000Project.Views.SearchParameters;
using COMP3000Project.UserDetailsSingleton;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace COMP3000Project.Views.EateryOptionDetails
{
    class EateryOptionDetailsPageViewModel : ViewModelBase, Subscriber
    {
        //VARIABLES
        private string eateryTitle;
        public string EateryTitle
        {
            get { return eateryTitle; }
            set
            {
                if (eateryTitle != value)
                {
                    SetProperty(ref eateryTitle, value);//informs view of change
                }
            }
        }

        private string eateryRating;
        public string EateryRating
        {
            get { return eateryRating; }
            set
            {
                if (eateryRating != value)
                {
                    SetProperty(ref eateryRating, value);//informs view of change
                }
            }
        }

        private string eateryAddress;
        public string EateryAddress
        {
            get { return eateryAddress; }
            set
            {
                if (eateryAddress != value)
                {
                    SetProperty(ref eateryAddress, value);//informs view of change
                }
            }
        }

        private string eateryPhoneNumber;
        public string EateryPhoneNumber
        {
            get { return eateryPhoneNumber; }
            set
            {
                if (eateryPhoneNumber != value)
                {
                    SetProperty(ref eateryPhoneNumber, value);//informs view of change
                }
            }
        }

        private string reviewsHeader;
        public string ReviewsHeader
        {
            get { return reviewsHeader; }
            set
            {
                if (reviewsHeader != value)
                {
                    SetProperty(ref reviewsHeader, value);//informs view of change
                }
            }
        }

        
        ObservableCollection<ImageHolder> images;
        public ObservableCollection<ImageHolder> Images { get => images; set => SetProperty(ref images, value); }

        ObservableCollection<Review> reviews;
        public ObservableCollection<Review> Reviews { get => reviews; set => SetProperty(ref reviews, value); }

        //COMMANDS
        public ICommand SortByOldest { get; }
        public ICommand SortByYoungest { get; }
        public ICommand SortByBest { get; }
        public ICommand SortByWorst { get; }
        public ICommand AddToFavourites { get; }
        public ICommand CastVote { get; }


        //CONSTRUCTOR
        public EateryOptionDetailsPageViewModel(EateryOption eateryOption)
        {

            //set commands
            SortByOldest = new Command(async () => await ExecuteSortByOldest());
            SortByYoungest = new Command(async () => await ExecuteSortByYoungest());
            SortByBest = new Command(async () => await ExecuteSortByBest());
            SortByWorst = new Command(async () => await ExecuteSortByWorst());
            AddToFavourites = new Command(async () => await ExecuteAddToFavourites(eateryOption));
            CastVote = new Command(async () => await ExecuteCastVote(eateryOption));

            //set the review and image arrays
            Reviews = sortByYoungest(eateryOption.Reviews);
            Images = new ObservableCollection<ImageHolder>();

            if(Reviews.Count > 0)
            {
                ReviewsHeader = "Customer Reviews:";
            } else
            {
                ReviewsHeader = "No reviews found!";
            }

            //set the eatery information
            EateryTitle = eateryOption.Title;
            EateryRating = eateryOption.FormattedRating;

            EateryAddress = eateryOption.Address;
            EateryPhoneNumber = eateryOption.PhoneNumber;

            //set text size
            VeryLarge = UserDetails.GetVeryLargeTextSetting();
            Large = UserDetails.GetlargeTextSetting();
            Medium = UserDetails.GetMediumTextSetting();
            Small = UserDetails.GetSmallTextSetting();

            populateImagesArray(eateryOption);

            WebsocketHandler.registerSubscriber(this);


        }

        //FUNCTIONS

        //cast vote for the option
        async Task<object> ExecuteCastVote(EateryOption eateryOption)
        {
            WebsocketHandler.RequestCastVote(UserDetails.Username, UserDetails.Password, UserDetails.SearchID, eateryOption.ID);
            return null;
        }

        //add option to favourites array
        async Task<object> ExecuteAddToFavourites(EateryOption eateryOption)
        {
            WebsocketHandler.RequestAddToFavourites(UserDetails.Username, UserDetails.Password, eateryOption);
            return null;
        }

        //sort reviews by age, oldest first
        async Task<object> ExecuteSortByOldest()
        {
            Reviews = sortByOldest(Reviews);
            return null;
        }

        //sort reviews by age, youngest first
        async Task<object> ExecuteSortByYoungest()
        {
            Reviews = sortByYoungest(Reviews);
            return null;
        }

        //sort reviews by rating, positive to negative
        async Task<object> ExecuteSortByBest()
        {
            Reviews = sortByBest(Reviews);
            return null;
        }

        //sort reviews by rating, negative to positive
        async Task<object> ExecuteSortByWorst()
        {
            Reviews = sortByWorst(Reviews);
            return null;
        }

        ObservableCollection<Review> sortByWorst(ObservableCollection<Review> reviews)
        {
            Review[] r = new List<Review>(reviews).ToArray();
            reviews.Clear();

            Review t;

            //for each review
            for (int p = 0; p <= r.Length - 2; p++)
            {
                for (int i = 0; i <= r.Length - 2; i++)
                {
                    if (int.Parse(r[i].Rating) > int.Parse(r[i + 1].Rating))//if selected review's rating is greater than its following neighbour's rating, swap the reviews
                    {
                        t = r[i + 1];
                        r[i + 1] = r[i];
                        r[i] = t;
                    }
                }
            }
            Console.WriteLine("\n" + "Sorted array :");
            foreach (Review rr in r)
            {
                Console.Write("\n");
                Console.Write(rr.TimeSinceReview + " ");

                reviews.Add(rr);
            }


            return reviews;
        }

        ObservableCollection<Review> sortByBest(ObservableCollection<Review> reviews)
        {
            Review[] r = new List<Review>(reviews).ToArray();
            reviews.Clear();

            Review t;

            //for each review
            for (int p = 0; p <= r.Length - 2; p++)
            {
                for (int i = 0; i <= r.Length - 2; i++)
                {
                    if (int.Parse(r[i].Rating) < int.Parse(r[i + 1].Rating))//if selected review's rating is less than its following neighbour's rating, swap the reviews
                    {
                        t = r[i + 1];
                        r[i + 1] = r[i];
                        r[i] = t;
                    }
                }
            }
            Console.WriteLine("\n" + "Sorted array :");
            foreach (Review rr in r)
            {
                Console.Write("\n");
                Console.Write(rr.TimeSinceReview + " ");

                reviews.Add(rr);
            }


            return reviews;
        }

        ObservableCollection<Review> sortByYoungest(ObservableCollection<Review> reviews)
        {
            Review[] r = new List<Review>(reviews).ToArray();
            reviews.Clear();

            Review t;

            //for each review
            for (int p = 0; p <= r.Length - 2; p++)
            {
                for (int i = 0; i <= r.Length - 2; i++)
                {
                    if (r[i].TimeSinceReview < r[i + 1].TimeSinceReview)//if selected review's age is less than its following neighbour's age, swap the reviews
                    {
                        t = r[i + 1];
                        r[i + 1] = r[i];
                        r[i] = t;
                    }
                }
            }
            Console.WriteLine("\n" + "Sorted array :");
            foreach (Review rr in r)
            {
                Console.Write("\n");
                Console.Write(rr.TimeSinceReview + " ");
                
                reviews.Add(rr);
            }
               

            return reviews;
        }



        ObservableCollection<Review> sortByOldest(ObservableCollection<Review> reviews)
        {
            Review[] r = new List<Review>(reviews).ToArray();
            reviews.Clear();

            Review t;
            

            //for each review
            for (int p = 0; p <= r.Length - 2; p++)
            {
                for (int i = 0; i <= r.Length - 2; i++)
                {
                    if (r[i].TimeSinceReview > r[i + 1].TimeSinceReview)//if selected review's age is greater than its following neighbour's age, swap the reviews
                    {
                        t = r[i + 1];
                        r[i + 1] = r[i];
                        r[i] = t;
                    }
                }
            }
            Console.WriteLine("\n" + "Sorted array :");
            foreach (Review rr in r)
            {
                Console.Write("\n");
                Console.Write(rr.TimeSinceReview + " ");

                reviews.Add(rr);
            }


            return reviews;
        }

        //populate images array
        async void populateImagesArray(EateryOption eateryOption)
        {
            //if image string is valid, add to array
            if (eateryOption.EateryImage0 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage0));
            }

            if (eateryOption.EateryImage1 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage1));
            }

            if (eateryOption.EateryImage2 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage2));
            }

            if (eateryOption.EateryImage3 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage3));
            }

            if (eateryOption.EateryImage4 != null)
            {
                Images.Add(new ImageHolder(eateryOption.EateryImage4));
            }
        }


        
        //catch incoming messages from the publisher
        public void Update(Message message)
        {
            switch (message.type)
            {
                case "":
                    Console.WriteLine("...");

                    break;

                case "participantJoined":
                    Console.WriteLine("[MSG] a participant joined");
                    //show feedback
                    ShowToast(message.Body + " joined the search!");
                    break;

                case "IVoted":
                    Console.WriteLine("[MSG] I voted! ");
                    //show feedback
                    ShowToast("Vote cast!");
                    break;


                case "participantVoted":
                    Console.WriteLine("[MSG] a participant voted");
                    //show feedback
                    ShowToast(message.Body + " voted!");
                    break;

                case "eateryAddedToFavourites":
                    Console.WriteLine("got eatery added to favs confirmation!");
                    //show feedback
                    ShowToast("Added to Favourites!");

                    break;

                default:
                    Console.WriteLine("[MSG] eatery option details page recieved unknown message");
                    break;
            }
        }
    }
}
