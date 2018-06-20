var wifi = require("Wifi");
wifi.scan(function(ap_list) {
  ap_list.forEach(function(ap) {
    console.log(ap.ssid +" - "+ap.authMode+" - "+ap.rssi);
  });

});


wifi.connect("Covfefe", {password:""}, function(ap) {
  console.log("Connected : "+ap);
});  

/*

Not on ESP32

wifi.setIP({
  ip:"192.168.10.210",
  netmask:"255.255.255.0",
  gw:"192.168.10.1"
});
*/