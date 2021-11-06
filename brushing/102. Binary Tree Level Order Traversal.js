var levelOrder = function(root) {
    const res = []
    if (!root) return res
    const queue = [root]
    while(queue.length > 0) {
        const level = []
        const len = queue.length
        for (let i = 0; i < len; i ++) {
            const cur = queue.shift()
            level.push(cur.val)
            if (cur.left) queue.push(cur.left)
            if (cur.right) queue.push(cur.right)
        }
        res.push(level)
    }
    return res
};

function TreeNode(val, left, right) {
    this.val = (val === undefined ? 0 : val)
    this.left = (left === undefined ? null : left)
    this.right = (right === undefined ? null : right)
}