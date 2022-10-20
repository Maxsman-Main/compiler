using Compiler.Structs;

namespace Compiler.Lexeme;

public class IdentifierLexeme : ILexeme
{
    private const string _name = "Identifier"; 
    private Coordinate _position = new Coordinate{Line =  0, Column = 0};
    private string _value = "";
    private string _source = "";

    public Coordinate Position => _position;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;

    public string Description => _position.Line.ToString() + "\t" +
                                 _position.Column.ToString() + "\t" +
                                 _name + " " +
                                 _value + " " +
                                 _source;

    public IdentifierLexeme(Coordinate coordinate, string source)
    {
        _position = coordinate;
        _source = source;
        _value = source;
    }
}