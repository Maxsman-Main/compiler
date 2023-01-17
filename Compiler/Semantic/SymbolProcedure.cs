using System.Collections.Specialized;
using Compiler.Constants;
using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolProcedure : Symbol
{
    protected readonly SymbolTable Locals;
    protected readonly INodeStatement Body;

    public SymbolTable Parameters { get; }

    public SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body) : base(name)
    {
        Parameters = parameters;
        Locals = locals;
        Body = body;
        
    }

    public override void Generate(Generator.Generator generator)
    {
        foreach (var parameter in Parameters.Data.Values)
        {
            var parameterVariable = (SymbolVariableParameter) parameter;
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
        generator.AddIterLog();
        generator.Add(AssemblerCommand.Ret);
    }

    public override string GetPrint(int level)
    {
        var result = "";
        result += Name;
        result += "\n";
        result += Parameters.GetPrint(level + 1);
        result += Locals.GetPrint(level + 1);
        result += Body.GetPrint(level + 1);
        return result;
    }
}