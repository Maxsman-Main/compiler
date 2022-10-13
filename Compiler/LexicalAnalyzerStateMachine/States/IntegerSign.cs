using Compiler.Constants;
using Compiler.Lexeme;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class IntegerSign : IState
{
    public IState GetNextState(char symbol)
    {
        if (IntegerConstants.NumbersDecimal.Contains(symbol))
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