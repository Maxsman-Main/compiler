using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class IdentifierStartState : IState
{
    public IState GetNextState(int symbol)
    {
        if(LexemesSeparators.VisibleSeparators.Contains((char)symbol))
        {
            return new IdentifierEndState();
        }
        
        if (IdentifierConstants.Symbols.Contains((char)symbol))
        {
            return new IdentifierState();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new IdentifierEndState();
        }

        return new ErrorState();
    }
}