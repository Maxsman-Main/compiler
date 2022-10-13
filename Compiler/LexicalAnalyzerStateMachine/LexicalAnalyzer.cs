using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexicalAnalyzer
    {
        private readonly LexemeFactory _lexemeFactory = new LexemeFactory();
        
        private IState _currentState = new StartState();

        public ILexeme GetNextLexeme(string word)
        {
            _currentState = new StartState();
            foreach (var letter in word) 
            {
                _currentState = _currentState.GetNextState(letter);
            }

            return _lexemeFactory.CreateLexemeByState(_currentState, new Coordinate{Line = 0, Column = 0}, word);
        }
    }
}