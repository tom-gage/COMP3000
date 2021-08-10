const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');
const ActiveSearch = require('./objects/ActiveSearch');
const UserObj = require('./objects/User');
const {Client} = require('@googlemaps/google-maps-services-js');
const client = new Client({});
const bcrypt = require("bcrypt");


global.USERS = [];
global.CONNECTED_USERS = [];
global.ACTIVE_SEARCHES = [];

let DB = require('./DB');

let APIKey = "AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw";

class ServerFunctions{

    connectionsMap = new Map();
    UserModel;
    PastSearchesModel;
    EateryOptionModel;

    constructor() {
        // this.connectionsMap = new Map();
    }

    //initiates the connection to the database
    async initConnection(){
        await DB.initialiseConnection();

        //gets the user model
        this.UserModel = await DB.getUserModel();
        //appends all users to USERS array
        await this.UserModel.find({}, function (err, users) {
                global.USERS = users;
        });

        //gets the past searches and eateryOptions models
        this.PastSearchesModel = await DB.getPastSearchesModel();
        this.EateryOptionModel = await DB.getEateryOptionModel();


    }

    //closes the connection to the database
    async closeConnection(){
        await DB.closeConnection();
    }

    //deletes an eatery from favorites collection, then informs the client of the action
    deleteFavouriteEatery(username, eateryTitle){

        let thisInstance = this;

        this.EateryOptionModel.findOneAndDelete({Username : username, Title : eateryTitle}, function (err) {
            if (err){
                return console.log(err);
            } else {
                let MSG = new Message(1, "favouriteEateryDeleted", "", []);
                thisInstance.sendToUser(username, MSG);
            }

        });
    }

    //updates an eatery note, then informs the client of the action
    updateFavouriteEateryNote(username, eateryTitle, note){

        let thisInstance = this;

        this.EateryOptionModel.findOneAndUpdate({Username : username, Title : eateryTitle}, {Notes : note}, {upsert : true, overwrite : false}, function (err) {
            if (err){
                return console.log(err);
            } else {
                let MSG = new Message(1, "noteUpdated", "", []);
                thisInstance.sendToUser(username, MSG);
            }

        });
    }

    //gets the users favourite eateries from the favourites collection, then sends a list of them to the client
    getFavourites(username){
        let thisInstance = this;

        this.EateryOptionModel.find({Username : username})
            .then((obj) => {
                let favourites = this.createFavouritesArray(obj)

                let MSG = new Message(1, "gotFavourites", "", [favourites]);
                this.sendToUser(username, MSG);
            });
    }

    //creates an array of favourite eateries
    createFavouritesArray(eateries){
        let favourites = [];

        if(eateries.length < 1){
            return [];
        }

        for(let i = 0; i < eateries.length; i++){
            let thisEatery = eateries[i];



            let eatery = new EateryOption(
                thisEatery.ID,
                thisEatery.Title,
                thisEatery.Description,
                thisEatery.Rating,
                thisEatery.PhotoReference0,
                thisEatery.PhotoReference1,
                thisEatery.PhotoReference2,
                thisEatery.PhotoReference3,
                thisEatery.PhotoReference4,
                thisEatery.Reviews,
                thisEatery.OpeningTime,
                thisEatery.ClosingTime,
                "0",
                thisEatery.Notes,
                thisEatery.Address,
                thisEatery.PhoneNumber

            );

            favourites.push(eatery);
        }

        console.log(favourites);
        return favourites;
    }

    //adds an eatery to the favourites collection
    addEateryToFavourites(username, eatery){
        let newEateryOption = JSON.parse(eatery);
        // newEateryOption.username = username;

        console.log(newEateryOption);

        let thisInstance = this;

        this.EateryOptionModel.findOneAndUpdate({Title : newEateryOption.Title}, newEateryOption, {upsert : true, overwrite : true}, function (err) {
            if (err){
                return console.log(err);
            } else {
                let MSG = new Message(1, "eateryAddedToFavourites", "", []);
                thisInstance.sendToUser(username, MSG);
            }

        });
    }

