using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class IntWithDotState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new FloatEndState();
        }

        if (FloatConstants.FloatSymbol == (char) symbol)
        {
            return new RangeState();
        }
        
        if (LexemesSeparators.VisibleSeparators.Contains((char)symbol))
        {
            return new FloatEndState();
        }
        
        if (FloatConstants.NumbersFloat.Contains((char)symbol))
        {
            return new FloatState();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new FloatEndState();
        }
        
        if (OperatorConstants.Operators.Contains((char)symbol))
        {
            return new FloatEndState();
        }

        return new ErrorState("Incorrect lexeme, excepted 0-9 or some separator or '.', but " + (char)symbol + " found");
    }
}