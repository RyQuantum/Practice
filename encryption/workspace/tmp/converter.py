s = '''B2 12 5C 62 4E 53 BD EC BB 5F 10 83 90 6D D8 48
     19 37 42 02 B4 CA 4D 03 5A 34 16 FC DA EF A6 5C
     94 49 84 F3 F8 B2 D6 7E 6E AC B5 FF 6F E3 D1 45
     4B C8 F7 4A B3 EA 39 43 63 8C E4 94 31 77 55 48
     77 DC 7A E5 7D FC 99 6E F1 3E 8D 89 C4 65 59 F3
     7D CC 56 7B B7 97 E5 17 BD 8D C3 0F EC 33 4C 60
     25 FC 6E F6 AD CB B7 35 B6 DA 45 11 4A AE 17 9B
     80 29 F3 EA 15 41 C3 92 31 AD AA 23 BA 7A 58 89'''
# s = s.replace("     ", " ")
res = bytearray.fromhex(s)
# print(s)
f = open("sample.txt", "wb")
f.write(res)
f.close()

print(s.replace(' ', '').replace('\n', ''))