const http = require('http');
http.createServer((req, res) => {
  console.log(req);
  res.end(JSON.stringify({}));
}).listen(80);
