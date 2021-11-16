var mergeTwoLists = function(l1, l2) {
    let cur1 = l1, cur2 = l2
    let cur = new ListNode(0)
    const pre = cur
    while (cur1 && cur2) {
        if (cur1.val < cur2.val) {
            cur.next = cur1
            cur = cur.next
            cur1 = cur1.next
        } else {
            cur.next = cur2
            cur = cur.next
            cur2 = cur2.next
        }
    }
    if (!cur1) cur.next = cur2
    if (!cur2) cur.next = cur1
    return pre.next
};