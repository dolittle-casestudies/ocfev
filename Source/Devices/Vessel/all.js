// dolittle-iot.westeurope.cloudapp.azure.com

I2C1.setup({scl:22,sda:21, bitrate: 100000});
const mpu = require("MPU6050").connect(I2C1);
function mtStr(s) {
  return String.fromCharCode(s.length>>8,s.length&255)+s;
} 
function mtPacket(cmd, variable, payload) {
  return String.fromCharCode(cmd, variable.length+payload.length)+variable+payload;
}
function mtpConnect(name) {
  return mtPacket(0b00010000, 
           mtStr("MQTT")/*protocol name*/+
           "\x04"/*protocol level*/+
           "\x00"/*connect flag*/+
           "\xFF\xFF"/*Keepalive*/, mtStr(name));
}
function mtpPub(topic, data) {
  return  mtPacket(0b00110001, mtStr(topic), data);
}


var server = "137.117.132.51";
var options = {
    client_id : getSerial(),
    port: 1883
};

var net = require("net");

function send() {
  var client = net.connect({
    host: server,
    port: options.port
  }, function() {
    console.log("Connected");
    client.write(mtpConnect(options.client_id));

    var gravity = mpu.getGravity();
    var message = {
      Gravity: {
        X: gravity[0],
        Y: gravity[1],
        Z: gravity[2]      } 
    };
    client.write(mtpPub("VesselOrientation",JSON.stringify(message)));

//    client.write(mtpPub("blah","{'angle_wind_relative':"+i+".4}"));

    client.on("end", function() {
      console.log("Client disconnected");
    });
  });
}

setInterval(function() {
  send();
}, 1000);

pinMode(19, "output", true);
pinMode(18, "output", true);
pinMode(23, "output", true);
pinMode(25, "output", true);

function setMotorAThrottle(throttle) {
  digitalWrite(19,1);
  analogWrite(18, throttle, { freq : 25000 });
}

function setMotorBThrottle(throttle) {
  digitalWrite(25,1);
  analogWrite(23, throttle, { freq : 25000 });
}

setMotorAThrottle(0);
setMotorBThrottle(0);


var http = require("http");
http.createServer(function (req, res) {
  var result = url.parse(req.url, true);
  var engine = parseInt(result.query["engine"]);
  var throttle = parseFloat(result.query["throttle"]);
  if( engine === 0 || engine == 2 ) setMotorAThrottle(throttle);
  if( engine == 1 || engine == 2 ) setMotorBThrottle(throttle);

  res.writeHead(200);
  res.end("Setting throttle to "+throttle+" on engine "+engine);
}).listen(8080);

