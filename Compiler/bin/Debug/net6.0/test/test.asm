global _main
extern _printf
extern _scanf
section .text
_main:
push 100
push 22
pop ebx
pop eax
sub eax, ebx
push eax
push integer_format
call _printf
add esp, 8
push 100
push 22
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
push 5
push integer_format
call _printf
add esp, 8
integer_format:
db "%d", 10, 0
double_format: 
db "%f", 10, 0
