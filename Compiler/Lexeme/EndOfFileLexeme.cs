using Compiler.Structs;

namespace Compiler.Lexeme;

public class EndOfFileLexeme : ILexeme
{
    private readonly string _value = "EndOfFile";
    private readonly string _source = "EndOfFile";
    private readonly string _name = "EndOfFile";
    private Coordinate _position;

    public Coordinate Position => _position;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;

    public string Description => Position.Line.ToString() + "\t" +
                                 Position.Column.ToString() + "\t" +
                                 _name;

    public EndOfFileLexeme(Coordinate coordinate)
    {
        _position = coordinate;
    }
}