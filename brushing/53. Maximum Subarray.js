var maxSubArray = function(nums) {
    let max = nums[0]
    let sum = 0
    nums.forEach(n => {
        if (sum > 0) {
            sum += n
        } else {
            sum = n
        }
        max = Math.max(sum, max)
    })
    return max
};

console.log(maxSubArray([-2,1,-3,4,-1,2,1,-5,4]))