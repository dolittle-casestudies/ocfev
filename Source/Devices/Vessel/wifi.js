var wifi = require("Wifi");
wifi.scan(function(ap_list) {
  ap_list.forEach(function(ap) {
    console.log(ap.ssid +" - "+ap.authMode+" - "+ap.rssi);
  });

});


wifi.connect("OCFEV", {password:""}, function(ap) {
  console.log("Connected : "+ap);
});  
