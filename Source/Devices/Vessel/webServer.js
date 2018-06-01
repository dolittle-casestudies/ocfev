var http = require("http");
http.createServer(function (req, res) {
  var result = url.parse(req.url, true);
  var engine = result.query["engine"];
  var throttle = result.query["throttle"];
  res.writeHead(200);
  res.end("Setting throttle to "+throttle+" on engine "+engine);
}).listen(8080);
