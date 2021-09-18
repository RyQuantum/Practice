const argon2 = require('argon2');
const func = async () => {
    try {
        const hash = await argon2.hash("password");
        console.log('hash:', hash);
        const isCorrect = await argon2.verify(hash, "password");
        console.log('isCorrect:', isCorrect);
      } catch (err) {
        console.log("error:", err);
      }
}

func();