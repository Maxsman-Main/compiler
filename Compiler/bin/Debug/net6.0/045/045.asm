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
_main:
push ecx
movsd xmm0, qword [doubleValue1]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm0, qword [doubleValue2]
sub esp, 8
movsd qword [esp], xmm0
movsd xmm1, qword [esp]
add esp, 8
movsd xmm0, qword [esp]
add esp, 8
comisd xmm0, xmm1
jae logic1
mov eax, 0
jmp endOfLogic1
logic1:
mov eax, 1
endOfLogic1:
push eax
push integer_format
call _printf
add esp, 8
pop ecx
section .data
doubleValue1: dq 1.1
doubleValue2: dq 5.2
