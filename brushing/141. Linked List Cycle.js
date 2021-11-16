var hasCycle = function(head) {
    if (!head) return false
    let cur = head.next
    const set = new Set()
    set.add(head)
    while (cur != null) {
        if (set.has(cur)) return true
        set.add(cur)
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