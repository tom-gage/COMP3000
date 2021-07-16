class EateryOption {
    ID;
    Title = '';
    Description = '';
    Rating = 0;
    PhotoReference = '';
    Votes = [];
    OpeningTime;
    ClosingTime;
    TimeToClosingTime;

    constructor(ID, title, description, rating, photoReference, openingTime, closingTime, timeToClosingTime) {
        this.ID = ID;
        this.Title = title;
        this.Description = description;
        this.Rating = rating;
        this.PhotoReference = photoReference;
        this.Votes = [];
        this.OpeningTime = openingTime;
        this.ClosingTime = closingTime;
        this.TimeToClosingTime = timeToClosingTime;
    }

    recordVote(username){
        this.Votes.push(username);
    }
}

module.exports = EateryOption;
