class EateryOption {
    ID;
    Title = '';
    Description = '';
    Rating = 0;
    PhotoReference = '';
    Votes = [];
    OpeningTime;
    ClosingTime;

    constructor(ID, title, description, rating, photoReference, openingTime, closingTime) {
        this.ID = ID;
        this.Title = title;
        this.Description = description;
        this.Rating = rating;
        this.PhotoReference = photoReference;
        this.Votes = [];
        this.OpeningTime = openingTime;
        this.ClosingTime = closingTime;
    }

    recordVote(username){
        this.Votes.push(username);
    }
}

module.exports = EateryOption;
