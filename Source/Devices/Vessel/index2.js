// https://engineer2you.blogspot.no/2018/01/brushless-motor-l298-arduino.html

/*
var motorPin1 = 5; // Engine A Power
var motorPin2 = 23;
var motorPin3 = 18;
var motorPin4 = 19;

var delayMs = 5000;

function stopAllEngines()
{
  analogWrite(motorPin1, 0);
  analogWrite(motorPin2, 0);
  analogWrite(motorPin3, 0);
  analogWrite(motorPin4, 0);
}


function doNothing(){
  // For a good time call Peetri
}

pinMode(motorPin1, "output", true);
pinMode(motorPin2, "output", true);
pinMode(motorPin3, "output", true);
pinMode(motorPin4, "output", true);
*/

// motor A ccw
/*
analogWrite(motorPin1, 0);
analogWrite(motorPin2, 180);
analogWrite(motorPin3, 0);
analogWrite(motorPin4, 180);
*/
//


// motor B cw
/*
console.log("Engine B clockwise");
analogWrite(motorPin1, 0);
analogWrite(motorPin2, 180);
analogWrite(motorPin3, 180);
analogWrite(motorPin4, 0);

setTimeout(doNothing, delayMs);
stopAllEngines();
*/

//*/

// motor A cw

console.log("Engine A clockwise");
analogWrite(motorPin3, 180);
//analogWrite(motorPin2, 0);
/*
analogWrite(motorPin3, 180);
analogWrite(motorPin4, 0);
*/

//*/

//setTimeout(doNothing, delayMs);
//stopAllEngines();





/*
var trust = 1.0;
var hz = 1000;

// Direction
digitalWrite(18,1);
digitalWrite(25,1);

pinMode(23, "output", true);
analogWrite(23, 1);

pinMode(19, "output", true);
analogWrite(19, 1);
*/
/*
var trust = 0.5;
var hz = 100;

// Direction
digitalWrite(18,1);
digitalWrite(25,1);

pinMode(23, "output", true);
analogWrite(23, trust,{ freq : hz });

pinMode(19, "output", true);
analogWrite(19, trust,{ freq : hz });
*/



I2C1.setup({scl:22,sda:21, bitrate: 100000});
const mpu = require("MPU6050").connect(I2C1);
//const DMP = require("MPU6050_DMP").create(MPU, 3);

setInterval(function() {
  console.log("Accelleration: "+mpu.getAcceleration());
  console.log("Gravity: "+mpu.getGravity());
  console.log("Rotation: "+mpu.getRotation());
  console.log("Degrees Per Second: "+mpu.getDegreesPerSecond());
}, 1000);



/*
var sensor = require("HC-SR04").connect(D2,D0,function(dist) {
  console.log(dist+" cm away");
});
setInterval(function() {
  sensor.trigger();
}, 100);
*/

//digitalWrite(D2, 0);

//digitalWrite(D2, 1);



/*
var triggerTime = 0.0;
var triggered = false;

var trigger = function() {

  triggerTime = getTime();
  digitalPulse(D2, 0, 0.002);
  digitalPulse(D2, 1, 0.01);

  triggered = true;
};


setWatch(function(e) {
  console.log("Yo");
  triggerTime = getTime();
  triggered = true;
}, D0, {
  repeat: true,
  edge: "rising"
});

setWatch(function(e) {
  if( triggered ) {
    var duration = e.time-triggerTime;
    var distance = (duration/2)/29.1;
    console.log(((duration*1000000)*0.034)/2);
    //console.log(distance);
  }
  triggered = false;
}, D0, {
  repeat: true,
  edge: "falling"
});

setInterval(function() {
  trigger();
  console.log("Trigger");
}, 1000);

*/




