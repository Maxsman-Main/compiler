﻿using Compiler.Constants;
using Compiler.Structs;

namespace Compiler.Lexeme
{
    public interface ILexeme
    {
        public string Description { get; }
        public Coordinate Position { get; }
        public string Name { get; }
        public string Source { get; }
    }

    public interface IIntegerLexeme
    {
        public int Value { get; }
    }

    public interface IFloatLexeme
    {
        public double Value { get; }
    }

    public interface IOperatorLexeme
    {
        public OperatorValue Value { get; }
    }

    public interface IIdentifierLexeme
    {
        public string Value { get; }
    }

    public interface ISeparatorLexeme
    {
        public SeparatorValue Value { get; }
    }

    public interface IKeyWordLexeme
    {
        public KeyWordValue Value { get; }
    }

    public interface IStringLexeme
    {
        public string Value { get; }
    }

    public interface ICharLexeme
    {
        public string Value { get; }
    }
}