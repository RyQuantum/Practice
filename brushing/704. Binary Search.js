// var search = function(nums, target) {
//     return nums.indexOf(target)
// };

const search = (nums, target) => {
    if (nums.length === 0) return -1
    if (nums.length === 1) {
        if (nums[0] !== target) return -1
        else return 0
    }
    while(true) {
        const index = Math.floor(nums.length / 2)
        let i = -1
        if (nums[index] > target) {
            i = search(nums.slice(0, index), target)
            if (i !== -1) return i
            else return -1
        } else if (nums[index] < target) {
            i = search(nums.slice(index + 1), target)
            if (i !== -1) return index + i + 1
            else return -1
        } else {
            return index
        }
    }
}

nums = [-1,0,3,5,9,12], target = 13
console.log(search(nums, target))