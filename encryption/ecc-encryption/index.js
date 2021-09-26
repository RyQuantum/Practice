const ECKey = require('ec-key');
const crypto = require('crypto');
const fs = require('fs');
const net = require('net');

const PIPE_NAME = "ecc_pipe";
const PIPE_PATH = "\\\\.\\pipe\\" + PIPE_NAME;

const serverPrivateKeyPem = fs.readFileSync('./openssl/server.key', { encoding:'utf8', flag:'r' });
const clientPublicKeyPem = fs.readFileSync('./tmp/client_pub.key', { encoding:'utf8', flag:'r' });

var serverKey = new ECKey(serverPrivateKeyPem, 'pem');
var clientKey = new ECKey(clientPublicKeyPem, 'pem');

let secret = serverKey.computeSecret(clientKey);

secret = secret.slice(-8);
console.log('secret:', secret);

const md5 = crypto.createHash('md5');
md5.update(secret);

const client = net.createConnection(PIPE_PATH, () => {
    //'connect' listener
    console.log('connected to server!');
    client.write('Secret: ' + secret.toString('hex').toUpperCase()); 
    client.write(' AES: ' + md5.digest('hex').toUpperCase()); 
    client.end();
});