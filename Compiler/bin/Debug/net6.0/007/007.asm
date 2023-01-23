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
_main:
push ecx
push 1
push 1
pop ebx
pop eax
sub eax, ebx
push eax
push integer_format
call _printf
add esp, 8
pop ecx
section .data