    //process's a user's request to vote for a specific eatery
    castVoteInSearch(searchCode, username, eateryOptionID){
        //get the active search the user is in
        let search = this.getActiveSearch(searchCode);


        if(search){//if search exists
            search.castVote(username, eateryOptionID);//cast the vote


            let match = search.checkForMatch();//check for a match

            if(match){//if match exists
                console.log("[MATCH] got match!")

                let participants = [];

                for(let i = 0; i < search.Participants.length; i++){//make an array of participants usernames
                    participants.push(search.Participants[i].Username);
                }

                for(let i = 0; i < search.Participants.length; i++){//for each participant

                    let MSG = new Message(1, "matched!", "", [match, participants]);
                    this.sendToUser(search.Participants[i].Username, MSG);//send matched message!
                }

            } else {//if no match
                let MSG = new Message(1, "IVoted", "", []);
                this.sendToUser(username, MSG);//send "I voted" feedback to voter

                this.sendToAllExcept(username, search, "participantVoted",username, []);//send "Someone else voted" feedback to everyone else
            }
            // search.showVotes();
        }
    }

    //process request to join an existing search
    joinExistingSearch(searchCode, username){
        console.log("[SEARCH] user joining search");
        //get existing search in ACTIVE_SEARCHES by search code
        let search = this.getActiveSearch(searchCode);

        if(search){//if search exists
            //get user by username
            //add user to the search

            let user = this.getUser(username);

            if(user){
                search.addParticipant(user);

                //send feedback to user
                let MSG = new Message(1, "joinSearchRequestGranted", searchCode, [search.ID, search.EateryOptions, search.Participants]);
                this.sendToUser(username, MSG);

                //send "someone joined" to other users
                this.sendToAllExcept(username, search, "participantJoined", username, []);
            }

        } else {
            //else, reject request
            let MSG = new Message(1, "joinSearchRequestRejected", searchCode, []);
            this.sendToUser(username, MSG);
        }


    }


    //gets the past searches for a user
    getPastSearches(username){
        //go to database, get three most recent searches, send them back to the user

        this.PastSearchesModel.find({Username : username}).sort({MonthOfSearch: -1, YearOfSearch: -1}).limit(5)
            .then((obj) => {

            let MSG = new Message(1, "gotPastSearches", "", [obj]);
            this.sendToUser(username, MSG);
        });
    }

    //create a new past search
    createNewPastSearch(username, location, time, eateryType){
        let date = new Date();
        let newPastSearch = {
            Username : username,
            Location : location,
            Time : time,
            EateryType : eateryType,
            DayOfSearch : date.getDate(),
            MonthOfSearch : date.getMonth() + 1,
            YearOfSearch : date.getFullYear()
        };

        this.PastSearchesModel.create(newPastSearch, function (err) {
            if (err) return console.log(err);
        });


        console.log('[PAST_SEARCHES] new past search created');
        console.log(newPastSearch);

        return null;
    }

