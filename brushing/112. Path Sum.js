var hasPathSum = function(root, targetSum) {
    if (!root) return false
    if (!root.left && !root.right) return root.val === targetSum
    let a = false, b = false
    if (root.left) {
        a = hasPathSum(root.left, targetSum - root.val)
    }
    if (root.right) {
        b = hasPathSum(root.right, targetSum - root.val)
    }
    return a || b
};

function TreeNode(val, left, right) {
    this.val = (val === undefined ? 0 : val)
    this.left = (left === undefined ? null : left)
    this.right = (right === undefined ? null : right)
}

const root = new TreeNode(1, new TreeNode(2, null, null), null)

console.log(hasPathSum(root, 2))