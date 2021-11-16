const isNeeded = (s) => {
    for (let i = 0; i <= s.length - i - 1; i++) {
        if (s[i] !== s[s.length - i - 1]) return false
    }
    return true
}

var longestPalindrome = function(s) {
    let string = ''
    for (let i = 0; i < s.length; i++) {
        for (let j = i; j < s.length; j++) {
            const subString = s.slice(i, j + 1)
            const res = isNeeded(subString)
            if (res) string = subString.length > string.length ? subString : string
        }
    }
    return string
};

console.log(longestPalindrome('babad'))