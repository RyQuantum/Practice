fs = require('fs')
fs.readFile('crt test(1).txt', 'utf8', function (err,data) {
  if (err) {
    return console.log(err);
  }
  const buf = Buffer.from(data, 'ascii');
  //converting string into buffer
  // var hexvalue = buf.toString('hex');
  //with buffer, convert it into hex
  console.log(data.replace(/[\r\n]/g,"").split(' ').length)
  console.log(data.replace(/[\r\n]/g,"").split(' ').map(c => '0x' + c).join(','));
});