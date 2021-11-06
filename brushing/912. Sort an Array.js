var sortArray = function(nums) {
    if (nums.length <= 1) return nums
    const index = Math.floor(nums.length / 2)
    const pivot = nums.splice(index, 1)[0]
    const left = [], right = []
    for (let i = 0; i < nums.length; i++) {
        if (nums[i] <= pivot) left.push(nums[i])
        if (nums[i] > pivot) right.push(nums[i])
    }
    return [...sortArray(left), pivot, ...sortArray(right)]
};

nums = [5,2,3,1]
console.log(sortArray(nums))