    //create a new active search
    async createNewActiveSearch(username, locationName, time, eateryTypes){
        let selectedEateryType = eateryTypes[0];

        let latitude;
        let longitude;

        let eateryOptionsArray = [];

        //create an active search object, populated with a user and eatery options array
        console.log("[SEARCH] creating new search");

        console.log("making API calls with following data: ");

        console.log(locationName);
        console.log(time);
        console.log(eateryTypes);

        this.createNewPastSearch(username, locationName, time, selectedEateryType);


        //make the API calls to get the relevant data on nearby eateries
        client.geocode({params : {//first, do geocode request which converts a location string into coordinates
                address : locationName,
                key : APIKey,
                region : "uk"
            },
            timeout:1000


        }).then((geoCodeResponse) => {
            latitude = geoCodeResponse.data.results[0].geometry.location.lat;
            longitude = geoCodeResponse.data.results[0].geometry.location.lng;

            client.placesNearby({params:{//then, do a place search request for places nearby the coordinates to get list of nearby eateries
                    location : [latitude, longitude],
                    radius : 2000,
                    type : selectedEateryType,
                    key : APIKey
                },
                timeout:1000


            }).then((PlaceSearchResponse) => {

                console.log("PLACES SEARCH RESPONSE IS: ");

                let counter = 0;

                for(let i = 0; i < PlaceSearchResponse.data.results.length; i++){

                    client.placeDetails({ params : {//then, for each result, do a place details search
                            place_id : PlaceSearchResponse.data.results[i].place_id,
                            key : APIKey
                        },
                    timeout : 1000
                    }).then((placeDetailsResponse) => {
                        console.log("PLACE DETAILS SEARCH RESPONSE IS: " + counter);
                        console.log("- response here -");

                        let currentDay = new Date().getDay();

                        //
                        //the following lines append Place DETAILS response data to Place SEARCH response data
                        //

                        try{//do for opening and closing times
                            PlaceSearchResponse.data.results[i].openingTimeForToday = placeDetailsResponse.data.result.opening_hours.periods[currentDay].open.time;
                            PlaceSearchResponse.data.results[i].closingTimeForToday = placeDetailsResponse.data.result.opening_hours.periods[currentDay].close.time;
                        }catch(err){
                            PlaceSearchResponse.data.results[i].openingTimeForToday = null;
                            PlaceSearchResponse.data.results[i].closingTimeForToday = null;
                        }

                        try{//do for photos
                            PlaceSearchResponse.data.results[i].detailsPhotosArray = placeDetailsResponse.data.result.photos;
                        }catch(err){

                        }


                        if(placeDetailsResponse.data.result.reviews){//do for reviews
                            PlaceSearchResponse.data.results[i].reviews = placeDetailsResponse.data.result.reviews;
                        }

                        //do for address and phone number
                        PlaceSearchResponse.data.results[i].formatted_address = placeDetailsResponse.data.result.formatted_address;
                        PlaceSearchResponse.data.results[i].formatted_phone_number = placeDetailsResponse.data.result.formatted_phone_number;

                        placeDetailsResponse.Latitude = latitude;
                        placeDetailsResponse.Longitude = longitude;

                        //responses are asynchronous and come in at varying times, this counter tracks which have come in
                        counter++;



                        if(counter >= PlaceSearchResponse.data.results.length){//when all the responses are accounted for the data can be compiled and sent to the user
                            this.compileAndSendToUser(username, PlaceSearchResponse, time);
                        }

                    }).catch((err) => {


                        console.log("PLACE DETAILS SEARCH RESPONSE IS: " + counter);
                        console.log("- error! -");

                        //opening and closing times can be a bit iffy, this code captures them if they're not in an expected format
                        counter++;
                        PlaceSearchResponse.data.results[i].openingTimeForToday = null;
                        PlaceSearchResponse.data.results[i].closingTimeForToday = null;

                        if(counter >= PlaceSearchResponse.data.results.length){
                            this.compileAndSendToUser(username, PlaceSearchResponse, time);
                        }
                    })

                }



            }).catch((error) => {
                console.log('[ERROR]');
                console.log(error);

            });


        }).catch(function (err){
            console.log('GEOCODE REQUEST FAILED');
            console.log(err);
        });


    }


    //compiles the API responses into a tidy bundle that is converted to JSON and sent to the user
    compileAndSendToUser(username, eateryData, time){
        //create an arrau of eatery options
        let eateryOptionsArray = this.createEateryOptionsArray(eateryData, time);

        //create a new active search
        let newActiveSearch = new ActiveSearch(this.getUser(username), eateryOptionsArray);

        //add it to the roster of active searches
        ACTIVE_SEARCHES.push(newActiveSearch);

        console.log("Search code is: " + newActiveSearch.ID);

        //then pass a success message back to the user, containing the ActiveSearch object
        let MSG = new Message(1, "newActiveSearchRequestGranted", "", [newActiveSearch.ID, newActiveSearch.EateryOptions]);
        this.sendToUser(username, MSG);
    }

