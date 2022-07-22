// my solution, why only 55 points?
// function solution(A) {
//     // write your code in JavaScript (Node.js 8.9.4)
//     A = Array.from(new Set(A)).sort((a, b) => a - b)
//     for (let i = 0; i < A.length; i++) {
//         if (A[i] !== i + 1) return i + 1
//     }
//     return A.length + 1
// }

var firstMissingPositive = function(list) {
    const map = {}
    for (let i = 0; i < list.length; i++) {
        map[list[i]] = i
    }
    for (let i = 0; i < list.length; i++) {
        if (map[i + 1] === undefined) return i + 1
    }
    return list.length + 1
};
const A = [1,2,0] //3
const B = [3,4,-1,1] //2
const C = [7,8,9,11,12] //1
const test = []
console.log(firstMissingPositive(test))