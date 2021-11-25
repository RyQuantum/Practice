file = open("crt test(1).txt","r")
str = file.read()
arr = str.replace('\n', '').split(' ')
# print(list(map(lambda s: '0x' + s, arr)))

file = open("tmp.txt","r")
str = file.read()
print(str.replace('\'', ''))
