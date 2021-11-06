var isValid = function(s) {
    const map = {
        ')': '(',
        '}': '{',
        ']': '[',
    }
    const arr = []
    for (let i = 0; i < s.length; i++) {
        if (arr.length === 0) arr.push(s[i])
        else {
            if (arr[arr.length - 1] === map[s[i]]) arr.pop()
                else arr.push(s[i])
        }
    }
    return arr.length === 0
};

console.log(isValid("()[]{}"))