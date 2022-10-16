﻿using System.Text;
using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States
{
    public class DecimalInteger : IState
    {
        public IState GetNextState(int symbol)
        {
            if (symbol == LexemesSeparators.EndOfFile)
            {
                return new DecimalEndState();
            }
            
            if (IntegerConstants.NumbersDecimal.Contains((char)symbol))
            {
                return new DecimalInteger();
            }

            if (FloatConstants.FloatSymbol == symbol)
            {
                return new FloatState();
            }

            if (LexemesSeparators.ContainSymbol(symbol))
            {
                return new DecimalEndState();
            }
            return new ErrorState();
        }
    }
}