using Compiler.Constants;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class ArrayAssignment : INodeStatement
{
    private SymbolVariable _variable;
    private List<INodeExpression> _indexes;
    private INodeExpression _assigmentValue;

    public ArrayAssignment(SymbolVariable variable, List<INodeExpression> indexes, INodeExpression assigmentValue)
    {
        _variable = variable;
        _indexes = indexes;
        _assigmentValue = assigmentValue;
    }
    
    public string GetPrint(int level)
    {
        return "";
    }

    public void Generate(Generator.Generator generator)
    {
        _assigmentValue.Generate(generator);
        _indexes[0].Generate(generator);
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Eax);
        generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Ebx);
        generator.Add($"mov _{_variable.Name}[eax], ebx");
    }
}