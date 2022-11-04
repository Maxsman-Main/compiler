using Compiler.Structs;

namespace Compiler.Lexeme
{
    public class IntegerLexeme : ILexeme, IIntegerLexeme
    {
        private const string _name = "Integer";
        private readonly Coordinate _coordinate;
        private readonly int _value;
        private readonly string _source;

        public Coordinate Position => _coordinate;
        public string Name => _name;
        public int Value => _value;
        public string Source => _source;
        public string Description => _coordinate.Line.ToString() + " " + "\t" +
                                     _coordinate.Column.ToString() + " " + "\t" +
                                     _name + " " + "\t" +
                                     _value + " " + "\t" +
                                     _source;

        public IntegerLexeme(Coordinate coordinate, string source, int value)
        {
            _coordinate = coordinate;
            _value = value;
            _source = source;
        }
    }
}
