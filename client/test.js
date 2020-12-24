const axios = require('axios');
const FormData = require('form-data');

const serial = 'TESTSERIAL';
const API_KEY = '9a9845167f5644d6a3fcf3386da97331';
axios.defaults.baseURL = 'https://chunks.memfault.com';

const form = new FormData();
const data = Buffer.from([11, 22, 33]);
form.append('chunk', data, { header: { 'Content-Length': data.length } });
form.append('chunk', data, { header: { 'Content-Length': data.length } });

const func = async () => {
  try {
    const res = await axios.post(`/api/v0/chunks/${serial}`, form, {
      headers: {
        'Memfault-Project-Key': API_KEY,
        'Content-Type': `multipart/mixed; boundary=${form.getBoundary()}`,
        'Content-Length': form.getLengthSync(),
      },
    });

    console.log(JSON.stringify(res.data));
  } catch (err) {
    console.log(err.message);
  }
};

func();
