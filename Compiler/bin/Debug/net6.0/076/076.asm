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
push 1
pop eax
imul eax, -1
push eax
pop dword [_i]
push dword [_i]
push 0
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
pop eax
cmp eax, 0
je elseOfIf1
push ecx
push dword [_i]
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push stringValue1
call _printf
add esp, 4
pop ecx
jmp endOfIf1
elseOfIf1:
push ecx
push dword [_i]
push integer_format
call _printf
add esp, 8
pop ecx
push ecx
push stringValue2
call _printf
add esp, 4
pop ecx
endOfIf1:
section .data
stringValue1: db "yes", 10, 0
stringValue2: db "no", 10, 0
