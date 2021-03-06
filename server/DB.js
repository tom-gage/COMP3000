let mongoose = require('mongoose');

let User;
let PastSearch;

async function initialiseConnection() {
    //console.log('[DB] attempting to initialise connection...');

    let dbUrl = "mongodb+srv://barnaby:admin@cluster0.3qn4a.mongodb.net/RestaurantTinder?retryWrites=true&w=majority";//

    let userSchema = mongoose.Schema({
        Username:String,
        Password:String,
        current_WSID:String,
    });

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

    if(!User){//if User not initiated
        User = mongoose.model('Users', userSchema);//initiate user model
    }

    if(!PastSearch){
        PastSearch = mongoose.model('pastSearches', pastSearchSchema)
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

async function closeConnection() {// close connection
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

module.exports = {
    initialiseConnection,
    getUserModel,
    getPastSearchesModel,
    closeConnection
};