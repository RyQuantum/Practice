var permute = function(nums) {
    const used = {}
    const res = []
    const dfs = (path) => {
        if (path.length === nums.length) {
            return res.push(path.slice())
        }
        nums.forEach(num => {
            if (used[num]) return
            used[num] = true
            path.push(num)
            dfs(path)
            used[num] = false
            path.pop()
        })
    }
    dfs([])
    return res
};

console.log(permute([1,2,3]))