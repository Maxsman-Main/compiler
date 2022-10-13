﻿using Compiler.Structs;

namespace Compiler.Lexeme
{
    public class Integer : ILexeme
    {
        private const string _name = "Integer";
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

        public Integer(Coordinate coordinate, string source, string valueForConvert, int basis)
        {
            _coordinate = coordinate;
            _value = Convert.ToInt32(valueForConvert, basis).ToString();
            _source = source;
        }
    }
}
