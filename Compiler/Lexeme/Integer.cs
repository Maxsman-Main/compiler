using Compiler.Structs;

namespace Compiler.Lexeme
{
    public class Integer : ILexeme
    {
        private const string Name = "Integer";

        private readonly Coordinate _coordinate;
        private readonly int _value;
        private readonly string _source;

        public string Description => _coordinate.Line.ToString() + " " +
                                     _coordinate.Column.ToString() + " " +
                                     Name + " " +
                                     _value.ToString() + " " +
                                     _source;

        public Integer(Coordinate coordinate, string source)
        {
            _coordinate = coordinate;
            _value = Convert.ToInt32(source);
            _source = source;
        }
    }
}
