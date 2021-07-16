const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');
const ActiveSearch = require('./objects/ActiveSearch');
const UserObj = require('./objects/User');
const {Client} = require('@googlemaps/google-maps-services-js');
const client = new Client({});

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

    constructor() {
        // this.connectionsMap = new Map();
    }

    async initConnection(){
        await DB.initialiseConnection();

        this.UserModel = await DB.getUserModel();
        await this.UserModel.find({}, function (err, users) {
                global.USERS = users;
        });


    }

    async closeConnection(){
        await DB.closeConnection();
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
        //get user by username
        //add user to the search
        search.addParticipant(this.getUser(username));

        //send feedback to app
        let MSG = new Message(1, "joinSearchRequestGranted", "", [search.ID, search.EateryOptions, search.Participants]);
        this.sendToUser(username, MSG);

    }

    async createNewActiveSearch(username, locationName, time, eateryTypes){
        //create an active search object, populated with a user and eatery options array
        console.log("[SEARCH] creating new search");

        console.log("making API calls with following data: ");

        console.log(locationName);
        console.log(time);
        console.log(eateryTypes);

        let selectedEateryType = eateryTypes[0];

        let eateryOptionsArray = [];

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
                        console.log("PLACE DETAILS SEARCH RESPONSE IS: ");
                        console.log("- response here -");

                        let currentDay = new Date().getDay();

                        if(placeDetailsResponse.data.result.opening_hours.periods[currentDay].open.time === undefined || placeDetailsResponse.data.result.opening_hours.periods[currentDay].close.time === undefined){
                            eateryData.data.results[i].openingTimeForToday = null;
                            eateryData.data.results[i].closingTimeForToday = null;
                        } else {
                            eateryData.data.results[i].openingTimeForToday = placeDetailsResponse.data.result.opening_hours.periods[currentDay].open.time;
                            eateryData.data.results[i].closingTimeForToday = placeDetailsResponse.data.result.opening_hours.periods[currentDay].close.time;
                        }

                        counter++;

                        if(counter >= eateryData.data.results.length){
                            this.compileAndSendToUser(username, eateryOptionsArray, time);
                        }

                    }).catch((err) => {
                        console.log("PLACE DETAILS SEARCH RESPONSE IS: ");
                        console.log("- response here -");

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

        // console.log(JSON.stringify(eateryData));

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
                let photoRef;
                let OpeningTime = eateryData.data.results[i].openingTimeForToday;
                let ClosingTime = eateryData.data.results[i].closingTimeForToday;
                let TimeToClosingTime = 0;

                if(ClosingTime !=  null){
                    TimeToClosingTime = Number(ClosingTime.toString().slice(0, 2) - Number(desiredArrivalTime.toString().slice(0, 2)));
                }




                if(eateryData.data.results[i].photos){
                    ID = eateryData.data.results[i].photos[0].photo_reference;
                    photoRef = eateryData.data.results[i].photos[0].photo_reference;

                    let eatery = new EateryOption(
                        ID,
                        name,
                        description,
                        rating,
                        photoRef,
                        OpeningTime,
                        ClosingTime,
                        TimeToClosingTime.toString()

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

    validateCredentials(username, password){
        if(username === "" || password === ""){
            return false;
        }

        if(username && password){
            if(USERS.find(function (user) {//if credentials match existing user
                return (user.Username === username && user.Password === password);
            })) {
                return true;
            }
            else {
                console.log('[LOGIN] user login failed');
                return false;
            }
        }
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

    async updateUsername(username, password, newUsername){
        console.log("[LOGIN] updating username...");


        this.UserModel.updateOne(
            {
                Username : username,
                Password : password
            },
            {
                Username : newUsername
            })
            .then((obj) => {
                let MSG = new Message(1, "usernameUpdated", newUsername, []);
                this.sendToUser(username, MSG);

                let thisClass = this;

                this.UserModel.find({}, function (err, users) {
                    let ws = thisClass.connectionsMap.get(thisClass.getUser(username));
                    thisClass.connectionsMap.delete(thisClass.getUser(username));//wipe connections map entry for user
                    global.USERS = users;
                    thisClass.connectionsMap.set(thisClass.getUser(newUsername), ws);//replace entry, username change reflected
                });



                console.log("[LOGIN] username update succeeded!");
            })
    }

    async updatePassword(username, password, newPassword){
        console.log("[LOGIN] updating password...");

        this.UserModel.updateOne(
            {
                Username : username,
                Password : password
            },
            {
                Password : newPassword
            })
            .then((obj) => {
                let MSG = new Message(1, "passwordUpdated", newPassword, []);
                this.sendToUser(username, MSG);

                let thisClass = this;

                this.UserModel.find({}, function (err, users) {
                    let ws = thisClass.connectionsMap.get(thisClass.getUser(username));
                    thisClass.connectionsMap.delete(thisClass.getUser(username));
                    global.USERS = users;
                    thisClass.connectionsMap.set(thisClass.getUser(username), ws);
                });

                console.log("[LOGIN] password update succeeded!");
            })
    }

    async deleteUser(username, password){
        console.log("[LOGIN] deleting user...");

        // UserModel = DB.getUserModel();

        this.UserModel.deleteOne({
            Username : username,
            Password : password
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
        let newUser = {
            Username : username,
            Password : password,
        };

        // let UsersModel = DB.getUserModel();

        await this.UserModel.create(newUser, function (err) {
            if (err) return console.log(err);
        });

        await this.UserModel.find({}, function (err, users) {
            global.USERS = users;
        });

        console.log('[LOGIN] user registration succeeded');
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