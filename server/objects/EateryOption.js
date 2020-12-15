class EateryOption{
    Title = '';
    Description = '';
    Rating = 0;
    PhotoReference = '';

    constructor(title, description, rating, photoReference) {
        this.Title = title;
        this.Description = description;
        this.Rating = rating;
        this.PhotoReference = photoReference;
    }
}

module.exports = EateryOption;
