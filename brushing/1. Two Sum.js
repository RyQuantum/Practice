var twoSum = function(nums, target) {
    const map = {}
    for (let i = 0; i < nums.length; i++) {
        const res = target - nums[i]
        if (map[res] !== undefined) return [map[res], i]
        map[nums[i]] = i
    }
};

nums = [2,7,11,15], target = 9
console.log(twoSum(nums, target))