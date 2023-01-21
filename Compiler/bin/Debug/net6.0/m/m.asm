global _main
extern _printf
extern _scanf
section .bss
_a resd 1
_b resd 1
section .text
integer_format:
db "%d", 10, 0
double_format: 
db "%f", 10, 0
char_format:   
db "%c", 10, 0
string_format: 
db "%s", 10, 0
_abc:
push ebp
mov ebp, esp
push ecx
push dword [ebp + 12]
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push dword [ebp + 8]
push integer_format
call _printf
add esp, 8
pop ecx
push 1
pop edx
mov esp, ebp
pop ebp
ret
_main:
push 10
pop dword [_b]
push ecx
push ecx
push 12
push dword [_b]
call _abc
add esp, 8
pop ecx
push edx
push integer_format
call _printf
add esp, 8
pop ecx
