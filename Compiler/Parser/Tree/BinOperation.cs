using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Semantic;

namespace Compiler.Parser.Tree;

public class BinOperation : INodeExpression
{
    private readonly OperatorValue _operation;
    
    private SymbolType? _type;
    private INodeExpression _left;
    private INodeExpression _right;

    public BinOperation(OperatorValue operation, INodeExpression left, INodeExpression right)
    {
        _operation = operation;
        _left = left;
        _right = right;
    }
    
    public SymbolType GetExpressionType()
    {
        if (_type is not null)
        {
            return _type;
        }
        
        var leftType = _left.GetExpressionType();
        var rightType = _right.GetExpressionType();
        switch (leftType)
        {
            case SymbolInteger when rightType is SymbolInteger:
                _type = new SymbolInteger("Integer");
                break;
            case SymbolDouble when rightType is SymbolDouble:
                if (_operation is OperatorValue.Equal or OperatorValue.Less or OperatorValue.More or OperatorValue.LessEqual or OperatorValue.MoreEqual)
                {
                    _type = new SymbolInteger("Integer");
                }
                else
                {
                    _type = new SymbolDouble("Double");
                }
                break;
            case SymbolDouble when rightType is SymbolInteger:
                _right = new CastToDouble(_right);
                if (_operation is OperatorValue.Equal or OperatorValue.Less or OperatorValue.More or OperatorValue.LessEqual or OperatorValue.MoreEqual)
                {
                    _type = new SymbolInteger("Integer");
                }
                else
                {
                    _type = new SymbolDouble("Double");
                }
                break;
            case SymbolInteger when rightType is SymbolDouble:
                _left = new CastToDouble(_left);
                if (_operation is OperatorValue.Equal or OperatorValue.Less or OperatorValue.More or OperatorValue.LessEqual or OperatorValue.MoreEqual)
                {
                    _type = new SymbolInteger("Integer");
                }
                else
                {
                    _type = new SymbolDouble("Double");
                }
                break;
            case SymbolChar when rightType is SymbolChar:
                _type = new SymbolChar("Char");
                break;
            case SymbolString when rightType is SymbolString:
                if (_operation is not OperatorValue.Plus)
                {
                    throw new CompilerException("can't not sum strings");
                }
                _type = new SymbolString("String");
                break;
            default:
                throw new CompilerException("can't use bin operation for " + _left.GetExpressionType().Name + " and " +
                                            _right.GetExpressionType().Name);
        }
        return _type;
    }

