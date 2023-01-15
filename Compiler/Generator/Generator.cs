using Compiler.Constants;
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

    public void Add(AssemblerCommand command, string someString)
    {
        var assemblerCommand =
            $"{GeneratorConstants.Commands[command]} {someString}";
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

    public void Add(string command)
    {
        Commands.Add(command);
    }

    public void AddOutputFormats()
    {
        Add("integer_format:\ndb \"%d\", 10, 0");
        Add("double_format: \ndb \"%f\", 10, 0");
    }

    private void Initialize()
    {
        Commands = new List<string>
        {
            "global _main",
            "extern _printf",
            "extern _scanf",
            "section .text",
            "_main:"
        };
    }
}