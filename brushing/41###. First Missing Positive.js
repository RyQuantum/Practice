// my solution, why only 55 points?
// function solution(A) {
//     // write your code in JavaScript (Node.js 8.9.4)
//     A = Array.from(new Set(A)).sort((a, b) => a - b)
//     for (let i = 0; i < A.length; i++) {
//         if (A[i] !== i + 1) return i + 1
//     }
//     return A.length + 1
// }

var firstMissingPositive = function(A) {
    const map = {}
    for (let i = 0; i < A.length; i++) {
        map[A[i]] = i
    }
    for (let i = 0; i < A.length; i++) {
        if (map[i + 1] === undefined) return i + 1
    }
    return A.length + 1
};
A = []
console.log(firstMissingPositive(A))