    public void Generate(Generator.Generator generator)
    {
        _left.Generate(generator);
        _right.Generate(generator);
        if (_left.GetExpressionType() is not SymbolDouble)
        {
            generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Ebx);
            generator.Add(AssemblerCommand.Pop, AssemblerRegisters.Eax);
            switch (_operation)
            {
                case OperatorValue.Plus:
                    generator.Add(AssemblerCommand.Add, AssemblerRegisters.Eax, AssemblerRegisters.Ebx);
                    break;
                case OperatorValue.Minus:
                    generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Eax, AssemblerRegisters.Ebx);
                    break;
                case OperatorValue.Multiplication:
                    generator.Add(AssemblerCommand.IMul, AssemblerRegisters.Eax, AssemblerRegisters.Ebx);
                    break;
                case OperatorValue.Div:
                    generator.Add(AssemblerCommand.Cdq);
                    generator.Add(AssemblerCommand.IDiv, AssemblerRegisters.Ebx);
                    break;
                case OperatorValue.Equal:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Je);
                    break;
                case OperatorValue.Less:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jl);
                    break;
                case OperatorValue.More:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jg);
                    break;
                case OperatorValue.LessEqual:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jle);
                    break;
                case OperatorValue.MoreEqual:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jge);
                    break;
                case OperatorValue.And:
                    generator.LogicCounter += 1;
                    generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Je, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Ebx, 0);
                    generator.Add(AssemblerCommand.Je, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
                    generator.Add($"endOfLogic{generator.LogicCounter}:");
                    break;
                case OperatorValue.Or:
                    generator.LogicCounter += 1;
                    generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
                    generator.Add(AssemblerCommand.Jne, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Ebx, 0);
                    generator.Add(AssemblerCommand.Je, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
                    generator.Add($"endOfLogic{generator.LogicCounter}:");
                    break;
            }

            generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
        }
        else
        {
            generator.Add("movsd xmm1, qword [esp]");
            generator.Add(AssemblerCommand.Add, AssemblerRegisters.Esp, 8);
            generator.Add("movsd xmm0, qword [esp]");
            generator.Add(AssemblerCommand.Add, AssemblerRegisters.Esp, 8);
            switch (_operation)
            {
                case OperatorValue.Plus:
                    generator.Add(AssemblerCommand.Addsd, AssemblerRegisters.Xmm0, AssemblerRegisters.Xmm1);
                    generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
                    generator.Add("movsd qword [esp], xmm0");
                    break;
                case OperatorValue.Minus:
                    generator.Add(AssemblerCommand.Subsd, AssemblerRegisters.Xmm0, AssemblerRegisters.Xmm1);
                    generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
                    generator.Add("movsd qword [esp], xmm0");
                    break;
                case OperatorValue.Multiplication:
                    generator.Add(AssemblerCommand.Mulsd, AssemblerRegisters.Xmm0, AssemblerRegisters.Xmm1);
                    generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
                    generator.Add("movsd qword [esp], xmm0");
                    break;
                case OperatorValue.Div:
                    generator.Add(AssemblerCommand.Divsd, AssemblerRegisters.Xmm0, AssemblerRegisters.Xmm1);
                    generator.Add(AssemblerCommand.Sub, AssemblerRegisters.Esp, 8);
                    generator.Add("movsd qword [esp], xmm0");
                    break;
                case OperatorValue.Equal:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Je);
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
                case OperatorValue.Less:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jb);
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
                case OperatorValue.More:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Ja);
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
                case OperatorValue.LessEqual:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jbe);
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
                case OperatorValue.MoreEqual:
                    GenerateLogicOperatorCode(generator, AssemblerCommand.Jae);
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
                case OperatorValue.And:
                    generator.LogicCounter += 1;
                    generator.Add(AssemblerCommand.Comisd, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Je, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Ebx, 0);
                    generator.Add(AssemblerCommand.Je, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
                    generator.Add($"endOfLogic{generator.LogicCounter}:");
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
                case OperatorValue.Or:
                    generator.LogicCounter += 1;
                    generator.Add(AssemblerCommand.Comisd, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
                    generator.Add(AssemblerCommand.Jne, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 0);
                    generator.Add(AssemblerCommand.Cmp, AssemblerRegisters.Ebx, 0);
                    generator.Add(AssemblerCommand.Je, $"endOfLogic{generator.LogicCounter}");
                    generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
                    generator.Add($"endOfLogic{generator.LogicCounter}:");
                    generator.Add(AssemblerCommand.Push, AssemblerRegisters.Eax);
                    break;
            }
        }
    }

    public string GetPrint(int level)
    {
        var value = "";
        for (int i = 0; i < level * 4; i++)
        {
            value += " ";
        }

        value += OperatorConstants.OperatorSymbols[_operation];
        value += "\n";
        value += _left.GetPrint(level + 1);
        value += "\n";
        value += _right.GetPrint(level + 1);
        return value;
    }

    private void GenerateLogicOperatorCode(Generator.Generator generator, AssemblerCommand jump)
    {
        var command = AssemblerCommand.Cmp;
        var leftRegister = AssemblerRegisters.Eax;
        var rightRegister = AssemblerRegisters.Ebx;
        if (_left.GetExpressionType() is SymbolDouble || _right.GetExpressionType() is SymbolDouble)
        {
            command = AssemblerCommand.Comisd;
            leftRegister = AssemblerRegisters.Xmm0;
            rightRegister = AssemblerRegisters.Xmm1;
        }
        generator.LogicCounter += 1;
        generator.Add(command,leftRegister, rightRegister);
        generator.Add(jump, $"logic{generator.LogicCounter}");
        generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 0);
        generator.Add(AssemblerCommand.Jmp, $"endOfLogic{generator.LogicCounter}");
        generator.Add($"logic{generator.LogicCounter}:");
        generator.Add(AssemblerCommand.Mov, AssemblerRegisters.Eax, 1);
        generator.Add($"endOfLogic{generator.LogicCounter}:");
    }
}