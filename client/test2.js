const request = require('request-promise');

const serial = 'TESTSERIAL';
const API_KEY = '9a9845167f5644d6a3fcf3386da97331';
const data = Buffer.from([44, 55, 66]);

const func = async () => {
  try {

    const res = await request({
      method: 'POST',
      uri: `https://chunks.memfault.com/api/v0/chunks/${serial}`,
      headers: {
        'Memfault-Project-Key': API_KEY,
        'Content-Type': 'multipart/mixed',
      },
      multipart: [
        { body: data, 'Content-Length': data.length },
        { body: data, 'Content-Length': data.length },
      ],
    });

    console.log('res: ', res);
  } catch (err) {
    console.log('err:', err.message);
  }
};

func();
