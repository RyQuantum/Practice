var inorderTraversal = function(root) {
    const arr = []
    if (!root) return arr
    const dfs = (cur) => {
        if (cur.left) dfs(cur.left)
        arr.push(cur.val)
        if (cur.right) dfs(cur.right)
    }
    dfs(root)
    return arr
};