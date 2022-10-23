using Compiler.Structs;

namespace Compiler.Lexeme;

public class SeparatorLexeme : ILexeme
{
    private const string _name = "Separator";
    private Coordinate _coordinate = new Coordinate{Line = 0, Column = 0};
    private string _value = "";
    private string _source = "";

    public Coordinate Position => Position;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;

    public string Description => _coordinate.Line.ToString() + " " + "\t" + 
                                 _coordinate.Column.ToString() + " " + "\t" +
                                 _name + " " + "\t" +
                                 _value + " " + "\t" +
                                 _source;

    public SeparatorLexeme(Coordinate coordinate, string source)
    {
        _coordinate = coordinate;
        _source = source;
        _value = source;
    }
}