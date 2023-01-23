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
_abc:
push ebp
mov ebp, esp
push 1
pop dword [ebp - 8]
sub esp, 8
push 2
pop dword [ebp - 4]
sub esp, 0
push ecx
push dword [ebp + 12]
push dword [ebp - 8]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push dword [ebp + 8]
push dword [ebp - 4]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push dword [_x]
push integer_format
call _printf
add esp, 8
pop ecx
mov esp, ebp
pop ebp
ret
_main:
push 5
pop dword [_x]
push 5
push 6
call _abc
add esp, 8
push ecx
push 1
push integer_format
call _printf
add esp, 8
pop ecx
push 7
push 8
call _abc
add esp, 8
section .data
