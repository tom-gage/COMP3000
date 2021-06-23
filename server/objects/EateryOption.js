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

    recordVote(user){
        this.Votes.push(user.Username);
    }
}

module.exports = EateryOption;
