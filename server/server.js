// const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');
// const ActiveSearch = require('./objects/ActiveSearch');
// const UserObj = require('./objects/User');

const ServerFunctions = require('./ServerFunctions');

const express = require('express');
const http = require('http');
const https = require('https');
const WebSocket = require("ws");
const { v4: uuid } = require('uuid');
const prompt = require('prompt');
const readline = require('readline');

// const {Client} = require('@googlemaps/google-maps-services-js');
// const client = new Client({});

const port = 9000;
const server = http.createServer(express);
const wss = new WebSocket.Server({ server });

serverFunctions = new ServerFunctions();
serverFunctions.initConnection();

wss.on('connection', function connection(ws) {
    console.log('[WS] new websocket connection established');



    ws.id = uuid();
    // connectionsMap.set(ws.id, ws);

    ws.on('close', function() {
        // remove from the map
        // connectionsMap.delete(ws);//this dont work
    });

    ws.on('message', function incoming(data){
        console.log('[MSG] received message');

        let message = JSON.parse(data);

        console.log('message type: ' + message.type);

        // message.Items = [0,0,0,0,0,0,0,0,0]

        let username = message.Items[0];
        let password = message.Items[1];

        let newUsername = message.Items[2];
        let newPassword = message.Items[2];

        let latitude = message.Items[2];
        let longitude = message.Items[3];

        let searchCode = message.Items[2];
        let eateryOptionID = message.Items[3];

        let locationName = message.Items[2];
        let time = message.Items[3];
        let eateryTypes = message.Items[4];

        switch(message.type) {
            case "testMessage":
                console.log("[MSG] received debug message request, pinging back");
                serverFunctions.sendTestMessage(ws);
                break;

            case "getPastSearches":
                // console.log("[MSG] received get past searches request");
                serverFunctions.getPastSearches(username);
                break;

            case "registerNewUser":
                console.log("[MSG] received register new user request");
                console.log(username);
                console.log(password);

                if(serverFunctions.usernameNotTaken(username)){
                    serverFunctions.registerNewUser(username, password);
                }
                break

            case "loginExistingUser":
                console.log("[MSG] received login request");
                console.log(username);
                console.log(password);

                if(serverFunctions.validateCredentials(username, password)){
                    serverFunctions.grantLoginRequest(ws, username);

                    //test stuff, makes it so there is a search that can be joined
                    // serverFunctions.grantLoginRequest(ws, "testUser");
                    // serverFunctions.createNewActiveSearch("testUser", 37.421998333333335, -122.08400000000002);
                }
                break

            case "updateUsername":
                console.log("[MSG] received update username request");
                console.log(username);
                console.log(password);
                console.log(newUsername);

                if(serverFunctions.validateCredentials(username, password)){//if credentials are valid
                    if(serverFunctions.usernameNotTaken(newUsername)){
                        serverFunctions.updateUsername(username, password, newUsername);
                    }
                }
                break

            case "updatePassword":
                console.log("[MSG] received update password request");
                console.log(username);
                console.log(password);
                console.log(newPassword)

                if(serverFunctions.validateCredentials(username, password)){//credentials are valid
                    //do update password
                    //pass feedback to app
                    serverFunctions.updatePassword(username, password, newPassword);
                }
                break

            case "deleteUser":
                console.log("[MSG] received delete user request");
                console.log(username);
                console.log(password);

                if(serverFunctions.validateCredentials(username, password)){//credentials are valid
                    //do delete user
                    //pass feedback to app
                    serverFunctions.deleteUser(username, password);
                }
                break

            case "startNewSearch":
                console.log("[MSG] received start new search request");
                console.log(message);

                if(serverFunctions.validateCredentials(username, password)){//credentials are valid
                    //do create new search
                    serverFunctions.createNewActiveSearch(username, locationName, time, eateryTypes);
                }
                break

            case "joinExistingSearch":
                console.log("[MSG] received join existing search request");
                console.log(message);

                if(serverFunctions.validateCredentials(username, password)){//credentials are valid
                    //do create new search
                    serverFunctions.joinExistingSearch(searchCode, username);
                }
                break

            case "castVote":
                console.log("[MSG] received cast vote request");
                console.log(message);

                if(serverFunctions.validateCredentials(username, password)){//credentials are valid
                    serverFunctions.castVoteInSearch(searchCode, username, eateryOptionID);
                }
                break

            default:
                console.log('[MSG] unrecognised message received');
        }
    })
});

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





