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
// let UserModel;

// let connectionsMap = new Map();

let APIKey = "AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw";

class ServerFunctions{

    connectionsMap = new Map();
    UserModel;
    PastSearchesModel;
    EateryOptionModel;

    constructor() {
        // this.connectionsMap = new Map();
    }

    async initConnection(){
        await DB.initialiseConnection();


        this.UserModel = await DB.getUserModel();
        await this.UserModel.find({}, function (err, users) {
                global.USERS = users;
        });
        this.PastSearchesModel = await DB.getPastSearchesModel();
        this.EateryOptionModel = await DB.getEateryOptionModel();


    }

    async closeConnection(){
        await DB.closeConnection();
    }

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

    getFavourites(username){
        let thisInstance = this;

        this.EateryOptionModel.find({Username : username})
            .then((obj) => {
                let favourites = this.createFavouritesArray(obj)

                let MSG = new Message(1, "gotFavourites", "", [favourites]);
                this.sendToUser(username, MSG);
            });
    }

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
                thisEatery.Notes

            );

            favourites.push(eatery);
        }

        console.log(favourites);
        return favourites;
    }

    addEateryToFavourites(username, eatery){
        let newEateryOption = JSON.parse(eatery);
        // newEateryOption.username = username;

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


    castVoteInSearch(searchCode, username, eateryOptionID){
        //get active search by searchcode
        let search = this.getActiveSearch(searchCode);

        //cast vote
        if(search){
            search.castVote(username, eateryOptionID);

            //if users have matched, send "you matched!" feedback
            let match = search.checkForMatch();

            if(match){

                let MSG = new Message(1, "matched!", "", [match]);
                this.sendToUser(username, MSG);
            }

            search.showVotes();


            //TEST STUFF
            // let x = search.EateryOptions[0];
            //
            // let MSG = new Message(1, "gotMatch", "", [x]);
            // this.sendToUser(username, MSG);
        }



    }

    joinExistingSearch(searchCode, username){
        console.log("[SEARCH] user joining search");
        //get existing search in ACTIVE_SEARCHES by search code
        let search = this.getActiveSearch(searchCode);

        if(search){
            //if search is valid
            //get user by username
            //add user to the search
            search.addParticipant(this.getUser(username));

            //send feedback to app
            let MSG = new Message(1, "joinSearchRequestGranted", searchCode, [search.ID, search.EateryOptions, search.Participants]);
            this.sendToUser(username, MSG);
        } else {
            //else, reject request
            let MSG = new Message(1, "joinSearchRequestRejected", searchCode, []);
            this.sendToUser(username, MSG);
        }


    }

    getPastSearches(username){
        //go to database, get three most recent searches, send them back to the user

        this.PastSearchesModel.find({Username : username}).sort({MonthOfSearch: -1, YearOfSearch: -1}).limit(5)
            .then((obj) => {

            let MSG = new Message(1, "gotPastSearches", "", [obj]);
            this.sendToUser(username, MSG);
        });
    }

    createNewPastSearch(username, location, time, eateryType){
        let date = new Date();
        let newPastSearch = {
            Username : username,
            Location : location,
            Time : time,
            EateryType : eateryType,
            DayOfSearch : date.getDate(),
            MonthOfSearch : date.getMonth(),
            YearOfSearch : date.getFullYear()
        };

        this.PastSearchesModel.create(newPastSearch, function (err) {
            if (err) return console.log(err);
        });


        console.log('[PAST_SEARCHES] new past search created');

        return null;
    }

    async createNewActiveSearch(username, locationName, time, eateryTypes){
        let selectedEateryType = eateryTypes[0];

        let eateryOptionsArray = [];

        //create an active search object, populated with a user and eatery options array
        console.log("[SEARCH] creating new search");

        console.log("making API calls with following data: ");

        console.log(locationName);
        console.log(time);
        console.log(eateryTypes);

        this.createNewPastSearch(username, locationName, time, selectedEateryType);



        client.geocode({params : {
                address : locationName,
                key : APIKey
            },
            timeout:1000


        }).then((geoCodeResponse) => {//first, do geocode request for a location string to get coods
            console.log('GEOCODE RESPONSE IS: ');
            console.log(geoCodeResponse.data.results);

            let latitude = geoCodeResponse.data.results[0].geometry.location.lat;
            let longitude = geoCodeResponse.data.results[0].geometry.location.lng;

            client.placesNearby({params:{//then, do place search request for places nearby the coods to get list of nearby eateries
                    location : [latitude, longitude],
                    // location : [50.381773,-4.133786],
                    radius : 2000,
                    type : selectedEateryType,
                    key : APIKey
                },
                timeout:1000


            }).then((eateryData) => {//then, for each result, do places details search

                console.log("PLACES SEARCH RESPONSE IS: ");
                console.log("- response here -");
                // console.log(eateryData.data.results[0]);
                // console.log(eateryData);

                //do place details request for each result

                let counter = 0;

                for(let i = 0; i < eateryData.data.results.length; i++){

                    client.placeDetails({ params : {
                            place_id : eateryData.data.results[i].place_id,
                            key : APIKey
                        },
                    timeout : 1000
                    }).then((placeDetailsResponse) => {
                        console.log("PLACE DETAILS SEARCH RESPONSE IS: " + counter);
                        console.log("- response here -");

                        let currentDay = new Date().getDay();

                        if(placeDetailsResponse.data.result.opening_hours.periods[currentDay].open.time === undefined || placeDetailsResponse.data.result.opening_hours.periods[currentDay].close.time === undefined){
                            eateryData.data.results[i].openingTimeForToday = null;
                            eateryData.data.results[i].closingTimeForToday = null;
                        } else {
                            eateryData.data.results[i].openingTimeForToday = placeDetailsResponse.data.result.opening_hours.periods[currentDay].open.time;
                            eateryData.data.results[i].closingTimeForToday = placeDetailsResponse.data.result.opening_hours.periods[currentDay].close.time;
                        }

                        if(placeDetailsResponse.data.result.reviews){
                            eateryData.data.results[i].reviews = placeDetailsResponse.data.result.reviews;
                        }

                        counter++;

                        if(counter >= eateryData.data.results.length){
                            this.compileAndSendToUser(username, eateryData, time);
                        }

                    }).catch((err) => {
                        console.log("PLACE DETAILS SEARCH RESPONSE IS: " + counter);
                        console.log("- error! -");

                        counter++;
                        eateryData.data.results[i].openingTimeForToday = null;
                        eateryData.data.results[i].closingTimeForToday = null;

                        if(counter >= eateryData.data.results.length){
                            this.compileAndSendToUser(username, eateryData, time);
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

    compileAndSendToUser(username, eateryData, time){
        let eateryOptionsArray = this.createEateryOptionsArray(eateryData, time);

        let newActiveSearch = new ActiveSearch(this.getUser(username), eateryOptionsArray);

        ACTIVE_SEARCHES.push(newActiveSearch);

        //then pass a success message back to the user, containing the ActiveSearch object
        let MSG = new Message(1, "newActiveSearchRequestGranted", "", [newActiveSearch.ID, newActiveSearch.EateryOptions]);
        this.sendToUser(username, MSG);
    }

    createEateryOptionsArray(eateryData, desiredArrivalTime){



        let EateriesArray = [];

        try{
            if(eateryData.data.results.length === 0){
                return [];
            }

            for (let i = 0; i < eateryData.data.results.length; i++) {

                let ID;
                let name = eateryData.data.results[i].name;
                let description = "description would be here, if there was one";
                let rating = eateryData.data.results[i].rating;
                let photoRef0 = "";
                let photoRef1 = "";
                let photoRef2 = "";
                let photoRef3 = "";
                let photoRef4 = "";
                let Reviews = [];
                let PhotoReferences = [];
                let OpeningTime = eateryData.data.results[i].openingTimeForToday;
                let ClosingTime = eateryData.data.results[i].closingTimeForToday;
                let TimeToClosingTime = 0;
                let Notes = "";

                if(ClosingTime !=  null){
                    TimeToClosingTime = Number(ClosingTime.toString().slice(0, 2) - Number(desiredArrivalTime.toString().slice(0, 2)));
                }




                if(eateryData.data.results[i].photos){
                    ID = eateryData.data.results[i].photos[0].photo_reference;

                    if(eateryData.data.results[i].photos[0]){
                        photoRef0 = eateryData.data.results[i].photos[0].photo_reference
                    }
                    if(eateryData.data.results[i].photos[1]){
                        photoRef0 = eateryData.data.results[i].photos[1].photo_reference
                    }
                    if(eateryData.data.results[i].photos[2]){
                        photoRef0 = eateryData.data.results[i].photos[2].photo_reference
                    }
                    if(eateryData.data.results[i].photos[3]){
                        photoRef0 = eateryData.data.results[i].photos[3].photo_reference
                    }
                    if(eateryData.data.results[i].photos[4]){
                        photoRef0 = eateryData.data.results[i].photos[4].photo_reference
                    }

                    if(eateryData.data.results[i].reviews){
                        for(let y = 0; y < eateryData.data.results[i].reviews.length; y++){
                            Reviews.push({
                                AuthorName : eateryData.data.results[i].reviews[y].author_name.toString(),
                                Rating : eateryData.data.results[i].reviews[y].rating.toString(),
                                RelativeTimeDescription : eateryData.data.results[i].reviews[y].relative_time_description.toString(),
                                Text : eateryData.data.results[i].reviews[y].text.toString(),
                                TimeSinceReview : eateryData.data.results[i].reviews[y].time
                            })
                        }
                    }

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
                        Notes

                    );

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

        return EateriesArray;
    }

    async validateCredentials(username, plainTextPassword){
        if(username === "" || plainTextPassword === ""){
            return false;
        }

        let that = this;

        let credentialsValidity = await new Promise((resolve, reject) => {
            console.log(USERS);

            USERS.find(function (user) {//if credentials match existing user
                if(user.Username === username){
                    resolve(that.comparePassword(plainTextPassword, user.Password));
                }

            });
             console.log("returning... " + false);
             resolve(false);
        })

        return credentialsValidity;

    }

    grantLoginRequest(ws, username){
        //set user's new WSID
        //inform user that their login request is granted

        this.connectionsMap.set(this.getUser(username), ws);

        let MSG = new Message(1, "loginRequestGranted", "", []);

        try{
            ws.send(JSON.stringify(MSG));
            console.log('[LOGIN] user login succeeded');
        } catch {

        }
    }

        rejectLoginRequest(ws){
            //inform user that their login request is granted
            let MSG = new Message(1, "loginRequestRejected", "", []);

            try{
                ws.send(JSON.stringify(MSG));
                console.log('[LOGIN] user login rejected');
            } catch {

            }
        }

    async updateUsername(username, password, newUsername){
        console.log("[LOGIN] updating username...");

        let that = this;

        this.UserModel.updateOne(
            {
                Username : username
            },
            {
                Username : newUsername
            })
            .then((obj) => {
                let MSG = new Message(1, "usernameUpdated", newUsername, []);
                this.sendToUser(username, MSG);

                let thisUser = {
                    Username : username
                }

                // this.PastSearchesModel.updateMany(thisUser, { Username : newUsername}, function (err, result){
                //     console.log(result)
                // });

                this.PastSearchesModel.updateMany(thisUser, { Username : newUsername}, function (){
                    that.EateryOptionModel.updateMany(thisUser, { Username : newUsername}, function (){
                        that.UserModel.find({}, function (err, users) {
                            let ws = that.connectionsMap.get(that.getUser(username));
                            that.connectionsMap.delete(that.getUser(username));//wipe connections map entry for user
                            global.USERS = users;
                            that.connectionsMap.set(that.getUser(newUsername), ws);//replace entry, username change reflected
                        });
                    })
                })









                console.log("[LOGIN] username update succeeded!");
            })
    }

    async updatePassword(username, password, newPassword){
        console.log("[LOGIN] updating password...");

        let that = this;

        await this.generateSaltedAndHashedPassword(newPassword).then(
            async function (hashedPass){
                that.UserModel.updateOne(
                    {
                        Username : username
                    },
                    {
                        Password : hashedPass
                    })
                    .then((obj) => {
                        let MSG = new Message(1, "passwordUpdated", newPassword, []);
                        that.sendToUser(username, MSG);

                        let thisClass = this;

                        that.UserModel.find({}, function (err, users) {
                            let ws = that.connectionsMap.get(that.getUser(username));
                            that.connectionsMap.delete(that.getUser(username));
                            global.USERS = users;
                            that.connectionsMap.set(that.getUser(username), ws);
                        });

                        console.log("[LOGIN] password update succeeded!");
                    })
            }
        )


    }

    async deleteUser(username, password){
        console.log("[LOGIN] deleting user...");

        let that = this;

        this.UserModel.deleteOne({
            Username : username
        }).then((obj) => {
            let MSG = new Message(1, "userDeleted", "", []);
            this.sendToUser(username, MSG);

            let thisClass = this;

            this.UserModel.find({}, function (err, users) {
                thisClass.connectionsMap.delete(thisClass.getUser(username));
                global.USERS = users;
            });

            console.log("[LOGIN] user delete succeeded!");
        });

    }

    usernameNotTaken(username){
        if(username){
            if(USERS.find(function (user) {//if credentials match existing user, username is taken
                return (user.Username === username);
            })) {
                console.log('[LOGIN] Validation failure, username is taken!');
                return false;
            }
            else {

                return true;
            }
        }
    }
    async registerNewUser(username, password){
        let that = this;
        await this.generateSaltedAndHashedPassword(password).then(
            async function (hashedPass){
                let newUser = {
                    Username : username,
                    Password : hashedPass
                };

                await that.UserModel.create(newUser, async function (err) {
                    if(err){
                        console.log(err);
                    }

                    await that.UserModel.find({}, function (err, users) {
                        if(err){
                            console.log(err);
                        }
                        global.USERS = users;

                        console.log(newUser);
                        console.log(global.USERS);

                        console.log('[LOGIN] user registration succeeded');

                    });
                });
            }
        )



    }

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

    getUser(username){
        let targetUser = null;
        targetUser = USERS.find(function (User) {
            return User.Username.toString() === username.toString();
        });

        return targetUser;
    }

    getActiveSearch(ID) {
        let targetSearch = null;
        targetSearch = ACTIVE_SEARCHES.find(function (ActiveSearch, index) {
            return ActiveSearch.ID.toString() === ID.toString();
        });
        return targetSearch;
    }

    sendTestMessage(ws){
        ws.send(JSON.stringify(new Message("1", "debugMessage", "hello there")));
    }

    clearACTIVE_SEARCHES(){
        global.ACTIVE_SEARCHES = [];
    }

    clearUSERS(){
        global.USERS = [];
    }

    sendToUser(username, message){
        let ws = this.connectionsMap.get(this.getUser(username));

        if(ws){
            ws.send(JSON.stringify(message));
            console.log("[MSG] sent the following to user: " + username);
            console.log("Message type: " + message.type);
            console.log("Message body: " + message.Body);
            console.log("Message.items contains: " + message.Items.length + " object(s)");
        }



        // if(message.type === "newActiveSearchRequestGranted"){
        //     console.log("Message.items.eateryOptions contains: ");
        //     console.log(message.Items[1]);
        // }
    }
}

module.exports = ServerFunctions