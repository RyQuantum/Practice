const lengthOfLongestSubstring = function (s) {
  let i = 0, j = 0
  if (s.length < 2) return s.length

  let max = 0
  const map = {}
  while (j < s.length) {
    if (map[s[j]] === undefined) {
      map[s[j]] = j
    } else {
      max = Math.max(max, j - i)
      // for (let k = i; k <= map[s[j]]; k++) {
      //
      //   map[s[i]] = undefined
      //   i++
      // }
      while (i <= map[s[j]]) {
        map[s[i]] = undefined
        i++
      }
      map[s[j]] = j
    }
    j++
    if (j === s.length) {
      max = Math.max(max, j - i)
    }
  }
  return max
}

let test = [
  ['aab', 2],
  ['abcabcbb', 3],
  ['bbbbb', 1],
  ['pwwkew', 3],
  ['abcbadbb123', 4],
  ['wcibxubumenmeyatdrmydiajxloghiqfmzhlvihjouvsuyoypayulyeimuotehzriicfskpggkbbipzzrzucxamludfyk', 12],
];

test.forEach(t => {
  if (lengthOfLongestSubstring(t[0]) !== t[1]) {
    console.log(t[0], lengthOfLongestSubstring(t[0]));
  }
})