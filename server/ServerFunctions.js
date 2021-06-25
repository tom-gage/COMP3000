const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');
const ActiveSearch = require('./objects/ActiveSearch');
const UserObj = require('./objects/User');
const {Client} = require('@googlemaps/google-maps-services-js');
const client = new Client({});

let DB = require('./DB');
let UserModel;

let connectionsMap;

class ServerFunctions{

    constructor() {
        //set up DB stuff
        DB.initialiseConnection();

        UserModel = DB.getUserModel();
        UserModel.find({}, function (err, users) {
            global.USERS = users;
        });

        connectionsMap = new Map();
    }


    castVoteInSearch(searchCode, username, eateryOptionID){
        //get active search by searchcode
        let search = this.getActiveSearch(searchCode);
        //cast vote
        search.castVote(username, eateryOptionID);

        //if users have matched, send "you matched!" feedback
        let match = search.checkForMatch();

        if(match){
            let MSG = new Message(1, "matched!", "", [match]);
            this.sendToUser(username, MSG);
        }

        search.getVotes();
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

    createNewActiveSearch(username, latitude, longitude){
        //create an active search object, populated with a user and eatery options array
        console.log("[SEARCH] creating new search");

        console.log("making API call...");
        console.log(latitude);
        console.log(longitude);
        let eateryOptionsArray = [];

        client.placesNearby({params:{
                location : [latitude, longitude],
                // location : [50.381773,-4.133786],
                radius : 1500,
                type : "restaurant",
                key : "AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"
            },
            timeout:1000

        }).then((eateryData) => {
            console.log("got response from api! data is as follows:");
            console.log("- data here -");
            // console.log(eateryData);

            eateryOptionsArray = this.createEateryOptionsArray(eateryData);

            console.log("got eateryOptionsArray after construction... is as follows: ");
            console.log("type: " + typeof (eateryOptionsArray) + " length: " + eateryOptionsArray.length);


            let newActiveSearch = new ActiveSearch(this.getUser(username), eateryOptionsArray);

            //add the active search object to ACTIVE_SEARCHES
            ACTIVE_SEARCHES.push(newActiveSearch);

            console.log("New ActiveSearch's search code is: " + newActiveSearch.ID);

            //then pass a success message back to the user, containing the ActiveSearch object
            let MSG = new Message(1, "newActiveSearchRequestGranted", "", [newActiveSearch.ID, newActiveSearch.EateryOptions]);
            this.sendToUser(username, MSG);

        }).catch((error) => {
            console.log('[ERROR]');
            console.log(error);

        });
    }

    createEateryOptionsArray(eateryData){
        let EateriesArray = [];

        try{
            if(eateryData.data.results.length === 0){
                return [];
            }

            for (let i = 0; i < eateryData.data.results.length; i++) {

                let name = eateryData.data.results[i].name;
                let description = "description would be here, if there was one";
                let rating = eateryData.data.results[i].rating;
                let photoRef;
                let ID;

                if(eateryData.data.results[i].photos){
                    ID = eateryData.data.results[i].photos[0].photo_reference;
                    photoRef = eateryData.data.results[i].photos[0].photo_reference;
                } else {
                    photoRef = "";
                }

                let eatery = new EateryOption(
                    ID,
                    name,
                    description,
                    rating,
                    photoRef
                );

                EateriesArray.push(eatery);
            }
        } catch {
            console.log('[ERROR] could not parse places data');
            return [];
        }

        return EateriesArray;
    }

    validateCredentials(username, password){
        if(username && password){
            if(USERS.find(function (user) {//if credentials match existing user
                return (user.username === username && user.password === password);
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

        connectionsMap.set(this.getUser(username), ws);

        let MSG = new Message(1, "loginRequestGranted", "", []);
        ws.send(JSON.stringify(MSG));

        console.log('[LOGIN] user login succeeded');
    }

    updateUsername(username, password, newUsername){
        console.log("[LOGIN] updating username...");


        UserModel.updateOne(
            {
                username : username,
                password : password
            },
            {
                username : newUsername
            })
            .then((obj) => {
                let MSG = new Message(1, "usernameUpdated", newUsername, []);
                this.sendToUser(username, MSG);

                let thisClass = this;

                UserModel.find({}, function (err, users) {
                    let ws = connectionsMap.get(thisClass.getUser(username));
                    connectionsMap.delete(thisClass.getUser(username));//wipe connections map entry for user
                    global.USERS = users;
                    connectionsMap.set(thisClass.getUser(newUsername), ws);//replace entry, username change reflected
                });



                console.log("[LOGIN] username update succeeded!");
            })
    }

    updatePassword(username, password, newPassword){
        console.log("[LOGIN] updating password...");

        UserModel.updateOne(
            {
                username : username,
                password : password
            },
            {
                password : newPassword
            })
            .then((obj) => {
                let MSG = new Message(1, "passwordUpdated", newPassword, []);
                this.sendToUser(username, MSG);

                let thisClass = this;

                UserModel.find({}, function (err, users) {
                    let ws = connectionsMap.get(thisClass.getUser(username));
                    connectionsMap.delete(thisClass.getUser(username));
                    global.USERS = users;
                    connectionsMap.set(thisClass.getUser(username), ws);
                });

                console.log("[LOGIN] password update succeeded!");
            })
    }

    deleteUser(username, password){
        console.log("[LOGIN] deleting user...");

        UserModel = DB.getUserModel();

        UserModel.deleteOne({
            username : username,
            password : password
        }).then((obj) => {
            let MSG = new Message(1, "userDeleted", "", []);
            this.sendToUser(username, MSG);

            let thisClass = this;

            UserModel.find({}, function (err, users) {
                connectionsMap.delete(thisClass.getUser(username));
                global.USERS = users;
            });

            console.log("[LOGIN] user delete succeeded!");
        });
    }

    usernameNotTaken(username){
        if(!USERS.find(function (user) {//if username is not taken
            return user.username === username;
        })){
            return true;
        } else {
            console.log('[LOGIN] Validation failure, username is taken!');
            return false;
        }
    }

    async registerNewUser(username, password){
        let newUser = {
            username : username,
            password : password,
        };

        let UsersModel = DB.getUserModel();

        await UsersModel.create(newUser, function (err) {
            if (err) return console.log(err);
        });

        await UsersModel.find({}, function (err, users) {
            global.USERS = users;
        });

        console.log('[LOGIN] user registration succeeded');
    }

//helper functions

    getUser(username){
        let targetUser = null;
        targetUser = USERS.find(function (User) {
            return User.username.toString() === username.toString();
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



    sendToUser(username, message){
        let ws = connectionsMap.get(this.getUser(username));
        ws.send(JSON.stringify(message));
        console.log("[MSG] sent the following to user: " + username);
        console.log("Message type: " + message.type);
        console.log("Message body: " + message.Body);
        console.log("Message.items contains: " + message.Items.length + " object(s)");

        // if(message.type === "newActiveSearchRequestGranted"){
        //     console.log("Message.items.eateryOptions contains: ");
        //     console.log(message.Items[1]);
        // }
    }
}

module.exports = ServerFunctions