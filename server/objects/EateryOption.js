class EateryOption {
    ID;
    Title = '';
    Description = '';
    Rating = 0;
    PhotoReferences = [];
    PhotoReference0 = '';
    PhotoReference1 = '';
    PhotoReference2 = '';
    PhotoReference3 = '';
    PhotoReference4 = '';
    Reviews = [];
    Votes = [];
    OpeningTime;
    ClosingTime;
    TimeToClosingTime;
    Notes;
    Address;
    PhoneNumber;

    constructor(ID, title, description, rating, photoReference0, photoReference1, photoReference2, photoReference3, photoReference4, reviews, openingTime, closingTime, timeToClosingTime, Notes, Address, PhoneNumber) {
        this.ID = ID;
        this.Title = title;
        this.Description = description;
        this.Rating = rating;
        this.PhotoReference0 = photoReference0;
        this.PhotoReference1 = photoReference1;
        this.PhotoReference2 = photoReference2;
        this.PhotoReference3 = photoReference3;
        this.PhotoReference4 = photoReference4;
        this.Reviews = reviews;
        this.Votes = [];
        this.OpeningTime = openingTime;
        this.ClosingTime = closingTime;
        this.TimeToClosingTime = timeToClosingTime;
        this.Notes = Notes;
        this.Address = Address;
        this.PhoneNumber = PhoneNumber;

    }

    recordVote(username){
        this.Votes.push(username);
    }
}

module.exports = EateryOption;
