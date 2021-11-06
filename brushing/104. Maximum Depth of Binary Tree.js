var maxDepth = function(root) {
    if (!root) return 0
    let maxLevel = 1
    const dfs = (cur, level) => {
        if (!cur.left && !cur.right) {
            maxLevel = Math.max(maxLevel, level)
            return level
        }
        if (cur.left) {
            dfs(cur.left, level + 1)
        }
        if (cur.right) {
            dfs(cur.right, level + 1)
        }
    }
    dfs(root, 1)
    return maxLevel
};