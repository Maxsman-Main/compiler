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
_main:
push 5
push 10
pop ebx
pop eax
cdq
idiv ebx
push eax
cvtsi2sd xmm0, [esp]
sub esp, 4
movsd qword [esp], xmm0
movsd xmm0, qword [esp]
add esp, 8
movsd qword [_a], xmm0
push ecx
sub esp, 8
movsd xmm0, qword [_a]
movsd qword [esp], xmm0
push double_format
call _printf
add esp, 12
pop ecx
section .data
