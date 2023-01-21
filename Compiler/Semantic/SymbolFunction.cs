using Compiler.Constants;
using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolFunction : SymbolProcedure
{
    public SymbolType ReturnType { get; }
    public INodeExpression? ReturnValue { get; }

    public SymbolFunction(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body, SymbolType returnType) : base(name, parameters, locals, body)
    {
        ReturnType = returnType;
        ReturnValue = null;
    }
    
    public SymbolFunction(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body, SymbolType returnType, INodeExpression returnValue) : base(name, parameters, locals, body)
    {
        ReturnType = returnType;
        ReturnValue = returnValue;
    }

    public override void Generate(Generator.Generator generator)
    {
        for (var i = Parameters.Data.Count - 1; i >= 0; i--)
        {
            var parameterVariable = (SymbolVariableParameter) Parameters.Data[i]!;
            parameterVariable.Generate(Parameters);
        }

        foreach (var local in Locals.Data.Values)
        {
            var localVariable = (SymbolVariableLocal) local;
            localVariable.Generate(Locals);
        }
        
        generator.Add(this);
        generator.AddProLog();
        Body.Generate(generator);
        ReturnValue?.Generate(generator);
        generator.AddPopInRegister(AssemblerRegisters.Edx);
        generator.AddIterLog();
        generator.Add(AssemblerCommand.Ret);
        generator.EspHead = 0;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        result += Name;
        result += "\n";
        result += Parameters.GetPrint(level + 1);
        result += Locals.GetPrint(level + 1);
        result += Body.GetPrint(level + 1);
        result += "\n";
        result += ReturnType.GetPrint(level + 1);
        return result;
    }
}