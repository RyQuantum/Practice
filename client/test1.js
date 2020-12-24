const axios = require('axios');
const serial = 'TESTSERIAL';
const API_KEY = '9a9845167f5644d6a3fcf3386da97331';

axios.defaults.baseURL = 'https://chunks.memfault.com';

const data = Buffer.from([11, 22, 33]);
const headers = {
  'Content-Length': data.length,
  'Content-Type': 'application/octet-stream;',
  'Memfault-Project-Key': API_KEY,
};
console.log(headers);

const func = async () => {
  try {
    const res = await axios.post(`/api/v0/chunks/${serial}`, data, { headers });
    console.log(JSON.stringify(res.data));
  } catch (err) {
    console.log(err);
  }
};

func();
