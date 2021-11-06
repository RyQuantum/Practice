const addStrings = (num1, num2) => {
    let i = num1.length - 1;
    let j = num2.length - 1;
    let res = ''
    let mod = 0

    for (; i >= 0 || j >= 0; i--, j--) {
        const a = parseInt(num1[i]) || 0
        const b = parseInt(num2[j]) || 0
        const sum = a + b + mod
        res = sum % 10 + res
        mod = (sum - sum % 10) / 10
    }
    if (mod !== 0) res = mod + res
    return res
}

console.log(addStrings('456', '77'))