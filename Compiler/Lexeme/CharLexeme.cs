using System.Text;
using Compiler.Structs;

namespace Compiler.Lexeme;

public class CharLexeme : ILexeme
{
    private const string _name = "Char";
    private readonly Coordinate _coordinate;
    private readonly string _value;
    private readonly string _source;

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;
    
    public string Description => _coordinate.Line.ToString() + " " +
                                 _coordinate.Column.ToString() + " " +
                                 _name + " " +
                                 _value + " " +
                                 _source;

    public CharLexeme(Coordinate coordinate, string source, string value)
    {
        _coordinate = coordinate;
        _source = source;
        _value = value;
    }
}