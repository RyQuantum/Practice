var getKthFromEnd = function(head, k) {
    const arr = [head]
    let cur = head
    while (cur.next) {
        arr.push(cur.next)
        cur = cur.next
    }
    return arr[arr.length - k]
};