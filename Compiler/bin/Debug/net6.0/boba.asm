global _main
extern _printf
extern _scanf

section .bss
_a resd 1
_b resd 1

section .text

integer_format:
db "%d", 10, 0

double_format:
db "%f", 10, 0

_main:
    push 10
    pop dword [_b]
    
    push 0
    pop dword [_a]
    
    push dword [_b]
    pop ecx

    for1:
        cmp [_a], ecx
        je endOfFor1
        
        push dword [_a]
        push integer_format
        call _printf
        add esp, 8
        
        push dword [_b]
        push integer_format
        call _printf
        add esp, 8
        
        cmp [_a], ecx
        jl incFor1
        
        sub [_a], dword 1
        jmp for1
        
        incFor1:
           add [_a], dword 1
           jmp for1

    endOfFor1: