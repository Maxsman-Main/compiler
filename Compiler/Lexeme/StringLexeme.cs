﻿using Compiler.Structs;

namespace Compiler.Lexeme;

public class StringLexeme : ILexeme, IStringLexeme
{
    private const string _name = "String";
    private readonly Coordinate _coordinate;
    private readonly string _value;
    private readonly string _source;

    public Coordinate Position => _coordinate;
    public string Name => _name;
    public string Value => _value;
    public string Source => _source;
    
    public string Description => _coordinate.Line.ToString() + " " + "\t" +
                                 _coordinate.Column.ToString() + " " + "\t" +
                                 _name + " " + "\t" +
                                 _value + " " + "\t" +
                                 _source;

    public StringLexeme(Coordinate coordinate, string source, string value)
    {
        _coordinate = coordinate;
        _source = source;
        _value = value;
    }
}