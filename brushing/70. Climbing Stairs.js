var climbStairs = function(n) {
    if (n == 1) return 1
    if (n == 2) return 2
    const arr = [1, 2]
    for (let i = 3; i <= n; i++) {
        arr.push(arr[i - 2] + arr[i - 3])
    }
    return arr[n - 1]
};

console.log(climbStairs(45))