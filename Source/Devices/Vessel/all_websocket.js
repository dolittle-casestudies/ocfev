var host = '192.168.10.148';
var WebSocket = require('ws');
var ws = null;
var connected = false;

function connectWebSocket() {
  if( ws !== null ) delete ws;
  try {
    ws = new WebSocket(host, {
      path: '/vessel/orientation',
      port: 5000,
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

    ws.on('message', function (msg) {
      if( msg != 'pong' ) {
        var methodCall = JSON.parse(msg);
        if( methodCall.method == "throttleChanged" ) {
          var engine = methodCall.arguments[0];
          var target = methodCall.arguments[1];
          console.log(`Throttle Changed for engine ${engine} to value ${target}`);
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
}

onInit();