global _main
extern _printf
extern _scanf
section .bss
_x resd 1
section .text
integer_format:
db "%d", 10, 0
double_format: 
db "%f", 10, 0
_abc:
push 4
push 2
pop ebx
pop eax
sub eax, ebx
push eax
push integer_format
call _printf
add esp, 8
ret
_main:
push 11
pop dword [_x]
push 3
push 7
pop ebx
pop eax
add eax, ebx
push eax
push dword [_x]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
call _abc
