var compareVersion = function(version1, version2) {
    const a = version1.split('.').map(n => parseInt(n))
    const b = version2.split('.').map(n => parseInt(n))
    for (let i = 0, j = 0; i < a.length || j < b.length; i++, j++) {
        const x = a[i] || 0
        const y = b[j] || 0
        if (x > y) return 1
        if (x < y) return -1
    }
    return 0
};

version1 = "7.5.2.4", version2 = "7.5.3"
console.log(compareVersion(version1, version2))