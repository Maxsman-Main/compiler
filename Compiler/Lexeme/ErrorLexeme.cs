using Compiler.Structs;

namespace Compiler.Lexeme;

public class ErrorLexeme : ILexeme
{
    private const string _name = "Error";
    private Coordinate _coordinate;
    private string _value;
    private string _source;

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;
    public string Description => _coordinate.Line.ToString() + " " + "\t" +
                                 _coordinate.Column.ToString() + " " + "\t" +
                                 _value;

    public ErrorLexeme(Coordinate coordinate, string value, string source)
    {
        _coordinate = coordinate;
        _value = value;
        _source = source;
    }
}