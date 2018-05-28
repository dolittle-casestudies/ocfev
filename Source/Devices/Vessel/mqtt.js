// http://www.espruino.com/MQTT#line=35,39,52,53
// http://forum.espruino.com/conversations/298824/
// https://github.com/rockymanobi/espruino-mqtt-sample/blob/master/espruino_mqtt.js
// http://docs.oasis-open.org/mqtt/mqtt/v3.1.1/os/mqtt-v3.1.1-os.html#_Toc385349228

function mtStr(s) {
    return String.fromCharCode(s.length >> 8, s.length & 255) + s;
}
function mtPacket(cmd, variable, payload) {
    return String.fromCharCode(cmd, variable.length + payload.length) + variable + payload;
}
function mtpConnect(name) {
    return mtPacket(0b00010000,
        mtStr("MQTT")/*protocol name*/ +
        "\x04"/*protocol level*/ +
        "\x00"/*connect flag*/ +
        "\xFF\xFF"/*Keepalive*/, mtStr(name));
}
function mtpPub(topic, data) {
    return mtPacket(0b00110001, mtStr(topic), data);
}

var server = "137.117.132.51";
var options = {
    client_id: getSerial(),
    port: 1883
};

var i = 0;
var net = require("net");

function send() {
    var client = net.connect({
        host: server,
        port: options.port
    }, function () {
        console.log("Connected");
        client.write(mtpConnect(options.client_id));

        console.log("Sending : " + i);
        client.write(mtpPub("blah", "{'angle_wind_relative':" + i + ".4}"));
        i++;

        client.on("end", function () {
            console.log("Client disconnected");
        });
    });
}



setInterval(function () {
    send();
}, 1000);

