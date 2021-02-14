const EateryOption = require('./objects/EateryOption');

const express = require('express');
const http = require('http');
const https = require('https');
const WebSocket = require("ws");


const {Client} = require('@googlemaps/google-maps-services-js');
const client = new Client({});

const port = 9000;
const server = http.createServer(express);
const wss = new WebSocket.Server({ server });

var increment = 0;


var restaurantData;

// let server = http.createServer(app);

// setup sockets
// let io = socketIo(server);
// io.on('connection', async function(socket) {
//     console.log('got connection');
//     socket.on('please send me some restaurant data', async function(msg) {
//         console.log('message received: ' + msg);
//
//     });
// });
//
//
// server.listen(9000, function (request, response) {
//     console.log('listening on port 9000');
// });


wss.on('connection', function connection(ws) {
    console.log('got connection');
    ws.on('message', function incoming(data){
        console.log('got message');

        let message = JSON.parse(data);

        console.log(message);



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
                }).then((response) => {

                    // console.log("response is: " + response.data.results[0].name);
                    // console.log(response);
                    restaurantData = response;

                    // returns eatery list to each client, needs refactoring
                    wss.clients.forEach(function (WSClient) {
                        if (WSClient.readyState === WebSocket.OPEN) {



                            EateryList = new Array();

                            for (i = 0; i < restaurantData.data.results.length; i++) {
                                // console.log("adding: " + restaurantData.data.results[i].toString());
                                // console.log('data, photos: ' + restaurantData.data.results[i].photos[0].photo_reference);

                                let eatery = new EateryOption(
                                    restaurantData.data.results[i].name,
                                    "description would be here, if there was one",
                                    restaurantData.data.results[i].rating,
                                    restaurantData.data.results[i].photos[0].photo_reference
                                );

                                EateryList.push(eatery);
                            }

                            console.log('sending eateryList to client: ' + JSON.stringify(EateryList));
                            WSClient.send(JSON.stringify(EateryList));
                        }
                    });
                })
                //     .catch((error) => {
                //     console.log("ERROR");
                //     console.log("error is: " + error);
                //     // console.log("error is: " + error.response.data.error_message);
                // });
                break;

            default:
                console.log('message received but not recognised');
        }
    })
});

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

// wss.on('connection', function connection(ws) {
//     ws.on('message', function incoming(data){
//         increment += 1;
//         console.log("message No. " + increment +" recieved: " + data);
//
//         if(data === "please send me some restaurant data"){
//             wss.clients.forEach(function (client) {
//                 if (client.readyState === WebSocket.OPEN) {
//                     console.log('sending data: ' + restaurantData);
//
//
//
//
//                     EateryList = new Array();
//
//                     for (i = 0; i < restaurantData.data.results.length; i++) {
//                         console.log("adding: " + restaurantData.data.results[i].toString());
//                         console.log('data, photos: ' + restaurantData.data.results[i].photos[0].photo_reference);
//                         let eatery = new EateryOption(
//                             restaurantData.data.results[i].name,
//                             "description would be here, if there was one",
//                             restaurantData.data.results[i].rating, restaurantData.data.results[i].photos[0].photo_reference
//                         );
//                         EateryList.push(eatery);
//                     }
//
//                     client.send(JSON.stringify(EateryList));
//                 }
//             });
//         }
//
//
//
//     })
// });
//
// server.listen(port, function () {
//     console.log('server listening on port: ' + port);
//
//     //request
//     client.placesNearby({params:{
//             location : [50.381773,-4.133786],
//             radius : 50,
//             type : "restaurant",
//             key : "AIzaSyBbIr0ggukOfFiCFLoQcpypMmhA5NAYCZw"
//         },
//         timeout:1000
//     }).then((response) => {
//         console.log("response is: " + response.data.results[0].name);
//         restaurantData = response;
//
//     }).catch((error) => {
//         console.log("error is: " + error.response.data.error_message);
//     });
// });