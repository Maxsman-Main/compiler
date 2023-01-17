using System.Collections.Specialized;
using Compiler.Constants;
using Compiler.Parser.Tree;

namespace Compiler.Semantic;

public class SymbolProcedure : Symbol
{
    protected readonly SymbolTable Locals;
    protected readonly INodeStatement Body;

    public SymbolTable Parameters { get; }

    public SymbolProcedure(string name, SymbolTable parameters, SymbolTable locals, INodeStatement body, SymbolTableStack stack) : base(name)
    {
        var dictionary = new OrderedDictionary();
        foreach (var parameter in parameters.Data.Values)
        {
            var symbol = (SymbolVariable) parameter;
            var symbolParameter = symbol.ConvertToParameter(stack);
            dictionary.Add(symbolParameter.Name, symbolParameter);
        }
        Parameters = new SymbolTable(dictionary);

        dictionary = new OrderedDictionary();
        foreach (var local in locals.Data.Values)
        {
            var symbol = (SymbolVariable) local;
            var symbolLocal = symbol.ConvertToLocal(stack);
            dictionary.Add(symbolLocal.Name, symbolLocal); 
        }
        Locals = new SymbolTable(dictionary);

        Body = body;
        
    }

    public override void Generate(Generator.Generator generator)
    {
        generator.Add(this);
        if (Body is CompoundStatement)
        {
            Body.Generate(generator);
        }
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