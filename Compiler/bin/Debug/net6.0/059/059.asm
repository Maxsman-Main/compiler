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
push stringValue1
call _printf
add esp, 4
pop ecx
mov esp, ebp
pop ebp
ret
_main:
call _abc
add esp, 0
push ecx
push 1
push integer_format
call _printf
add esp, 8
pop ecx
call _abc
add esp, 0
section .data
stringValue1: db "abc", 10, 0
