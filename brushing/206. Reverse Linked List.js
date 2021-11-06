var reverseList = function(head) {
    let cur = head, pre = null
    while (true) {
        if (cur == null) return pre
        const tmp = cur.next
        cur.next = pre
        pre = cur
        cur = tmp
    }
};