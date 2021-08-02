let mongoose = require('mongoose');

let User;
let PastSearch;
let EateryOption;

async function initialiseConnection() {
    //handles communication to and from the database

    let dbUrl = "mongodb+srv://barnaby:admin@cluster0.3qn4a.mongodb.net/RestaurantTinder?retryWrites=true&w=majority";//

    //the user schema, for interacting with user documents
    let userSchema = mongoose.Schema({
        Username:String,
        Password:String,
        current_WSID:String,
    });
    //the past search schema, for interacting with past search documents
    let pastSearchSchema = mongoose.Schema({
        Username:String,
        Location : String,
        Time : String,
        EateryType : String,
        DayOfSearch : String,
        MonthOfSearch : String,
        YearOfSearch : String,
        FullDateOfSearch : String
    });

    //the eateryOption schema, for interacting with favourited eateryOption documents
    let eateryOptionSchema = mongoose.Schema({
        Username : String,
        ID:String,
        Title:String,
        Description:String,
        Rating :Number,
        PhotoReferences:String,
        PhotoReference0:String,
        PhotoReference1:String,
        PhotoReference2:String,
        PhotoReference3:String,
        PhotoReference4:String,
        Reviews: Array,
        Votes: Array,
        OpeningTime:String,
        ClosingTime:String,
        TimeToClosingTime:String,
        Notes:String,
        Address:String,
        PhoneNumber:String
    });

    if(!User){//if User not initiated
        User = mongoose.model('Users', userSchema);//initiate user model
    }

    if(!PastSearch){//if pastSearch not initiated
        PastSearch = mongoose.model('pastSearches', pastSearchSchema)//init past search
    }

    if(!EateryOption){//if eateryOption not initiated
        EateryOption = mongoose.model('favourites', eateryOptionSchema)//init EateryOption
    }

    try{
        await mongoose.connect(dbUrl, {useUnifiedTopology: true, useNewUrlParser: true}).then(function () {// connect to DB
            console.log('[DB] connection initialised successfully');
            return true;
        });

    } catch (e) {
        console.log('[DB] connection failed');//connection failed
        console.log(e);//connection failed
        return false;
    }
}

async function closeConnection() {// close connection to database
    mongoose.connection.close();
}

function getUserModel() { // get user model
    if(User){
        return User;
    }
    return null;
}

function getPastSearchesModel() { // get user model
    if(PastSearch){
        return PastSearch;
    }
    return null;
}

function getEateryOptionModel() { // get user model
    if(EateryOption){
        return EateryOption;
    }
    return null;
}

module.exports = {
    initialiseConnection,
    getUserModel,
    getPastSearchesModel,
    getEateryOptionModel,
    closeConnection
};