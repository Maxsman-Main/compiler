global _main
extern _printf
extern _scanf
section .bss
_i resd 1
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
push 0
pop dword [_i]
push 10
pop eax
imul eax, -1
push eax
pop ecx
for1:
cmp [_i], ecx
jg endOfFor1
push ecx
push dword [_i]
push integer_format
call _printf
add esp, 8
pop ecx
add [_i], dword 1
jmp for1
endOfFor1:
push ecx
push dword [_i]
push integer_format
call _printf
add esp, 8
pop ecx
section .data
