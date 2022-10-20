using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class OctInteger : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new OctEndState();
        }
        
        if(LexemesSeparators.VisibleSeparators.Contains((char)symbol))
        {
            return new OctEndState();
        }
        
        if (IntegerConstants.NumbersOct.Contains((char)symbol))
        {
            return new OctInteger();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new OctEndState();
        }

        return new ErrorState();
    }
}