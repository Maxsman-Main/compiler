using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CharState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new CharEndState();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new CharEndState();
        }

        if (LexemesSeparators.VisibleSeparators.Contains((char) symbol))
        {
            return new CharEndState();
        }
        
        if (OperatorConstants.Operators.Contains((char)symbol))
        {
            return new CharEndState();
        }
        
        return new ErrorState();
    }
}