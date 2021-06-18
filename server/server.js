const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');

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

const connectionsMap = new Map();

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

        let username = message.Items[0];
        let password = message.Items[1];

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
                        let EateryOptionsArray = createEateriesArray(eateryData)
                        let x = new Message(1, "eateryOptionsArray", JSON.stringify(EateryOptionsArray), []);
                        ws.send(JSON.stringify(x));

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

                if(validateNewUserRegistration(username)){
                    registerNewUser(username, password);
                }
                break

            case "loginExistingUser":
                console.log("[MSG] received login request");
                console.log(username);
                console.log(password);

                if(validateLogin(username, password)){
                    grantLoginRequest(ws, getUser(username));
                }


                break

            default:
                console.log('[MSG] unrecognised message received');
        }
    })
});

function validateLogin(username, password){
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





function validateNewUserRegistration(username){
    if(!USERS.find(function (user) {//if username is not taken
        return user.username === username;
    })){
        return true;


    } else {
        console.log('[LOGIN] user registration failed');
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



function createEateriesArray(eateryData){
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




server.listen(port, function () {
    console.log('[START] server listening on port: ' + port);

    var rl = readline.createInterface(process.stdin, process.stdout);
    rl.setPrompt('CMD:');
    rl.prompt();
    rl.on('line', function(line) {
        if (line === "stop") rl.close();

        if(line === 't'){
            wss.clients.forEach(function (ws){
                console.log('[msg] sending debug message...')
                ws.send(JSON.stringify(new Message("1", "debugMessage", "hello there")));
            })
        }


        rl.prompt();
    }).on('close',function(){
        process.exit(0);
    });
});

