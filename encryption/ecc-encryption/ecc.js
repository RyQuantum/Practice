const ECKey = require('ec-key');

var serverPrivateKeyPem = `-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIP17ItsJhzB7MeAIAQq3EnldFRdNN+V4zjmyPlK+J3ZYoAoGCCqGSM49
AwEHoUQDQgAEtvkZATuuDUG41t+2Vj0HaQIComIin+8+xpDH40QV7lcD2JmBPX/S
nd+mQBJKntSqBfXYkWFmdWWSK7v9RJmIPg==
-----END EC PRIVATE KEY-----`;
var clientPublicKeyPem = `-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE45uC4bJ8GTLkhYP4wE9kTHFgHFP0
It1P88Qy7z3cDiowjNc+Spt8ofRXeu5CMogSw1Ib8P5MO3Aij+cLiS+HSg==
-----END PUBLIC KEY-----`;

var serverKey = new ECKey(serverPrivateKeyPem, 'pem');
var clientKey = new ECKey(clientPublicKeyPem, 'pem');

var secret = serverKey.computeSecret(clientKey);

console.log(secret);