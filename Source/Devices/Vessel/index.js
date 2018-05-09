var trust = 0.75;
var hz = 100;


digitalWrite(18,1);
digitalWrite(5,1);

pinMode(23, "output", true);
analogWrite(23, trust,{ freq : hz });

pinMode(19, "output", true);
analogWrite(19, trust,{ freq : hz });


I2C1.setup({scl:22,sda:21, bitrate: 100000});
const mpu = require("MPU6050").connect(I2C1);
//const DMP = require("MPU6050_DMP").create(MPU, 3);

setInterval(function() {
  console.log(mpu.getAcceleration());
  console.log(mpu.getGravity());
  console.log(mpu.getRotation());
  console.log(mpu.getDegreesPerSecond());
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


var triggerTime = 0.0;
var triggered = false;

var trigger = function() {

  triggerTime = getTime();
  digitalPulse(D2, 0, 0.002);
  digitalPulse(D2, 1, 0.01);

  triggered = true;
}

/*
setWatch(function(e) {
  triggerTime = getTime();
  triggered = true;
}, D0, {
  repeat: true,
  edge: "rising"
});
*/

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

/*
setInterval(function() {
  trigger();
}, 60);*/