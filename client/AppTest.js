const buf1 = Buffer.from(parseHexString('482102a6020203010a676f616b732d6677096531'));
const buf2 = Buffer.from(parseHexString('80122e302e30066344565404a201000500875e'));


const arr = [buf1, buf2]
const path = RNFS.DocumentDirectoryPath + '/test.bin';

const boundary = Math.random().toString(36).slice(-10);
const ending = `--${boundary}--\r\n`;
const data = arr.map(buf => `--${boundary}\r\nContent-Length: ${buf.length}\r\n\r\n${buf.reduce((acc, cur) => acc + String.fromCharCode(cur), '')}\r\n`).reduce((acc, cur) => acc + cur, '') + ending;
console.log('data:', data);
const r = await RNFS.writeFile(path, data, 'ascii');
console.log('FILE WRITTEN!');

try {
  const res = await RNFetchBlob.fetch('POST', 'http://192.168.2.247', {
    'Content-Type': `multipart/mixed; boundary=${boundary}`,
    'Memfault-Project-Key': '9a9845167f5644d6a3fcf3386da97331',
  }, RNFetchBlob.wrap(path))

  console.log('upload res:', res.text())
} catch (err) {
  console.log('err:', err);
}
