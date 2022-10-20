using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class HexInteger : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new HexEndState();
        }
        
        if(LexemesSeparators.VisibleSeparators.Contains((char)symbol))
        {
            return new HexEndState();
        }
        
        if (IntegerConstants.NumbersHex.Contains((char)symbol))
        {
            return new HexInteger();
        }

        if (LexemesSeparators.ContainSymbol(symbol))
        {
            return new HexEndState();
        }

        return new ErrorState();
    }
}