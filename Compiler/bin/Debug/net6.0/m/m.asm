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
_abc:
push ebp
mov ebp, esp
push 6
pop dword [ebp - 4]
sub esp, 4
push 1
pop dword [ebp - 8]
sub esp, 8
push 1
pop dword [ebp - 16]
sub esp, 12
push 2
pop dword [ebp - 12]
sub esp, 4
push dword [ebp - 8]
push dword [ebp - 4]
pop ebx
pop eax
add eax, ebx
push eax
push dword [ebp - 16]
pop ebx
pop eax
add eax, ebx
push eax
push dword [ebp - 12]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
mov esp, ebp
pop ebp
ret 8
_bca:
push ebp
mov ebp, esp
push 6
pop dword [ebp - 4]
sub esp, 4
push 1
pop dword [ebp - 8]
sub esp, 8
push 1
pop dword [ebp - 16]
sub esp, 12
push 2
pop dword [ebp - 12]
sub esp, 4
push dword [ebp - 8]
push dword [ebp - 4]
pop ebx
pop eax
add eax, ebx
push eax
push dword [ebp - 16]
pop ebx
pop eax
add eax, ebx
push eax
push dword [ebp - 12]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
push 1
pop ecx
mov esp, ebp
pop ebp
ret 8
_o:
push ebp
mov ebp, esp
push 6
pop dword [ebp - 4]
sub esp, 4
push 1
pop dword [ebp - 8]
sub esp, 8
push 1
pop dword [ebp - 16]
sub esp, 12
push 2
pop dword [ebp - 12]
sub esp, 4
push dword [ebp - 8]
push dword [ebp - 4]
pop ebx
pop eax
add eax, ebx
push eax
push dword [ebp - 16]
pop ebx
pop eax
add eax, ebx
push eax
push dword [ebp - 12]
pop ebx
pop eax
add eax, ebx
push eax
push integer_format
call _printf
add esp, 8
mov esp, ebp
pop ebp
ret 8
_main:
push 5
push 10
call _abc
add esp, 8
push 5
push 10
call _bca
push ecx
push integer_format
call _printf
add esp, 8
push 5
push 10
call _o
add esp, 8
