using Compiler.Structs;

namespace Compiler.Lexeme;

public class FloatLexeme : ILexeme, IFloatLexeme
{
    private const string _name = "Float";
    private readonly Coordinate _coordinate;
    private readonly double _value;
    private readonly string _source;

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public double Value => _value;
    public string Source => _source;
    
    public string Description => _coordinate.Line.ToString() + " " + "\t" +
                                 _coordinate.Column.ToString() + " " + "\t" +
                                 _name + " " + "\t" +
                                 _value + " " + "\t" +
                                 _source;

    public FloatLexeme(Coordinate coordinate, string source, double value)
    {
        _coordinate = coordinate;
        _value = value;
        _source = source;
    }
}