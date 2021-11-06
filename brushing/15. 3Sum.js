var threeSum = function(nums) {
    const res = []
    nums.sort((a, b) => a - b)
    for (let i = 0; i < nums.length; i++) {
        if (nums[i] > 0) break
        if (i > 0 && nums[i] === nums[i - 1]) continue
        let j = i + 1, k = nums.length - 1
        const target = 0 - nums[i]
        while (j < k) {
            const sum = nums[j] + nums[k]
            if (sum === target) {
                res.push([-target, nums[j], nums[k]])
                while (j < k && nums[j] === nums[j + 1]) j++
                while (j < k && nums[k] === nums[k - 1]) k--
                j++;
                k--;
            }
            else if (sum < target) j++
            else k--
        }
    }
    return res
};

nums = [1,-1,-1,0]
console.log(threeSum(nums))