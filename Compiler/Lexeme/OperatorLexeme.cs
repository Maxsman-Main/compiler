using Compiler.Constants;
using Compiler.Structs;

namespace Compiler.Lexeme;

public class OperatorLexeme : ILexeme, IOperatorLexeme
{
    private string _name = "Operator";
    private Coordinate _coordinate = new Coordinate {Line = 0, Column = 0};
    private OperatorValue _value;
    private string _source = "";

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public OperatorValue Value => _value;
    public string Source => _source;

    public string Description => _coordinate.Line.ToString() + " " + "\t" + 
                                 _coordinate.Column.ToString() + " " + "\t" +
                                 _name + " " + "\t" +
                                 _value + " " + "\t" +
                                 _source + " " + "\t";

    public OperatorLexeme(Coordinate coordinate, string source, OperatorValue value)
    {
        _coordinate = coordinate;
        _source = source;
        _value = value;
    }
}