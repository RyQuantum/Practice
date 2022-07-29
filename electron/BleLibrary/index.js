const noble = require('@abandonware/noble')

noble.startScanning(); // any service UUID, no duplicates


// noble.startScanning([], true); // any service UUID, allow duplicates


var serviceUUIDs = []; // default: [] => all
var allowDuplicates = true; // default: false

noble.on('discover', peripheral => {
    console.log(peripheral)
});
noble.startScanning(serviceUUIDs, allowDuplicates, (err) => console.log); // particular UUID's