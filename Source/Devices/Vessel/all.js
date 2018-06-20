// dolittle-iot.westeurope.cloudapp.azure.com
const wifi = require("Wifi");
const mpu = require("MPU6050").connect(I2C1);
const net = require("net");
const http = require("http");

function onInit() {
  var ip = wifi.getIP();
  
  I2C1.setup({scl:22,sda:21}); //, bitrate: 100000});

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

  var server = "192.168.1.100";
      //"137.117.132.51";

  var options = {
      client_id : getSerial(),
      port: 1883
  };

  function send(topic, message) {
    var client = net.connect({
      host: server,
      port: options.port
    }, function() {
      client.write(mtpConnect(options.client_id));
      console.log("Send to '"+topic+"' message: '"+message+"'");
      client.write(mtpPub(topic,message));

      client.on("end", function() {
  //      console.log("Client disconnected");
      });
    });
  }

  setInterval(function() {
    var gravity = mpu.getGravity();
    var message = {
      Gravity: {
        X: gravity[0],
        Y: gravity[1],
        Z: gravity[2]
      } 
    };
//    console.log(message.Gravity.X+", "+message.Gravity.Y+", "+message.Gravity.Z);    
//    console.log(message.Gravity.Y);
    send("VesselOrientation", JSON.stringify(message));
  }, 1000);

  pinMode(19, "output", true);
  pinMode(18, "output", true);
  pinMode(23, "output", true);
  pinMode(25, "output", true);


  var motorAMin = 0.555;
  var motorBMin = 0.555;
  var motorRelationship = (motorAMin/motorBMin);
  var motorBMax = 0.7;
  var motorAMax = motorBMax * motorRelationship;

  var motorDirectionPins = [19, 25];
  var motorThrottlePins = [18, 23];

  var motorMinValues = [motorAMin, motorBMin];
  var motorMaxValues = [motorAMax, motorBMax];

  var prevMotorThrottles = [0,0];

  function sendToMotor(motor, value) {
    motor = motor%2;
    digitalWrite(motorDirectionPins[motor],1);
    analogWrite(motorThrottlePins[motor], value, { freq : 25000 });
  }


  function stopMotor(motor) {
    sendToMotor(motor,0);
    prevMotorThrottles[motor] = 0;
  }

  function setMotorThrottle(motor, throttle) {
    motor = motor%2;
    if( throttle > 1 ) throttle = 1;
    if( throttle < 0 ) throttle = 0;

    if( throttle === 0 ) {
      stopMotor(motor);
      return;
    }

    function calculateAndSetActualThrottle() {


      var min = motorMinValues[motor];
      var max = motorMaxValues[motor];

      var delta = max - min;
      var actualThrottle = (throttle * delta) + min;

      console.log("Setting engine "+motor+" to "+actualThrottle);

      prevMotorThrottles[motor] = actualThrottle;

      sendToMotor(motor,actualThrottle);
    }

    if( prevMotorThrottles[motor] === 0 ) {
      sendToMotor(motor, 0.65);
      setTimeout(calculateAndSetActualThrottle, 200);

    } else { 
      calculateAndSetActualThrottle();
    }
  }

  stopMotor(0);
  stopMotor(1);

  send("vesselip",ip.ip);

  http.createServer(function (req, res) {
    var result = url.parse(req.url, true);

    if( result.query.hasOwnProperty("stop") ) {
      stopMotor(0);
      stopMotor(1);

      res.writeHead(200);
      res.end("Stopping engines");

    } else {
      var engine = parseInt(result.query["engine"]);
      var throttle = parseFloat(result.query["throttle"]);

      if( engine == 2 ) {
        setMotorThrottle(0, throttle);
        setMotorThrottle(1, throttle);
      } else {
        setMotorThrottle(engine, throttle);
      }

      res.writeHead(200);
      res.end("Setting throttle to "+throttle+" on engine "+engine);
    }
  }).listen(8080);
}