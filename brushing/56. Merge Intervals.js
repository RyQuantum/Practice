var merge = function (intervals) {
    intervals.sort((a, b) => a[0] - b[0])
    const res = []
    if (intervals.length === 1) return intervals
    res.push(intervals[0])
    for (let i = 1; i < intervals.length; i++) {
        if (res[res.length - 1][1] >= intervals[i][0]) {
            res[res.length - 1][1] = Math.max(intervals[i][1], res[res.length - 1][1])
        } else {
            res.push(intervals[i].slice())
        }
    }
    return res
};

arr = [[1,4],[2,3]]
console.log(merge(arr))