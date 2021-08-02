using System;
using System.Collections.Generic;
using System.Text;

namespace COMP3000Project.TestObjects
{
    class TransmittableEateryOption
    {
        //this is the TransmittableEateryOption class, it is used as a map for the EateryOption class when it needs to be serialised for transmission
        string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username == value)
                {
                    return;
                }
                _username = value;
            }
        }

        string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
            }
        }

        string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title == value)
                {
                    return;
                }
                _title = value;
            }
        }

        string description;

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description == value)
                {
                    return;
                }
                description = value;
            }
        }
        float rating;

        public float Rating
        {
            get
            {
                return rating;
            }
            set
            {
                if (rating == value)
                {
                    return;
                }
                rating = value;
            }
        }


        string photoReference0;
        public string PhotoReference0
        {
            get
            {
                return photoReference0;
            }
            set
            {
                if (photoReference0 == value)
                {
                    return;
                }
                photoReference0 = value;
            }
        }

        string photoReference1;
        public string PhotoReference1
        {
            get
            {
                return photoReference1;
            }
            set
            {
                if (photoReference1 == value)
                {
                    return;
                }
                photoReference1 = value;
            }
        }

        string photoReference2;
        public string PhotoReference2
        {
            get
            {
                return photoReference2;
            }
            set
            {
                if (photoReference2 == value)
                {
                    return;
                }
                photoReference2 = value;
            }
        }

        string photoReference3;
        public string PhotoReference3
        {
            get
            {
                return photoReference3;
            }
            set
            {
                if (photoReference3 == value)
                {
                    return;
                }
                photoReference3 = value;
            }
        }

        string photoReference4;
        public string PhotoReference4
        {
            get
            {
                return photoReference4;
            }
            set
            {
                if (photoReference4 == value)
                {
                    return;
                }
                photoReference4 = value;
            }
        }

        
        Review[] reviews;
        public Review[] Reviews
        {
            get
            {
                return reviews;
            }
            set
            {
                if (reviews == value)
                {
                    return;
                }
                reviews = value;
            }
        }




        string[] _votes;
        public string[] Votes
        {
            get
            {
                return _votes;
            }
            set
            {
                if (_votes == value)
                {
                    return;
                }
                _votes = value;
            }
        }

        string _openingTime;
        public string OpeningTime
        {
            get
            {
                return _openingTime;
            }
            set
            {
                if (_openingTime == value)
                {
                    return;
                }
                _openingTime = value;
            }
        }

        string _closingTime;
        public string ClosingTime
        {
            get
            {
                return _closingTime;
            }
            set
            {
                if (_closingTime == value)
                {
                    return;
                }
                _closingTime = value;
            }
        }

        string _timeToClosingTime;
        public string TimeToClosingTime
        {
            get
            {
                return _timeToClosingTime;
            }
            set
            {
                if (_timeToClosingTime == value)
                {
                    return;
                }
                _timeToClosingTime = value;
            }
        }

        string notes;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                if (notes == value)
                {
                    return;
                }
                notes = value;
            }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set
            {
                if (address == value)
                {
                    return;
                }
                address = value;
            }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (phoneNumber == value)
                {
                    return;
                }
                phoneNumber = value;
            }
        }

        public TransmittableEateryOption(string username, string ID, string Title, string description, float rating, string photoReference0, string photoReference1, string photoReference2, string photoReference3, string photoReference4, Review[] reviews, string[] votes, string openingTime, string closingTime, string timeToClosingTime, string address, string phoneNumber)
        {
            this.Username = username;
            this.ID = ID;
            this.Title = Title;
            this.Description = description;
            this.Rating = rating;
            this.PhotoReference0 = photoReference0;
            this.PhotoReference1 = photoReference1;
            this.PhotoReference2 = photoReference2;
            this.PhotoReference3 = photoReference3;
            this.PhotoReference4 = photoReference4;
            this.OpeningTime = openingTime;
            this.ClosingTime = closingTime;
            this.TimeToClosingTime = timeToClosingTime;

            this.Reviews = reviews;
            this.Votes = votes;

            this.Address = address;
            this.PhoneNumber = phoneNumber;
        }
    }
}
