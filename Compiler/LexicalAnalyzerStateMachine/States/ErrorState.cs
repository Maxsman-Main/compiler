using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class ErrorState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new ErrorEndState();
        }
        
        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new ErrorEndState();
        }

        if (LexemesSeparators.VisibleSeparators.Contains((char) symbol))
        {
            return new ErrorEndState();
        }

        if (OperatorConstants.Operators.Contains((char)symbol))
        {
            return new ErrorEndState();
        }

        return new ErrorState();
    }
}