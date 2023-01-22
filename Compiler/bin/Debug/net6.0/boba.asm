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
movsd qword [ebp - 8], xmm0
sub esp, 8
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
movsd xmm0, qword [ebp + 12]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
mov esp, ebp
pop ebp
ret
_main:
movsd xmm0, qword [doubleValue2]
sub esp, 8
movsd qword [esp], xmm0
call _abc
add esp, 4
push ecx
movsd xmm0, qword [doubleValue3]
sub esp, 8
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
section .data
doubleValue1: dq 0.5
doubleValue2: dq 0.5
doubleValue3: dq 1.5
