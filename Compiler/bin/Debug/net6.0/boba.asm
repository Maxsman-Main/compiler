global _main
extern _printf
extern _scanf
section .bss
_a resd 1
section .text
integer_format:
db "%d", 10, 0
double_format:
db "%f", 10, 0
_main:
push 3
pop dword [_a]
push dword [_a]
push 3
pop ebx
pop eax
mov eax, eax eq ebx
push eax
push integer_format
call _printf
add esp, 8
