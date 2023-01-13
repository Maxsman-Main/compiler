﻿using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Generator;

public class Generator
{
    private readonly List<string> _commands;

    public Generator()
    {
        _commands = new List<string>();
    }

    public void Add(AssemblerCommand command, AssemblerRegisters register)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[register]}";
    }

    public void Add(AssemblerCommand command, IndirectAssemblerRegisters register)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[register]}";
    }

    public void Add(AssemblerCommand command, SymbolVariable variable)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{variable.Name}";
    }
    
    public void Add(AssemblerCommand command, int number)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} dword ptr {number}";
    }
    
    public void Add(AssemblerCommand command, AssemblerRegisters register, int number)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[register]}, {number}";
    }

    public void Add(AssemblerCommand command, AssemblerRegisters firstRegister, AssemblerRegisters secondRegister)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[firstRegister]}, {GeneratorConstants.Registers[secondRegister]}";
    }
    
    public void Add(AssemblerCommand command, SymbolVariable left, AssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{left.Name}, {GeneratorConstants.Registers[right]}";
    }
    
    public void Add(AssemblerCommand command, AssemblerRegisters left, SymbolVariable right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[left]}, _{right}";
    }
    
    public void Add(AssemblerCommand command, SymbolVariable left, SymbolVariable right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{left.Name}, _{right.Name}";
    }
    
    public void Add(AssemblerCommand command, IndirectAssemblerRegisters left, AssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[left]}, {GeneratorConstants.Registers[right]}";
    }
    
    public void Add(AssemblerCommand command, AssemblerRegisters left, IndirectAssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[left]}, {GeneratorConstants.IndirectRegisters[right]}";
    }
    
    public void Add(AssemblerCommand command, IndirectAssemblerRegisters left, IndirectAssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[left]}, {GeneratorConstants.IndirectRegisters[right]}";
    }
    
    public void Add(AssemblerCommand command, IndirectAssemblerRegisters left, SymbolVariable right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[left]}, _{right.Name}";
    }
    
    public void Add(AssemblerCommand command, SymbolVariable left, IndirectAssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{left.Name}, {GeneratorConstants.IndirectRegisters[right]}";
    }
}