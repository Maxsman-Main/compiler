using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexicalAnalyzer
    {
        private readonly LexemeFactory _lexemeFactory = new();
        private readonly FileReader.FileReader _reader = new();
        
        private IState _currentState = new StartState();
        private int _symbol;
        private string _wordBuffer = "";

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
                    _reader.IncreaseColumn();
                    _wordBuffer = word;
                    word = "";
                    continue;
                }
                
                if (_currentState is not IEndState)
                {
                    if (_symbol == LexemesSeparators.EndOfLine)
                    {
                        _reader.IncreaseLine();
                    }
                    _reader.MoveToNextPosition();
                }
                
                if (_currentState is not StartState && _currentState is not IEndState)
                {
                    _wordBuffer = word;
                    word += (char)_symbol;
                }
            }
            
            _reader.IncreaseColumn();
            
            var lexeme = _lexemeFactory.CreateLexemeByState(_currentState, _reader.Coordinate, word);
            return lexeme;
        }

        public void SetFile(string file)
        {
            _reader.SetFile(file);
        }
    }
}