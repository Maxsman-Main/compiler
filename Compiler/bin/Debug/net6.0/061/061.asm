global _main
extern _printf
extern _scanf
section .bss
section .text
integer_format:
db "%d", 10, 0
double_format: 
db "%f", 10, 0
char_format:   
db "%c", 10, 0
string_format: 
db "%s", 10, 0
double_minus_multiplier: 
dq -1.0
_abc:
push ebp
mov ebp, esp
push ecx
push dword [ebp + 8]
call _printf
add esp, 4
pop ecx
mov esp, ebp
pop ebp
ret
_main:
push stringValue1
call _abc
add esp, 4
push ecx
push 1
push integer_format
call _printf
add esp, 8
pop ecx
push stringValue2
call _abc
add esp, 4
section .data
stringValue1: db "hello", 10, 0
stringValue2: db "hello", 10, 0
