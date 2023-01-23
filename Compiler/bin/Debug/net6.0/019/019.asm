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
push 2
push 2
pop ebx
pop eax
cmp eax, ebx
jg logic1
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
