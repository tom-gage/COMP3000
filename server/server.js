const EateryOption = require('./objects/EateryOption');
const Message = require('./objects/Message');

const express = require('express');
const http = require('http');
const https = require('https');
const WebSocket = require("ws");
const { v4: uuid } = require('uuid');

const {Client} = require('@googlemaps/google-maps-services-js');
const client = new Client({});

const port = 9000;
const server = http.createServer(express);
const wss = new WebSocket.Server({ server });

const idMap = new Map();


var restaurantData;




wss.on('connection', function connection(ws) {
    console.log('got connection');
    ws.id = uuid();
    idMap.set(ws.id, ws);

    ws.on('close', function() {
        // remove from the map
        idMap.delete(ws.id);
    });

    ws.on('message', function incoming(data){
        console.log('got message');

        let message = JSON.parse(data);
        console.log(message);
        console.log(message.type);



        switch(message.type) {
            case "getEateries":

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
                        let x = new Message(1, "eatery options array", JSON.stringify(EateryOptionsArray));
                        ws.send(JSON.stringify(x));

                    }
                }).catch((error) => {

                    console.log("got error: " + error);

                });
                break;

            case "testMessage":
                console.log("test message received!");
                ws.send(JSON.stringify(new Message("1", "debugMessage", "hello there")));
                break;
            default:
                console.log('message received but not recognised');
        }
    })
});

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
        console.log('could not parse places data');
        return [];
    }

    return EateriesArray;
}

server.listen(port, function () {
    console.log('server listening on port: ' + port);

    // //api call
    // client.placesNearby({params:{
    //         location : [50.381773,-4.133786],
    //         radius : 50,
    //         type : "restaurant",
    //         key : "AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"
    //     },
    //     timeout:1000
    // }).then((response) => {
    //     console.log("response is: " + response.data.results[0].name);
    //     restaurantData = response;
    //
    // }).catch((error) => {
    //     console.log("error is: " + error.response.data.error_message);
    // });

    // console.log({
    //     type : "getEateries",
    //     body : "",
    //     latitude : "",
    //     longitude : ""
    // })
});

