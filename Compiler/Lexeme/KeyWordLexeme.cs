using Compiler.Constants;
using Compiler.Structs;

namespace Compiler.Lexeme;

public class KeyWordLexeme : ILexeme, IKeyWordLexeme
{
    private const string _name = "KeyWord";
    private Coordinate _coordinate;
    private KeyWordValue _value;
    private string _source;

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public KeyWordValue Value => _value;
    public string Source => _source;

    public string Description => _coordinate.Line.ToString() + " " + "\t" +
                                 _coordinate.Column.ToString() + " " + "\t" +
                                 _name + " " + "\t" + 
                                 _value + " " + "\t" +
                                 _source + " " + "\t";

    public KeyWordLexeme(Coordinate coordinate, string source, KeyWordValue value)
    {
        _coordinate = coordinate;
        _source = source;
        _value = value;
    }
}