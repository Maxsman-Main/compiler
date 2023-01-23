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
_abc:
push ebp
mov ebp, esp
push dword [ebp + 8]
push 10
pop ebx
pop eax
imul eax, ebx
push eax
pop dword [ebp - 4]
sub esp, 4
push ecx
push dword [ebp - 4]
push 1
pop ebx
pop eax
sub eax, ebx
push eax
push integer_format
call _printf
add esp, 8
pop ecx
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
push ecx
push ecx
push 5
call _abc
add esp, 4
pop ecx
push edx
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
push 7
call _abc
add esp, 4
pop ecx
push edx
push integer_format
call _printf
add esp, 8
pop ecx
section .data
