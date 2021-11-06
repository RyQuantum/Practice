var maxProfit = function(prices) {
    let min = prices[0]
    let profit = Number.MIN_VALUE
    for (let i = 1; i < prices.length; i++) {
        min = Math.min(min, prices[i])
        profit = Math.max(profit, prices[i] - min)
    }
    return profit
};

console.log(maxProfit([7,1,5,3,6,4]))