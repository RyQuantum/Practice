var hasCycle = function(head) {
    let cur = head.next
    const arr = [head]
    while (cur != null) {
        if ((arr.filter(elm => elm === cur)).length > 0) return true
        arr.push(cur)
        cur = cur.next
    }
    return false
};

function ListNode(val) {
    this.val = val;
    this.next = null;
}
const a = new ListNode(3)
const b = new ListNode(2)
const c = new ListNode(0)
const d = new ListNode(-4)
a.next = b
b.next = c
c.next = d
// d.next = b

console.log(hasCycle(a))