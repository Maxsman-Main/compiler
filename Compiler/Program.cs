using Compiler.Lexeme;
using Compiler.Structs;

namespace Compiler
{
    public static class Program
    {
        public static void Main()
        {
            ILexeme integer = new Integer(new Coordinate{Line = 0, Column = 0}, "1123123");
            Console.WriteLine(integer.Description);
        }  
    }
}