    createEateryOptionsArray(APISearchResults, desiredArrivalTime){
        let EateriesArray = [];

        try{
            if(APISearchResults.data.results.length === 0){
                return [];
            }

            for (let i = 0; i < APISearchResults.data.results.length; i++) {//for each search result
                let TheEateryOption = APISearchResults.data.results[i];

                let ID;
                let name = TheEateryOption.name;
                let description = "description would be here, if there was one";
                let rating = TheEateryOption.rating;
                let photoRef0 = "";
                let photoRef1 = "";
                let photoRef2 = "";
                let photoRef3 = "";
                let photoRef4 = "";
                let Reviews = [];
                // let PhotoReferences = [];
                let OpeningTime = TheEateryOption.openingTimeForToday;
                let ClosingTime = TheEateryOption.closingTimeForToday;
                let TimeToClosingTime = 0;
                let Notes = "";
                let Address = "";
                let PhoneNumber = "";

                if(ClosingTime !=  null){
                    TimeToClosingTime = Number(ClosingTime.toString().slice(0, 2) - Number(desiredArrivalTime.toString().slice(0, 2)));
                }

                if(TheEateryOption.photos){//if the eatery option has photos
                    ID = TheEateryOption.photos[0].photo_reference;//set ID to be the first photo reference, bit hacky but I bet a very clever google employee has made sure its not going to collide with anything this side of the big crunch

                    //if a photos object exists, go into it and add its reference
                    if(TheEateryOption.photos[0]){
                        photoRef0 = TheEateryOption.photos[0].photo_reference
                    }
                    if(TheEateryOption.detailsPhotosArray[1]){
                        photoRef1 = TheEateryOption.detailsPhotosArray[1].photo_reference
                    }
                    if(TheEateryOption.detailsPhotosArray[2]){
                        photoRef2 = TheEateryOption.detailsPhotosArray[2].photo_reference
                    }
                    if(TheEateryOption.detailsPhotosArray[3]){
                        photoRef3 = TheEateryOption.detailsPhotosArray[3].photo_reference
                    }
                    if(TheEateryOption.detailsPhotosArray[4]){
                        photoRef4 = TheEateryOption.detailsPhotosArray[4].photo_reference
                    }

                    if(TheEateryOption.reviews){//if the eatery option has reviews
                        for(let y = 0; y < TheEateryOption.reviews.length; y++){//for each review, append a review object to Reviews[]
                            Reviews.push({
                                AuthorName : TheEateryOption.reviews[y].author_name.toString(),
                                Rating : TheEateryOption.reviews[y].rating.toString(),
                                RelativeTimeDescription : TheEateryOption.reviews[y].relative_time_description.toString(),
                                Text : TheEateryOption.reviews[y].text.toString(),
                                TimeSinceReview : TheEateryOption.reviews[y].time
                            })
                        }
                    }


                    //grab the address and phone number
                    Address = TheEateryOption.formatted_address;
                    PhoneNumber = TheEateryOption.formatted_phone_number;


                    //use the previously set variables to construct an eateryOption object
                    let eatery = new EateryOption(
                        ID,
                        name,
                        description,
                        rating,
                        photoRef0,
                        photoRef1,
                        photoRef2,
                        photoRef3,
                        photoRef4,
                        Reviews,
                        OpeningTime,
                        ClosingTime,
                        TimeToClosingTime.toString(),
                        Notes,
                        Address,
                        PhoneNumber

                    );

                    //push to the eateries array
                    if(ClosingTime === null){
                        EateriesArray.push(eatery);
                    } else if( ClosingTime > desiredArrivalTime){
                        EateriesArray.push(eatery);
                    }
                }
            }
        } catch(err) {
            console.log('[ERROR] could not parse places data');
            console.log(err);

            return [];
        }

        //throw it back
        return EateriesArray;
    }


    //returns true if username and password matches a user, else returns false
    async validateCredentials(username, plainTextPassword){
        if(username === "" || plainTextPassword === ""){
            return false;
        }

        let that = this;

        let credentialsValidity = await new Promise((resolve, reject) => {
            USERS.find(function (user) {//get user from users array
                if(user.Username === username){
                    resolve(that.comparePassword(plainTextPassword, user.Password));//compare input password with password on file
                }

            });
             console.log("returning... " + false);
             resolve(false);
        })

        return credentialsValidity;

    }

    //grant a login request
    grantLoginRequest(ws, username){
        //assigns the user a new Websocket connection on the connections map
        this.connectionsMap.set(this.getUser(username), ws);

        let MSG = new Message(1, "loginRequestGranted", "", []);
        try{
            ws.send(JSON.stringify(MSG));//inform the user that their login request is granted
            console.log('[LOGIN] user login succeeded');
        } catch {

        }
    }

