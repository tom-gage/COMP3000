class EateryOption {
    ID;
    Title = '';
    Description = '';
    Rating = 0;
    PhotoReference = '';
    Votes = [];

    constructor(ID, title, description, rating, photoReference) {
        this.ID = ID;
        this.Title = title;
        this.Description = description;
        this.Rating = rating;
        this.PhotoReference = photoReference;
        this.Votes = [];
    }

    recordVote(username){
        this.Votes.push(username);
    }
}

module.exports = EateryOption;
