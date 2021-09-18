const ECKey = require('ec-key');

var clientPrivateKeyPem = `-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIOqjfZrP8Q7tmVqfi1+Ko802MR6yrJGgKTr9JPyYImQyoAoGCCqGSM49
AwEHoUQDQgAE45uC4bJ8GTLkhYP4wE9kTHFgHFP0It1P88Qy7z3cDiowjNc+Spt8
ofRXeu5CMogSw1Ib8P5MO3Aij+cLiS+HSg==
-----END EC PRIVATE KEY-----`;
var serverPublicKeyPem = `-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEtvkZATuuDUG41t+2Vj0HaQIComIi
n+8+xpDH40QV7lcD2JmBPX/Snd+mQBJKntSqBfXYkWFmdWWSK7v9RJmIPg==
-----END PUBLIC KEY-----`;

var serverKey = new ECKey(serverPublicKeyPem, 'pem');
var clientKey = new ECKey(clientPrivateKeyPem, 'pem');

var secret = clientKey.computeSecret(serverKey);

console.log(secret);