    //reject a login request
    rejectLoginRequest(ws){
        //inform user that their login request is rejected
        let MSG = new Message(1, "loginRequestRejected", "", []);

        try{
            ws.send(JSON.stringify(MSG));
            console.log('[LOGIN] user login rejected');
        } catch {

        }
    }

    //update a user's username
    async updateUsername(username, password, newUsername){
        console.log("[LOGIN] updating username...");

        let that = this;

        this.UserModel.updateOne(//where DB.Username == username DO DB.Username = newUsername
            {
                Username : username
            },
            {
                Username : newUsername
            })
            .then((obj) => {


                let thisUser = {
                    Username : username
                }

                this.PastSearchesModel.updateMany(thisUser, { Username : newUsername}, function (){//update past searches to reflect username change
                    that.EateryOptionModel.updateMany(thisUser, { Username : newUsername}, function (){//update favourites to reflect username change
                        that.UserModel.find({}, function (err, users) {
                            let ws = that.connectionsMap.get(that.getUser(username));
                            that.connectionsMap.delete(that.getUser(username));//wipe connections map entry for user
                            global.USERS = users;
                            that.connectionsMap.set(that.getUser(newUsername), ws);//replace entry, username change reflected
                        });
                    })
                })

                console.log("[LOGIN] username update succeeded!");
                let MSG = new Message(1, "usernameUpdated", newUsername, []);
                this.sendToUser(username, MSG);
            })
    }

    //reject username change request
    async rejectUpdateUsernameRequest(ws){
        //inform user that their login request is granted
        let MSG = new Message(1, "usernameChangeRequestRejected", "", []);

        try{
            ws.send(JSON.stringify(MSG));
            console.log('[LOGIN] username change rejected');
        } catch {

        }
    }

    //process update password request
    async updatePassword(username, password, newPassword){
        console.log("[LOGIN] updating password...");

        let that = this;

        await this.generateSaltedAndHashedPassword(newPassword).then(//generate hashed password from the new password
            async function (hashedPass){
                that.UserModel.updateOne(//then update user document to reflect the change
                    {
                        Username : username
                    },
                    {
                        Password : hashedPass
                    })
                    .then((obj) => {//when done
                        let thisClass = this;

                        that.UserModel.find({}, function (err, users) {
                            let ws = that.connectionsMap.get(that.getUser(username));
                            that.connectionsMap.delete(that.getUser(username));

                            global.USERS = users;//update the USERS array
                            that.connectionsMap.set(that.getUser(username), ws);//update the connections map
                        });

                        console.log("[LOGIN] password update succeeded!");
                        let MSG = new Message(1, "passwordUpdated", newPassword, []);//finally, send response to the user
                        that.sendToUser(username, MSG);
                    })
            }
        )
    }

    //reject update password request
    async rejectUpdatePasswordRequest(ws){
        //inform user that their login request is granted
        let MSG = new Message(1, "passwordUpdateRequestRejected", "", []);

        try{
            ws.send(JSON.stringify(MSG));
            console.log('[LOGIN] password change rejected');
        } catch {

        }
    }

    //delete a user account
    async deleteUser(username){
        console.log("[LOGIN] deleting user...");

        let that = this;

        this.UserModel.deleteOne({//delete from database where DB.Username == username
            Username : username
        }).then((obj) => {


            let thisClass = this;

            this.UserModel.find({}, function (err, users) {
                thisClass.connectionsMap.delete(thisClass.getUser(username));
                global.USERS = users;
            });

            console.log("[LOGIN] user delete succeeded!");
            let MSG = new Message(1, "userDeleted", "", []);
            this.sendToUser(username, MSG);//finally, respond to user, inform them of deletion success
        });

    }

    //returns true if a username is available
    usernameNotTaken(username){
        if(username){
            if(USERS.find(function (user) {//if credentials match existing user, username is taken
                return (user.Username === username);
            })) {


                console.log('[LOGIN] Validation failure, username is taken!!');
                return false;
            }
            else {

                return true;
            }
        }
    }

