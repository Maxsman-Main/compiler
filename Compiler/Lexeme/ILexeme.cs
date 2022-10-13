using Compiler.Structs;

namespace Compiler.Lexeme
{
    public interface ILexeme
    {
        public string Description { get; }
        public Coordinate Position { get; }
        public string Name { get; }
        public string Value { get; }
        public string Source { get; }
    }
}