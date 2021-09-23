const ECKey = require('ec-key');

var serverPrivateKeyPem = `-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIP17ItsJhzB7MeAIAQq3EnldFRdNN+V4zjmyPlK+J3ZYoAoGCCqGSM49
AwEHoUQDQgAEtvkZATuuDUG41t+2Vj0HaQIComIin+8+xpDH40QV7lcD2JmBPX/S
nd+mQBJKntSqBfXYkWFmdWWSK7v9RJmIPg==
-----END EC PRIVATE KEY-----`;
var clientPublicKeyPem = `-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE6ptJWjwZzxbJ1RDIXHU8/vBt4Nhj
iRXkacxIuTQ4QlKBAhna2Ra+PmZgO6G4K6RwKILytlc9TL4gbHP4QT6IXA==
-----END PUBLIC KEY-----`;

var serverKey = new ECKey(serverPrivateKeyPem, 'pem');
var clientKey = new ECKey(clientPublicKeyPem, 'pem');

let secret = serverKey.computeSecret(clientKey);

secret = secret.slice(-8);
console.log(secret);

const crypto = require('crypto');

const md5 = crypto.createHash('md5');
const sha1 = crypto.createHash('sha1');
const sha256 = crypto.createHash('sha256');

md5.update(secret);
sha1.update(secret);
sha256.update(secret);

console.log('md5:', crypto.createHash('md5').update(secret).digest('hex'));
console.log('sha1:', sha1.digest('hex'));
console.log('sha256:', sha256.digest('hex'));