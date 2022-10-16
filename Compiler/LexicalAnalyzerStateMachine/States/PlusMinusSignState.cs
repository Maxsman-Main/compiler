using Compiler.Constants;
using Compiler.Lexeme;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class PlusMinusSignState : IState
{
    public IState GetNextState(int symbol)
    {
        if (IntegerConstants.NumbersDecimal.Contains((char)symbol))
        {
            return new DecimalInteger();
        }

        if (IntegerConstants.HexSymbol == symbol)
        {
            return new HexInteger();
        }

        if (IntegerConstants.OctSymbol == symbol)
        {
            return new OctInteger();
        }

        if (IntegerConstants.BinSymbol == symbol)
        {
            return new BinInteger();
        }

        return new ErrorState();
    }
}