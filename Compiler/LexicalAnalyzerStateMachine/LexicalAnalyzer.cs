using System.Text;
using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexicalAnalyzer
    {
        private readonly LexemeFactory _lexemeFactory = new();
        private readonly FileReader.FileReader _reader = new();
        
        private IState _currentState = new StartState();
        private int _symbol;

        private Coordinate _coordinate = new() {Line = 1, Column = 0};

        public ILexeme GetLexeme()
        {
            if (_reader.IsOpened == false)
            {
                _reader.OpenFile();
            }

            _currentState = new StartState();
            var word = "";

            while (_currentState is not IEndState)
            {
                _symbol = _reader.ReadSymbol();
                _currentState = _currentState.GetNextState(_symbol);

                if (_currentState is CommentEndState)
                {
                    _currentState = new StartState();
                    _coordinate.Column += 1;
                    word = "";
                    continue;
                }
                
                if (_currentState is not IEndState)
                {
                    if (_symbol == LexemesSeparators.EndOfLine)
                    {
                        _coordinate.Line += 1;
                        _coordinate.Column = 0;
                    }
                    _reader.MoveToNextPosition();
                }
                
                if (_currentState is not StartState && _currentState is not IEndState)
                {
                    word += (char)_symbol;
                }

            }
            
            _coordinate.Column += 1;

            var lexeme = _lexemeFactory.CreateLexemeByState(_currentState, _coordinate, word);
            return lexeme;
        }

        public void SetFile(string file)
        {
            _reader.SetFile(file);
        }
    }
}