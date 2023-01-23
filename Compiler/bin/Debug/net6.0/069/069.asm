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
movsd xmm0, qword [doubleValue1]
sub esp, 8
movsd qword [esp], xmm0
sub esp, 8
movsd xmm0, qword [ebp + 8]
movsd qword [esp], xmm0
movsd xmm1, qword [esp]
add esp, 8
movsd xmm0, qword [esp]
add esp, 8
addsd xmm0, xmm1
sub esp, 8
movsd qword [esp], xmm0
movsd xmm2, qword [esp]
add esp, 8
mov esp, ebp
pop ebp
ret
_main:
push ecx
push ecx
movsd xmm0, qword [doubleValue2]
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
section .data
doubleValue1: dq 1.1
doubleValue2: dq 5.1
