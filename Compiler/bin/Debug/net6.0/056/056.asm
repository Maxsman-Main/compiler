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
char_format:   
db "%c", 10, 0
string_format: 
db "%s", 10, 0
double_minus_multiplier: 
dq -1.0
_main:
push stringValue1
pop dword [_a]
push ecx
push dword [_a]
call _printf
add esp, 4
pop ecx
section .data
stringValue1: db "Hello!", 10, 0
