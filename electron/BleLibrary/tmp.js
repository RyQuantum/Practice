var noble = require('noble');
noble.on('discover', res => {
    console.log(res.address)
});
noble.startScanning();