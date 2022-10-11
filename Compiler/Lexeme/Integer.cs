using Compiler.Structs;

namespace Compiler.Lexeme
{
    public class Integer : ILexeme
    {
        private Coordinate _coordinate;
        private string _name = "Integer";
        private int _value = 0;
        private string _source;

        public string Description => _coordinate.Line.ToString() + " " +
                                     _coordinate.Column.ToString() + " " +
                                     _name + " " +
                                     _value.ToString() + " " +
                                     _source;
                                     

        public Integer(Coordinate coordinate, string source)
        {
            _coordinate = coordinate;
            _source = source;
            _value = Convert.ToInt32(_source);
        }
    }
}
