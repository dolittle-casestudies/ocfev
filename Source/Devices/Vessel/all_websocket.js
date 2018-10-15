var host = '104.40.203.222';
var port = 8080;
var WebSocket = require('ws');
var ws = null;
var connected = false;

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


function connectWebSocket() {
  if( ws !== null ) {
    delete ws;
    ws = null;
  }

  try {
    ws = new WebSocket(host, {
      path: '/vessel/orientation',
      port: port,
      protocol: 'none',
      protocolVersion: 13,
      origin: 'Espruino',
      keepAlive: 60,
      headers: {}
    });

    ws.on('open', function () {
      console.log('Connected to server');
      connected = true;
    });

    ws.on('error', function(msg) {
      console.log(`Error - ${msg}`);
    });

    ws.on('message', function (msg) {
      if( msg != 'pong' ) {
        var methodCall = JSON.parse(msg);
        if( methodCall.method == "throttleChanged" ) {
          var engine = methodCall.arguments[0];
          var throttle = methodCall.arguments[1];

          console.log(`Throttle Changed for engine ${engine} to value ${throttle}`);
          setMotorThrottle(engine, throttle);
        }
      }
    });
  } catch(ex) {
    connected = false;
    console.log('Error trying to connect - retrying in 5 seconds');
    setTimeout(function() {
      connectWebSocket();
    },5000);
  }
}


function onInit() {
  I2C1.setup({ scl: 22, sda: 21 }); //, bitrate: 100000});
  const mpu = require("MPU6050").connect(I2C1);

  connectWebSocket();

  setInterval(function () {
    if( !connected ) return;

    var gravity = mpu.getGravity();
    var payload = {
      method: 'changeGravity',
      arguments: [
        gravity[0],
        -gravity[1],
        gravity[2]
      ]
    };
    try {
      ws.send(JSON.stringify(payload));
    } catch(ex) {
      console.log('Failed trying to send - reconnect WebSocket in 5 seconds');
      connected = false;
      setTimeout(function() {
        connectWebSocket();
      }, 5000);
    }
  }, 1000);

  // Initialize Motor pins
  pinMode(19, "output", true);
  pinMode(18, "output", true);
  pinMode(23, "output", true);
  pinMode(25, "output", true);

  stopMotor(0);
  stopMotor(1);
}