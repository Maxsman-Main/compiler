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
char_format:   
db "%c", 10, 0
string_format: 
db "%s", 10, 0
double_minus_multiplier: 
dq -1.0
_inc:
push ebp
mov ebp, esp
push dword [ebp + 8]
push 1
pop ebx
pop eax
add eax, ebx
push eax
pop edx
mov esp, ebp
pop ebp
ret
_main:
push 1
pop dword [_x]
push ecx
push 2
push ecx
push dword [_x]
call _inc
add esp, 4
pop ecx
push edx
pop ebx
pop eax
imul eax, ebx
push eax
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push 7
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push ecx
push dword [_x]
call _inc
add esp, 4
pop ecx
push edx
push 2
pop ebx
pop eax
cdq
idiv ebx
push eax
push integer_format
call _printf
add esp, 8
pop ecx
section .data
