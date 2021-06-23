const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');
const ActiveSearch = require('./objects/ActiveSearch');

const express = require('express');
const http = require('http');
const https = require('https');
const WebSocket = require("ws");
const { v4: uuid } = require('uuid');
const prompt = require('prompt');
const readline = require('readline');

const {Client} = require('@googlemaps/google-maps-services-js');
const client = new Client({});

const port = 9000;
const server = http.createServer(express);
const wss = new WebSocket.Server({ server });

let connectionsMap = new Map();

let DB = require('./DB');


//set up DB stuff
DB.initialiseConnection();
let User = DB.getUserModel();

global.USERS = [];
global.CONNECTED_USERS = [];
global.ACTIVE_SEARCHES = [];

User.find({}, function (err, users) {
    global.USERS = users;
});



wss.on('connection', function connection(ws) {
    console.log('[WS] new websocket connection established');


    ws.id = uuid();
    // connectionsMap.set(ws.id, ws);

    ws.on('close', function() {
        // remove from the map
        connectionsMap.delete(ws);//this dont work
    });

    ws.on('message', function incoming(data){
        console.log('[MSG] received message');

        let message = JSON.parse(data);

        // message.Items = [0,0,0,0,0,0,0,0,0]

        let username = message.Items[0];
        let password = message.Items[1];
        let newUsername = message.Items[2];
        let newPassword = message.Items[2];
        let latitude = message.Items[2];
        let longitude = message.Items[3];

        switch(message.type) {
            case "getEateries":
                console.log("[MSG] received getEateries request");
                //make api call
                client.placesNearby({params:{
                        location : [message.latitude, message.longitude],
                        // location : [50.381773,-4.133786],
                        radius : 1500,
                        type : "restaurant",
                        key : "AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"
                    },
                    timeout:1000

                }).then((eateryData) => {

                    if(ws.readyState === WebSocket.OPEN){
                        let EateryOptionsArray = createEateryOptionsArray(eateryData)
                        let x = new Message(1, "eateryOptionsArray", JSON.stringify(EateryOptionsArray), []);
                        ws.send(JSON.stringify(x));

                        console.log("[PLACES] response is as follows: ");
                        console.log(JSON.stringify(EateryOptionsArray));

                    }
                }).catch((error) => {
                    console.log('[ERROR]');
                    console.log(error);

                });

                break;

            case "testMessage":
                console.log("[MSG] received debug message request, pinging back");
                ws.send(JSON.stringify(new Message("1", "debugMessage", "hello there")));
                break;

            case "registerNewUser":
                console.log("[MSG] received register new user request");
                console.log(username);
                console.log(password);

                if(usernameNotTaken(username)){
                    registerNewUser(username, password);
                }
                break

            case "loginExistingUser":
                console.log("[MSG] received login request");
                console.log(username);
                console.log(password);

                if(validateCredentials(username, password)){
                    grantLoginRequest(ws, getUser(username));
                }


                break

            case "updateUsername":
                console.log("[MSG] received update username request");
                console.log(username);
                console.log(password);
                console.log(newUsername);

                if(validateCredentials(username, password)){//if credentials are valid
                    if(usernameNotTaken(newUsername)){
                        updateUsername(username, password, newUsername);
                    }
                }
                break

            case "updatePassword":
                console.log("[MSG] received update password request");
                console.log(username);
                console.log(password);
                console.log(newPassword)

                if(validateCredentials(username, password)){//credentials are valid
                    //do update password
                    //pass feedback to app
                    updatePassword(username, password, newPassword);
                }
                break

            case "deleteUser":
                console.log("[MSG] received delete user request");
                console.log(username);
                console.log(password);

                if(validateCredentials(username, password)){//credentials are valid
                    //do delete user
                    //pass feedback to app
                    deleteUser(username, password);
                }

            case "startNewSearch":
                console.log("[MSG] received start new search request");
                console.log(message);

                if(validateCredentials(username, password)){//credentials are valid
                    //do create new search
                    createNewActiveSearch(username, latitude, longitude);
                }
                break

            default:
                console.log('[MSG] unrecognised message received');
        }
    })
});

