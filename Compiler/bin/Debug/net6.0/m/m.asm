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
_main:
push 10
pop dword [_b]
push 1
pop dword [_a]
push dword [_b]
pop ecx
for1:
cmp [_a], ecx
jg endOfFor1
push ecx
push dword [_a]
push integer_format
call _printf
add esp, 8
pop ecx
add [_a], dword 1
jmp for1
endOfFor1:
