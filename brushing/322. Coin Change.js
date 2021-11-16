var coinChange = function(coins, amount) {
    const arr = [0]
    for (let i = 1; i <= amount; i++) {
        const res = []
        coins.forEach((a) => {
            if (i - a >= 0) res.push(arr[i - a] + 1)
        })
        arr[i] = Math.min(...res)
    }
    return arr[amount] === Infinity ? - 1 : arr[amount]
};

coins = [2], amount = 3
console.log(coinChange(coins, amount))