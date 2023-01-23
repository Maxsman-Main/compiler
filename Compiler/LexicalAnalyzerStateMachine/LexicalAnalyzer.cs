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
        
        private ILexeme _currentLexeme = new ErrorLexeme(new Coordinate{Line = 0, Column = 0}, "You must call GetLexeme first time, before get currentLexeme", "");
        private IState _currentState = new StartState();
        private int _symbol = '@';
        private string _wordBuffer = "";

        public ILexeme CurrentLexeme => _currentLexeme;
        public string Coordinate => _reader.Coordinate.Line + " " + _reader.Coordinate.Column;
        
        public ILexeme GetLexeme()
        {
            if (_reader.IsOpened == false)
            {
                _reader.OpenFile();
            }
            
            if (_wordBuffer != "")
            {
                _reader.IncreaseColumn();
                _wordBuffer = "";
                var lexemeRange = _lexemeFactory.CreateLexemeByState(new OperatorEndState(), _reader.Coordinate, "..");
                _currentLexeme = lexemeRange;
                return lexemeRange;
            }

            _currentState = new StartState();
            var word = "";
            
            while (_currentState is not IEndState)
            {
                _symbol = _reader.ReadSymbol();
                _currentState = _currentState.GetNextState(_symbol);

                if (_currentState is RangeState)
                {
                    _reader.MoveToNextPosition();
                    _wordBuffer = word + (char)_symbol;
                    var decimalPart = "";
                    foreach(var lit in _wordBuffer)
                    {
                        if (lit != '.')
                        {
                            decimalPart += lit;
                        }
                    }

                    _reader.IncreaseColumn();
                    var lexemeDecimal = _lexemeFactory.CreateLexemeByState(new DecimalEndState(), _reader.Coordinate, decimalPart);
                    _currentLexeme = lexemeDecimal;
                    return lexemeDecimal;
                }

                if (_currentState is CommentEndState)
                {
                    _currentState = new StartState();
                    _reader.IncreaseColumn();
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
                
                if (_currentState is not StartState && _currentState is not IEndState && !LexemesSeparators.InvisibleSeparators.Contains((char)_symbol))
                {
                    word += (char)_symbol;
                }
            }
            
            _reader.IncreaseColumn();
            
            var lexeme = _lexemeFactory.CreateLexemeByState(_currentState, _reader.Coordinate, word);
            _currentLexeme = lexeme;
            return lexeme;
        }

        public void SetFile(string filePath)
        {
            _reader.SetFile(filePath);
        }

        public void CloseFile()
        {
            _reader.CloseFile();
        }
    }
}