    //rejects a registration request
    async rejectRegistration(ws){
        let MSG = new Message(1, "registrationRequestRejected", "", []);

        try{
            ws.send(JSON.stringify(MSG));
            console.log('[LOGIN] registration rejected');
        } catch {

        }
    }


    async registerNewUser(username, password, ws){
        let that = this;
        await this.generateSaltedAndHashedPassword(password).then(//generate a hashed password
            async function (hashedPass){
                let newUser = {
                    Username : username,
                    Password : hashedPass
                };

                //
                await that.UserModel.create(newUser, async function (err) {//create a new user entry
                    if(err){
                        console.log(err);
                    }

                    await that.UserModel.find({}, function (err, users) {//get all the users
                        if(err){
                            console.log(err);
                        }
                        global.USERS = users;//update USERS Array

                        // console.log(newUser);
                        // console.log(global.USERS);

                        //update connections map
                        that.connectionsMap.set(that.getUser(username), ws);//update the connections map

                        console.log('[LOGIN] user registration succeeded');

                        let MSG = new Message(1, "registrationSuccess", "", [username.toString(), password.toString()]);//respond to user, inform them of account creation
                        that.sendToUser(username, MSG);
                    });
                });
            }
        )



    }

    //returns a salted and hashed password for security reasons
    async generateSaltedAndHashedPassword(plainTextPassword){
        console.log("bcrypt begin password hashing...")
        let saltRounds = 12;

        let hashedPassword = await new Promise((resolve, reject) => {
            bcrypt.genSalt(saltRounds, (err, salt) => {
                bcrypt.hash(plainTextPassword, salt, (err, saltedHashedPassword) => {
                    console.log("s&h pass is: " + saltedHashedPassword);
                    if (err) reject(err)
                    resolve(saltedHashedPassword)
                });
            });
        })

        return hashedPassword

    }

    //compares a plain text password to a hashed password
    async comparePassword(plainTextPassword, saltedHashedPassword){

        let match = await new Promise((resolve, reject) => {
            bcrypt.compare(plainTextPassword, saltedHashedPassword, function(err, result) {
                    console.log("comparing passwords...")
                    if(result){
                        console.log("passwords match!")
                        resolve(true);
                    } else {
                        console.log("passwords dont match")
                        resolve(false);
                    }

                }
            );
        })

        return match;

    }

//helper functions

    //gets a user object from a username
    getUser(username){
        let targetUser = null;
        targetUser = USERS.find(function (User) {
            return User.Username.toString() === username.toString();
        });

        return targetUser;
    }

    //returns an active search object from an active search ID
    getActiveSearch(ID) {
        let targetSearch = null;
        targetSearch = ACTIVE_SEARCHES.find(function (ActiveSearch, index) {
            return ActiveSearch.ID.toString() === ID.toString();
        });
        return targetSearch;
    }

    //sends a test message
    sendTestMessage(ws){
        ws.send(JSON.stringify(new Message("1", "debugMessage", "hello there")));
    }

    clearACTIVE_SEARCHES(){
        global.ACTIVE_SEARCHES = [];
    }

    clearUSERS(){
        global.USERS = [];
    }

    //sends a message to all users in a search except one
    sendToAllExcept(username, search, type, body, items){
        for(let i = 0; i < search.Participants.length; i++){
            if(search.Participants[i].Username.toString() !== username.toString()){
                console.log("sending participant voted message to "+ search.Participants[i].Username);

                let MSG = new Message(1, type, body, items);
                this.sendToUser(search.Participants[i].Username, MSG);
            }

        }
    }

    //sends a message to a user
    sendToUser(username, message){
        let ws = this.connectionsMap.get(this.getUser(username));

        if(ws){
            ws.send(JSON.stringify(message));
            console.log("[MSG] sent the following to user: " + username);
            console.log("Message type: " + message.type);
            console.log("Message body: " + message.Body);
            console.log("Message.items contains: " + message.Items.length + " object(s)");
        } else {
            console.log("WS INVALID, MESSAGE SEND FAILED");
        }
    }
}

module.exports = ServerFunctions