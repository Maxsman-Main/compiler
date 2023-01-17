using Compiler.Generator;

namespace Compiler.Constants;

public enum AssemblerCommand
{
    Add,
    Push,
    Sub,
    Mov,
    Pop,
    Call,
    DD,
    Dword,
    Ret,
    Resd
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
    Esp,
    Ebp
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
        {AssemblerCommand.Call, "call"},
        {AssemblerCommand.DD, "dd"},
        {AssemblerCommand.Dword, "dword"},
        {AssemblerCommand.Ret, "ret"},
        {AssemblerCommand.Resd, "resd"}
    };

    public static Dictionary<AssemblerRegisters, string> Registers { get; } = new()
    {
        {AssemblerRegisters.Eax, "eax"},
        {AssemblerRegisters.Ebx, "ebx"},
        {AssemblerRegisters.Esp, "esp"}
    };
    
    public static Dictionary<IndirectAssemblerRegisters, string> IndirectRegisters { get; } = new()
    {
        {IndirectAssemblerRegisters.Eax, "eax"},
        {IndirectAssemblerRegisters.Ebx, "ebx]"},
        {IndirectAssemblerRegisters.Esp, "esp"},
        {IndirectAssemblerRegisters.Ebp, "ebp"}
    };

    public static Dictionary<Format, string> Formats { get; } = new()
    {
        {Format.Integer, "integer_format"},
        {Format.Double, "double_format"}
    };
}