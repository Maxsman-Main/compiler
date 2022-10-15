using System.Text;
using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine.States;
using Compiler.Structs;

namespace Compiler.LexicalAnalyzerStateMachine
{
    public class LexicalAnalyzer
    {
        //Разделить тестовую систему и обычные файлы
        private readonly LexemeFactory _lexemeFactory = new();
        private readonly FileReader.FileReader _reader = new();
        
        private IState _currentState = new StartState();
        private char _symbol;

        private Coordinate _coordinate = new() {Line = 1, Column = 0};

        public ILexeme GetLexeme()
        {
            if (_reader.IsOpened == false)
            {
                _reader.OpenFile();
            }

            _currentState = new StartState();
            var word = "";

            while (_currentState is not IEndState && _currentState is not ErrorState)
            {
                _symbol = _reader.ReadSymbol();
                _currentState = _currentState.GetNextState(_symbol);

                if (_currentState is not StartState)
                {
                    word += _symbol;
                }
            }
            
            // We should delete last element, and move file reader iterator back on one pos, cuz
            // we have stop state, when we get some SeparatorLexeme or symbol, which are another states
            _reader.MoveIteratorBackOnOneStep();
            word = DeleteLastElementOnWord(word);

            _coordinate.Column += 1;

            return _lexemeFactory.CreateLexemeByState(_currentState, _coordinate, word);
        }

        public void SetFile(string file)
        {
            _reader.SetFile(file);
        }

        private string DeleteLastElementOnWord(string word)
        {
            var builder = new StringBuilder(word);
            builder.Remove(word.Length - 1, 1);
            return builder.ToString();
        }
    }
}