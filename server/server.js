const Message = require('./objects/Message');

const ServerFunctions = require('./ServerFunctions');

const express = require('express');
const http = require('http');
const https = require('https');
const WebSocket = require("ws");
const { v4: uuid } = require('uuid');
const prompt = require('prompt');
const readline = require('readline');

const port = 9000;
const server = http.createServer(express);
const wss = new WebSocket.Server({ server });

serverFunctions = new ServerFunctions();
serverFunctions.initConnection();

//
wss.on('connection', function connection(ws) {
    console.log('[WS] new websocket connection established');



    ws.id = uuid();
    // connectionsMap.set(ws.id, ws);

    ws.on('close', function() {
        // remove from the map
        // connectionsMap.delete(ws);//this dont work
    });


    //on message recieved
    ws.on('message', function incoming(data){


        let message = JSON.parse(data);

        console.log('[MSG] got message of type: ' + message.type);

        let username = message.Items[0];
        let password = message.Items[1];

        let newUsername = message.Items[2];
        let newPassword = message.Items[2];

        let searchCode = message.Items[2];
        let eateryOptionID = message.Items[3];

        let locationName = message.Items[2];
        let time = message.Items[3];
        let eateryTypes = message.Items[4];

        let eateryTitle = message.Items[2];
        let note = message.Items[3];


        //switch on the message's type, each type corresponds to a function in the serverFunctions class
        switch(message.type) {
            case "testMessage":
                console.log("[MSG] received debug message request, pinging back");
                serverFunctions.sendTestMessage(ws);
                break;


            case "deleteFavouriteEatery":
                console.log("[MSG] received delete favourite eatery request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting delete favourite eatery, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.deleteFavouriteEatery(username, eateryTitle);
                    }
                })
                break;


            case "updateNote":
                console.log("[MSG] received updateNote request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting updateNote, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.updateFavouriteEateryNote(username, eateryTitle, note);
                    }
                })
                break;


            case "getFavourites":
                console.log("[MSG] received get favourites request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting get favourites, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.getFavourites(username, password);
                    }
                })
                break;


            case "addToFavourites":
                console.log("[MSG] received add to favourites request");

                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting add to favourites, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.addEateryToFavourites(username, message.Items[2]);
                    }
                })
                break;

            case "getPastSearches":
                console.log("[MSG] received get past searches request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting get past searches, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.getPastSearches(username);
                    }
                })
                break;

            case "registerNewUser":
                console.log("[MSG] received register new user request");
                if(serverFunctions.usernameNotTaken(username)){
                    serverFunctions.registerNewUser(username, password, ws);
                } else {
                    serverFunctions.rejectRegistration(ws);
                }
                break

            case "loginExistingUser":
                console.log("[MSG] received login request");

                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting log in, credentials are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.grantLoginRequest(ws, username);
                    } else {
                        serverFunctions.rejectLoginRequest(ws);
                    }
                })
                break

            case "updateUsername":
                console.log("[MSG] received update username request");

                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting update username, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        if(serverFunctions.usernameNotTaken(newUsername)){
                            serverFunctions.updateUsername(username, password, newUsername);
                        } else {
                            serverFunctions.rejectUpdateUsernameRequest(ws);
                        }
                    }
                })
                break

            case "updatePassword":
                console.log("[MSG] received update password request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting update password, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.updatePassword(username, password, newPassword);
                    } else {
                        serverFunctions.rejectUpdatePasswordRequest(ws);
                    }
                })

                break

            case "deleteUser":
                console.log("[MSG] received delete user request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting delete user, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.deleteUser(username, password);
                    }
                })

                break

            case "startNewSearch":
                console.log("[MSG] received start new search request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting start new search, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.createNewActiveSearch(username, locationName, time, eateryTypes);
                    }
                })

                break

            case "joinExistingSearch":
                console.log("[MSG] received join existing search request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting join existing search, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.joinExistingSearch(searchCode, username);
                    }
                })

                break

            case "castVote":
                console.log("[MSG] received cast vote request");
                serverFunctions.validateCredentials(username, password).then((credentialsAreValid)=>{
                    console.log("attempting cast vote, creds are: " + credentialsAreValid);
                    if(credentialsAreValid){
                        serverFunctions.castVoteInSearch(searchCode, username, eateryOptionID);
                    }
                })
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





