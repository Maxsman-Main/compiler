﻿using System.Text;
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

            while (_currentState is not IEndState && _currentState is not ErrorState)
            {
                _symbol = _reader.ReadSymbol();
                _currentState = _currentState.GetNextState(_symbol);
                
                if (_currentState is not IEndState)
                {
                    _reader.MoveToNextPosition();
                }
                
                if (_currentState is not StartState && _currentState is not IEndState)
                {
                    word += (char)_symbol;
                }
            }
            
            _coordinate.Column += 1;

            return _lexemeFactory.CreateLexemeByState(_currentState, _coordinate, word);
        }

        public void SetFile(string file)
        {
            _reader.SetFile(file);
        }
    }
}