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
push ebp
mov ebp, esp
push 4
push dword [ebp + 8]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
mov esp, ebp
push ebp
ret
_main:
push 12
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
push 7
call _abc
add esp, 4
