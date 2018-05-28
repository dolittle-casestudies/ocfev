/*
var mqtt = require("MQTT").create(server, options);
mqtt.on('connected', function() {
  console.log("Connected");
    mqtt.subscribe("blah");
    mqtt.publish("blah","{'angle_wind_relative':178.4}",0);
});
mqtt.on("error", function(message) {
  console.log("Error : "+message);
});

mqtt.on('disconnected', function() {
  console.log("MQTT disconnected...");
});

mqtt.on('publish', function(pub) {
  console.log("topic: "+pub.topic);
  console.log("message: "+pub.message);
});

mqtt.connect();
*/

var wifi = require("Wifi");
//wifi.getStatus(function(status) {
//  console.log("Wifi Status : "+status);
//  mqtt.connect();
//});


/*
wifi.connect("The Ingebrigtsens", {password:"Agei1208"}, function(ap) {
  console.log("Wifi Connected");
  mqtt.connect();
});*/

