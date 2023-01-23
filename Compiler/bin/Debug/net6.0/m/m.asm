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
double_minus_multiplier: 
dq -1.0
_main:
push 10
pop dword [_a]
push ecx
push dword [_a]
push integer_format
call _printf
add esp, 8
pop ecx
push 123
pop dword [_b]
push ecx
push dword [_a]
push integer_format
call _printf
add esp, 8
pop ecx
section .data
