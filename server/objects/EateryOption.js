class EateryOption {
    ID;
    Title = '';
    Description = '';
    Rating = 0;
    PhotoReference = '';

    constructor(ID, title, description, rating, photoReference) {
        this.ID;
        this.Title = title;
        this.Description = description;
        this.Rating = rating;
        this.PhotoReference = photoReference;
    }
}

module.exports = EateryOption;
