using Compiler.Structs;

namespace Compiler.Lexeme
{
    public class IntegerLexeme : ILexeme
    {
        private const string _name = "Integer";
        private readonly Coordinate _coordinate;
        private readonly string _value;
        private readonly string _source;

        public Coordinate Position => _coordinate;
        public string Name => _name;
        public string Value => _value;
        public string Source => _source;
        public string Description => _coordinate.Line.ToString() + "\t" +
                                     _coordinate.Column.ToString() + "\t" +
                                     _name + "\t" + "\t" +
                                     _value + "\t" +
                                     _source;

        public IntegerLexeme(Coordinate coordinate, string source, string valueForConvert, int basis, int sign)
        {
            _coordinate = coordinate;
            _value = (Convert.ToInt32(valueForConvert, basis) * sign).ToString();
            _source = source;
        }
    }
}
