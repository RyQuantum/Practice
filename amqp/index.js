const amqplib = require('amqplib');
// const querystring = require("querystring");
// const URI = require("uri-js");

// .connect("amqps://" + encodeURIComponent(username) + ":" + encodeURIComponent(password) + "@" + hostname + "?heartbeat=" + heartbeat, opts);
const username = '24b9rucZxRMcoar7iyjCEWGF'
const password = '69bHCUXuFYkxcwDemgD9NnsextyhCeMFYZ81PBjS'
// const username = 'ver=1&auth_mode=accessKey&sign_method=sha256&access_key=24b9rucZxRMcoar7iyjCEWGF&timestamp=1654058906409';
// const password = '0f6e058b99dfa1d23de9b3b76dd9409e8bc1faf47ad68c3ba73d8c20555aa46b';
const hostname = 'iot-amqp.quectelcn.com:5671/quec-open'
// const url = "amqps://" + encodeURIComponent(username) + ":" + encodeURIComponent(password) + "@" + hostname + "?heartbeat=" + 60;
const url = `amqps://${username}:${password}@${hostname}`;
// console.log(url);
(async () => {
  const queue = 'a.000000007550.test_queue';
//   const opts = {
//     ca: [fs.readFileSync('cacert.pem')]
//   };
//   const conn = await amqplib.connect(url, opts);
  const conn = await amqplib.connect(url);

  const ch1 = await conn.createChannel();
  await ch1.assertQueue(queue);

  // Listener
  ch1.consume(queue, (msg) => {
    if (msg !== null) {
      console.log('Recieved:', msg.content.toString());
      ch1.ack(msg);
    } else {
      console.log('Consumer cancelled by server');
    }
  });

  // Sender
  const ch2 = await conn.createChannel();

  setInterval(() => {
    ch2.sendToQueue(queue, Buffer.from('something to do'));
  }, 1000);
})();

// amqp.connect('amqp://iot-amqp.quectelcn.com:5671/quec-open').then(function (conn) {
//     process.once('SIGINT', function () { conn.close(); });
//     return conn.createChannel().then(function (ch) {

//         var ok = ch.assertQueue('hello', { durable: false });

//         ok = ok.then(function (_qok) {
//             return ch.consume('hello', function (msg) {
//                 console.log(" [x] Received '%s'", msg.content.toString());
//             }, { noAck: true });
//         });

//         return ok.then(function (_consumeOk) {
//             console.log(' [*] Waiting for messages. To exit press CTRL+C');
//         });
//     });
// }).catch(console.warn);
// var amqp = require('amqplib/callback_api');

// const url = 'amqp://24b9rucZxRMcoar7iyjCEWGF:69bHCUXuFYkxcwDemgD9NnsextyhCeMFYZ81PBjS@iot-amqp.quectelcn.com:5671/quec-open';
// // const url = 'amqp://iot-amqp.quectelcn.com:5671/quec-open';
// function start() {
//     amqp.connect(url, function (err, conn) {
//     // amqp.connect(url + "?heartbeat=60", function (err, conn) {
//         if (err) {
//             console.error("[AMQP]", err.message);
//             return setTimeout(start, 1000);
//         }
//         conn.on("error", function (err) {
//             if (err.message !== "Connection closing") {
//                 console.error("[AMQP] conn error", err.message);
//             }
//         });
//         conn.on("close", function () {
//             console.error("[AMQP] reconnecting");
//             return setTimeout(start, 1000);
//         });
//         console.log("[AMQP] connected");
//         amqpConn = conn;
//         whenConnected();
//     });
// }

// start();

// amqp.connect('amqp://24b9rucZxRMcoar7iyjCEWGF:69bHCUXuFYkxcwDemgD9NnsextyhCeMFYZ81PBjS@iot-amqp.quectelcn.com:5671/quec-open', function (error0, connection) {
//     if (error0) {
//         throw error0;
//     }
//     connection.createChannel(function (error1, channel) {
//         if (error1) {
//             throw error1;
//         }

//         var queue = 'hello';

//         channel.assertQueue(queue, {
//             durable: false
//         });

//         console.log(" [*] Waiting for messages in %s. To exit press CTRL+C", queue);

//         channel.consume(queue, function (msg) {
//             console.log(" [x] Received %s", msg.content.toString());
//         }, {
//             noAck: true
//         });
//     });
// });

// var amqp = require('amqp');

// var connection = amqp.createConnection({ url: 'amqp://24b9rucZxRMcoar7iyjCEWGF:69bHCUXuFYkxcwDemgD9NnsextyhCeMFYZ81PBjS@iot-amqp.quectelcn.com:5671/quec-open' });
// connection.on('ready', function () {
//     console.log(123)
//     var callbackCalled = false;
//     exchange = connection.exchange('exchange_name', { type: 'direct', autoDelete: false });
//     connection.queue("queue_name", { autoDelete: false }, function (queue) {
//         queue.bind('exchange_name', 'queue_name', function () {
//             exchange.publish('queue_name', 'this is message is testing ......');
//             callbackCalled = true;

//             setTimeout(function () {
//                 console.log("Single queue bind callback succeeded");
//                 //exchange.destroy();
//                 //queue.destroy();
//                 connection.end();
//                 connection.destroy();
//             }, 5000);

//         });

//         queue.subscribe(function (message) {
//             console.log('At 5 second recieved message is:' + message.data);
//         });

//     });
// });