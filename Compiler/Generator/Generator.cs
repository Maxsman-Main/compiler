using Compiler.Constants;
using Compiler.Parser.Tree;
using Compiler.Semantic;

namespace Compiler.Generator;

public class Generator
{
    public List<string> Commands { get; private set; }

    public Generator()
    {
        Commands = new List<string>();
        Initialize();
    }

    public void Add(AssemblerCommand command, AssemblerRegisters register)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[register]}";
        Commands.Add(assemblerCommand);
    }

    public void Add(SymbolVariable variable, AssemblerCommand command)
    {
        var assemblerCommand =
            $"_{variable.Name} {GeneratorConstants.Commands[command]} 1";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command1, AssemblerCommand command2, Variable variable)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command1]} {GeneratorConstants.Commands[command2]} [_{variable.Name}]";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command, string someString)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {someString}";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command, Variable variable)
    {
        if (command is AssemblerCommand.Pop)
        {
            Commands.Add($"pop dword [_{variable.Name}]");
            return;
        }
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{variable.Name}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, Format format)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Formats[format]}";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command, IndirectAssemblerRegisters register)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[register]}";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command, SymbolVariable variable)
    {
        if (command is AssemblerCommand.Pop)
        {
            Commands.Add($"{GeneratorConstants.Commands[command]} dword [_{variable.Name}]");
            return;
        }
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{variable.Name}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, SymbolProcedure procedure)
    {
        var name = procedure.Name;
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{name}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, int number)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {number}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, double number)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {number}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, AssemblerRegisters register, int number)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[register]}, {number}";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command, AssemblerRegisters firstRegister, AssemblerRegisters secondRegister)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[firstRegister]}, {GeneratorConstants.Registers[secondRegister]}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, SymbolVariable left, AssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{left.Name}, {GeneratorConstants.Registers[right]}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, AssemblerRegisters left, SymbolVariable right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[left]}, _{right}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, SymbolVariable left, SymbolVariable right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{left.Name}, _{right.Name}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, IndirectAssemblerRegisters left, AssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[left]}, {GeneratorConstants.Registers[right]}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, AssemblerRegisters left, IndirectAssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.Registers[left]}, {GeneratorConstants.IndirectRegisters[right]}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, IndirectAssemblerRegisters left, IndirectAssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[left]}, {GeneratorConstants.IndirectRegisters[right]}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, IndirectAssemblerRegisters left, SymbolVariable right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {GeneratorConstants.IndirectRegisters[left]}, _{right.Name}";
        Commands.Add(assemblerCommand);
    }
    
    public void Add(AssemblerCommand command, SymbolVariable left, IndirectAssemblerRegisters right)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} _{left.Name}, {GeneratorConstants.IndirectRegisters[right]}";
        Commands.Add(assemblerCommand);
    }

    public void Add(AssemblerCommand command)
    {
        var assemblerCommand = $"{GeneratorConstants.Commands[command]}";
        Commands.Add(assemblerCommand);
    }

    public void AddProLog()
    {
        Commands.Add("push ebp");
        Commands.Add("mov ebp, esp");
    }

    public void AddIterLog()
    {
        Commands.Add("mov esp, ebp");
        Commands.Add("push ebp");
    }

    public void Add(SymbolProcedure procedure)
    {
        var assemblerCommand = $"_{procedure.Name}:"; 
        Commands.Add(assemblerCommand);
    }

    public void Add(string command)
    {
        Commands.Add(command);
    }

    private void Initialize()
    {
        Add("global _main");
        Add("extern _printf");
        Add("extern _scanf");
    }

    public void AddSectionBss()
    {
        Add("section .bss");
    }

    public void AddSectionText()
    {
        Add("section .text");
        Add("integer_format:\ndb \"%d\", 10, 0");
        Add("double_format: \ndb \"%f\", 10, 0");
    }

    public void AddMain()
    {
        Add("_main:");
    }
}