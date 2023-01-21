using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class Variable : INodeExpression
{
    private readonly SymbolVariable? _symbol;
    private readonly List<INodeExpression> _expressions;
    
    public string Name { get; }

    public Variable(string name)
    {
        Name = name;
        _expressions = new List<INodeExpression>();
    }
    
    public Variable(string name, List<INodeExpression> expressions)
    {
        Name = name;
        _expressions = expressions;
    }

    public Variable(SymbolVariable symbol)
    {
        _symbol = symbol;
        _expressions = new List<INodeExpression>();
        Name = symbol.Name;
    }
    
    public Variable(SymbolVariable symbol, List<INodeExpression> expressions)
    {
        _symbol = symbol;
        _expressions = expressions;
        Name = symbol.Name;
    }


    public SymbolType GetExpressionType()
    {
        if (_expressions.Count != 0)
        {
            var type = _symbol?.Type as SymbolArray ?? throw new CompilerException(_symbol?.Name + " isn't array");
            var itemsType = type.ItemsType;
            
            if (_expressions[0].GetExpressionType() is not SymbolInteger)
            {
                throw new CompilerException(_symbol.Name +  " array index can't be not integer");
            }
            
            for (var i = 1; i < _expressions.Count; i++)
            {
                if (itemsType is not SymbolArray arrayType)
                {
                    throw new CompilerException(_symbol.Name +  " array has " + i + " indexes, but " + _expressions.Count + " received");
                }

                if (_expressions[i].GetExpressionType() is not SymbolInteger)
                {
                    throw new CompilerException(_symbol.Name +  " array index can't be not integer");
                }

                itemsType = arrayType.ItemsType;
            }

            return itemsType;
        }

        if (_symbol == null) throw new CompilerException("can't get type for variable");
        return _symbol.Type;
    }

    public void Generate(Generator.Generator generator)
    {
        if (_symbol.Type is not SymbolDouble)
        {
            if (_symbol is SymbolVariableParameter parameter)
            {
                generator.AddRight(AssemblerCommand.Push, IndirectAssemblerRegisters.Ebp, 8 + parameter.Offset);
                return;
            }

            if (_symbol is SymbolVariableLocal local)
            {
                generator.AddLeft(AssemblerCommand.Push, IndirectAssemblerRegisters.Ebp, 4 + local.Offset);
                return;
            }

            generator.Add(AssemblerCommand.Push, AssemblerCommand.Dword, this);
        }
        else
        {
            if (_symbol is SymbolVariableParameter parameter)
            {
                generator.AddRight(AssemblerCommand.Push, IndirectAssemblerRegisters.Ebp, 8 + parameter.Offset);
                return;
            }

            if (_symbol is SymbolVariableLocal local)
            {
                generator.AddLeft(AssemblerCommand.Push, IndirectAssemblerRegisters.Ebp, 4 + local.Offset);
                return;
            }

            generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
            generator.Add($"movsd xmm0, qword [_{_symbol.Name}]");
            generator.Add($"movsd qword [esp], xmm0");
        }
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += Name;
        if (_expressions.Count != 0)
        {
            value += "[]";
            value += "\n";
            for (int i = 0; i < (level + 1) * 4; i++)
            {
                value += " ";
            }

            value += "indexes";
            value += "\n";
        }
        for (int i = 0; i < _expressions.Count; i++)
        {
            value += _expressions[i].GetPrint(level + 2);
            
            if (i != _expressions.Count - 1)
            {
                value += "\n";
            }
        }
        return value;
    }
}