async function createNewActiveSearch(username, latitude, longitude){
    //create an active search object, populated with a user and eatery options array

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

        eateryOptionsArray = createEateryOptionsArray(eateryData);

        console.log("got eateryOptionsArray after construction... is as follows: ");
        console.log("type: " + typeof (eateryOptionsArray) + " length: " + eateryOptionsArray.length);

        let newActiveSearch = new ActiveSearch(getUser(username), eateryOptionsArray);

        //add the active search object to ACTIVE_SEARCHES
        ACTIVE_SEARCHES.push(newActiveSearch);

        //then pass a success message back to the user, containing the ActiveSearch object
        let MSG = new Message(1, "newActiveSearchRequestGranted", "", [newActiveSearch.ID, newActiveSearch.EateryOptions]);
        sendToUser(username, MSG);

    }).catch((error) => {
        console.log('[ERROR]');
        console.log(error);

    });
}

function createEateryOptionsArray(eateryData){
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
            if(eateryData.data.results[i].photos){
                photoRef = eateryData.data.results[i].photos[0].photo_reference;
            } else {
                photoRef = "";
            }

            let eatery = new EateryOption(
                photoRef,
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

function getEateryOptionsFromAPI(latitude, longitude){
    //make api call
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

        eateryOptionsArray = createEateryOptionsArray(eateryData);

        console.log("constructed eateryOptionsArray is as follows: ");
        console.log("- options array here -");
        // console.log(eateryOptionsArray);
        return eateryOptionsArray;

    }).catch((error) => {
        console.log('[ERROR]');
        console.log(error);

    });
}





function validateCredentials(username, password){
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

function grantLoginRequest(ws, user){
    //set user's new WSID
    //inform user that their login request is granted

    connectionsMap.set(user, ws);

    let MSG = new Message(1, "loginRequestGranted", "", []);
    ws.send(JSON.stringify(MSG));

    console.log('[LOGIN] user login succeeded');
}

function updateUsername(username, password, newUsername){
    console.log("[LOGIN] updating username...");


    User.updateOne(
        {
            username : username,
            password : password
        },
        {
            username : newUsername
        })
        .then((obj) => {
            let MSG = new Message(1, "usernameUpdated", newUsername, []);
            sendToUser(username, MSG);

            User.find({}, function (err, users) {
                connectionsMap.delete(getUser(username));
                global.USERS = users;
                connectionsMap.set(getUser(newUsername), ws);
            });

            console.log("[LOGIN] username update succeeded!");
    })
}

function updatePassword(username, password, newPassword){
    console.log("[LOGIN] updating password...");

    User.updateOne(
        {
            username : username,
            password : password
        },
        {
            password : newPassword
        })
        .then((obj) => {
            let MSG = new Message(1, "passwordUpdated", newPassword, []);
            sendToUser(username, MSG);

            User.find({}, function (err, users) {
                connectionsMap.delete(getUser(username));
                global.USERS = users;
                connectionsMap.set(getUser(username), ws);
            });

            console.log("[LOGIN] password update succeeded!");
        })
}

function deleteUser(username, password){
    console.log("[LOGIN] deleting user...");

    User = DB.getUserModel();

    User.deleteOne({
        username : username,
        password : password
    }).then((obj) => {
        let MSG = new Message(1, "userDeleted", "", []);
        sendToUser(username, MSG);

        User.find({}, function (err, users) {
            connectionsMap.delete(getUser(username));
            global.USERS = users;
        });

        console.log("[LOGIN] user delete succeeded!");
    });
}

function usernameNotTaken(username){
    if(!USERS.find(function (user) {//if username is not taken
        return user.username === username;
    })){
        return true;


    } else {
        console.log('[LOGIN] Validation failure, username is taken!');
        return false;
    }
}

async function registerNewUser(username, password){
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

function getUser(username){
    let targetUser = null;
    targetUser = USERS.find(function (User) {
        return User.username.toString() === username.toString();
    });

    return targetUser;
}





function sendToUser(username, message){
    let ws = connectionsMap.get(getUser(username));
    ws.send(JSON.stringify(message));
    console.log("[MSG] sent the following to user: " + username);
    console.log("Message type: " + message.type);
    console.log("Message body: " + message.Body);
    console.log("Message.items contains: " + message.Items.length + " object(s)");
}


server.listen(port, function () {
    console.log('[START] server listening on port: ' + port);

    var rl = readline.createInterface(process.stdin, process.stdout);
    rl.setPrompt('CMD:');
    rl.prompt();
    rl.on('line', function(line) {
        if (line === "stop") rl.close();

        if(line === 't'){
            wss.clients.forEach(function (ws){
                console.log('[MSG] sending debug message...')
                ws.send(JSON.stringify(new Message("1", "debugMessage", "hello there")));
            })
        }


        rl.prompt();
    }).on('close',function(){
        process.exit(0);
    });
});

