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
db '%c', 10, 0
string_format:
db '%s', 10, 0
_main:
push 10
pop dword [_b]
push ecx
push '!'
push 'B'
pop eax
pop ebx
add eax, ebx
push eax
push char_format
call _printf
add esp, 4
pop ecx	 