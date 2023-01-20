using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class WriteStatement : INodeStatement
{
    private readonly List<INodeExpression>? _parameters;
    
    public WriteStatement(List<INodeExpression>? parameters)
    {
        _parameters = parameters;
    }
    
    public void Generate(Generator.Generator generator)
    {
        if (_parameters is null) return;
        generator.Add(AssemblerCommand.Push, AssemblerRegisters.Ecx);
        foreach (var parameter in _parameters)
        {
            if (parameter.GetExpressionType() is SymbolInteger)
            {
                parameter.Generate(generator);
                generator.Add(AssemblerCommand.Push, Format.Integer);
                generator.Add(AssemblerCommand.Call, "_printf");
                generator.Add(AssemblerCommand.Add, AssemblerRegisters.Esp, 8);
            }

            if (parameter.GetExpressionType() is SymbolDouble)
            {
                parameter.Generate(generator);
                generator.Add(AssemblerCommand.Push, Format.Double);
                generator.Add(AssemblerCommand.Call, "_printf");
                generator.Add(AssemblerCommand.Add, AssemblerRegisters.Esp, 8);
            }
        }
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Ecx);
    }
    
    public string GetPrint(int level)
    {
        return "";
    }
}