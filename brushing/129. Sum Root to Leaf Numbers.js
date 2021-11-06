var sumNumbers = function(root) {
    let sum = 0
    const arr = [root.val]
    const dfs = (cur) => {
        if (!cur.left && !cur.right) {
            sum += parseInt(arr.reduce((prev, cur) => prev.toString() + cur.toString()))
            return
        }
        if (cur.left) {
            arr.push(cur.left.val)
            dfs(cur.left)
            arr.pop()
        }
        if (cur.right) {
            arr.push(cur.right.val)
            dfs(cur.right)
            arr.pop()
        }
    }
    dfs(root)
    return sum
};

function TreeNode(val, left, right) {
    this.val = (val === undefined ? 0 : val)
    this.left = (left === undefined ? null : left)
    this.right = (right === undefined ? null : right)
}

const head = new TreeNode(1)
const left = new TreeNode(2)
const right = new TreeNode(3)
head.left = left
head.right = right
console.log(sumNumbers(head))