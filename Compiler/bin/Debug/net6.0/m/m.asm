global _main
extern _printf
extern _scanf
section .bss
_a resq 1
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
movsd xmm0, qword [doubleValue1]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm0, qword [esp]
add esp, 8
movsd qword [ebp - 24], xmm0
sub esp, 24
movsd xmm0, qword [doubleValue2]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm0, qword [esp]
add esp, 8
movsd qword [ebp - 8], xmm0
sub esp, 0
movsd xmm0, qword [doubleValue3]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm0, qword [esp]
add esp, 8
movsd qword [ebp - 16], xmm0
sub esp, 0
push ecx
sub esp, 8
movsd xmm0, qword [ebp - 8]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
push ecx
sub esp, 8
movsd xmm0, qword [ebp - 16]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
push ecx
sub esp, 8
movsd xmm0, qword [ebp - 24]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
push ecx
sub esp, 8
movsd xmm0, qword [ebp + 8]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
push ecx
sub esp, 8
movsd xmm0, qword [ebp + 16]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
movsd xmm0, qword [doubleValue4]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm2, qword [esp]
add esp, 8
mov esp, ebp
pop ebp
ret
_qwe:
push ebp
mov ebp, esp
push 3
pop dword [ebp - 12]
sub esp, 12
push 1
pop dword [ebp - 4]
sub esp, 0
push 2
pop dword [ebp - 8]
sub esp, 0
push ecx
push dword [ebp - 4]
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push dword [ebp - 8]
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push dword [ebp - 12]
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push dword [ebp + 8]
push integer_format
call _printf
add esp, 8
pop ecx
push 6
pop edx
mov esp, ebp
pop ebp
ret
_main:
push ecx
push ecx
movsd xmm0, qword [doubleValue5]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm0, qword [doubleValue6]
sub esp, 8
movsd qword [esp], xmm0
call _abc
add esp, 8
pop ecx
sub esp, 8
movsd qword [esp], xmm2
push double_format
call _printf
add esp, 12
pop ecx
push ecx
push ecx
push 4
call _qwe
add esp, 4
pop ecx
push edx
push integer_format
call _printf
add esp, 8
pop ecx
section .data
doubleValue1: dq 0.7
doubleValue2: dq 0.5
doubleValue3: dq 0.6
doubleValue4: dq 1.2341
doubleValue5: dq 0.9
doubleValue6: dq 0.1
