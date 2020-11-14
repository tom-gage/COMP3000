const express = require('express');
const http = require('http');
const WebSocket = require("ws");

const port = 9000;
const server = http.createServer(express);
const wss = new WebSocket.Server({ server });

var increment = 0;

wss.on('connection', function connection(ws) {
    ws.on('message', function incoming(data){
        increment += 1;
        console.log("message No. " + increment +" recieved: " + data);

        wss.clients.forEach(function (client) {
            if(client.readyState === WebSocket.OPEN){
                client.send(data);
            }
        })
    })
});

server.listen(port, function () {
    console.log('server listening on port: ' + port)
});