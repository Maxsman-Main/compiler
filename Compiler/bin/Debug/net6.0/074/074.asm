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
while1:
push dword [_i]
push 10
pop ebx
pop eax
cmp eax, ebx
jl logic1
mov eax, 0
jmp endOfLogic1
logic1:
mov eax, 1
endOfLogic1:
push eax
pop eax
cmp eax, 0
je whileEnd1
push ecx
push dword [_i]
push integer_format
call _printf
add esp, 8
pop ecx
push dword [_i]
push 1
pop ebx
pop eax
add eax, ebx
push eax
pop dword [_i]
jmp while1
whileEnd1:
section .data
