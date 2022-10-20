using Compiler.Structs;

namespace Compiler.Lexeme;

public class KeyWordLexeme : ILexeme
{
    private const string _name = "KeyWord";
    private Coordinate _coordinate;
    private string _value;
    private string _source;

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;

    public string Description => _coordinate.Line.ToString() + "\t" +
                                 _coordinate.Column.ToString() + "\t" +
                                 _name + " " +
                                 _value + " " +
                                 _source + " ";

    public KeyWordLexeme(Coordinate coordinate, string source)
    {
        _coordinate = coordinate;
        _source = source;
        _value = source;
    }
}