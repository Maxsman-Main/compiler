using Compiler.Generator;

namespace Compiler.Constants;

public enum AssemblerCommand
{
    Add,
    Push,
    Sub,
    Mov,
    Pop,
    Call
}

public enum AssemblerRegisters
{
    Eax,
    Ebx,
    Esp
}

public enum IndirectAssemblerRegisters
{
    Eax,
    Ebx,
    Esp
}

public enum Format
{
    Integer,
    Double
}

public static class GeneratorConstants
{
    public static Dictionary<AssemblerCommand, string> Commands { get; } = new()
    {
        {AssemblerCommand.Add, "add"},
        {AssemblerCommand.Push, "push"},
        {AssemblerCommand.Sub, "sub"},
        {AssemblerCommand.Mov, "mov"},
        {AssemblerCommand.Pop, "pop"},
        {AssemblerCommand.Call, "call"}
    };

    public static Dictionary<AssemblerRegisters, string> Registers { get; } = new()
    {
        {AssemblerRegisters.Eax, "eax"},
        {AssemblerRegisters.Ebx, "ebx"},
        {AssemblerRegisters.Esp, "esp"}
    };
    
    public static Dictionary<IndirectAssemblerRegisters, string> IndirectRegisters { get; } = new()
    {
        {IndirectAssemblerRegisters.Eax, "[eax]"},
        {IndirectAssemblerRegisters.Ebx, "[ebx]"},
        {IndirectAssemblerRegisters.Esp, "[esp]"}
    };

    public static Dictionary<Format, string> Formats { get; } = new()
    {
        {Format.Integer, "integer_format"},
        {Format.Double, "double_format"}
    };
}