using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexicalAnalyzer
    {
        private IState _currentState = new StartState();
        private string _fileName = "";
        
        public void SetFileName(string fileName)
        {
            _fileName = fileName;
        }
        
        public ILexeme GetNextLexeme()
        {
            _currentState = new StartState();
            _currentState = new EndState();
            return new Integer(new Coordinate{Line = 0, Column = 0}, "1234");
        }
    }
}