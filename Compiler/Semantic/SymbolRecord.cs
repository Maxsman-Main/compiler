namespace Compiler.Semantic;

public class SymbolRecord : SymbolType
{
    public SymbolTable Fields { get; }

    public SymbolRecord(string name, SymbolTable fields) : base(name)
    {
        Fields = fields;
    }

    public override string GetPrint(int level)
    {
        var result = "";
        for (var i = 0; i < level * 4; i++)
        {
            result += " ";
        }

        result += Name;
        if (Fields.Count == 0) return result;
        result += "\n";
        result += Fields.GetPrint(level + 1);
        //Delete excess \n symbol in the end of string
        result = result.Remove(result.Length - 1);
        
        return result;
    }
}