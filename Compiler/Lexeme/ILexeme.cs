using Compiler.Structs;

namespace Compiler.Lexeme
{
    //Добавить новые интерфейсы для лексем с Value разных типов - int, Enum и т.д.
    public interface ILexeme
    {
        public string Description { get; }
        public Coordinate Position { get; }
        public string Name { get; }
        public string Value { get; }
        public string Source { get; }
    }
}