using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public class StartState : IState
    {
        public IState GetNextState(char symbol)
        {
            if (IntegerConstants.NumbersDecimal.Contains(symbol))
            {
                return new DecimalInteger();
            }
            if(symbol == IntegerConstants.HexSymbol)
            {
                return new HexInteger();
            }

            if (symbol == IntegerConstants.OctSymbol)
            {
                return new OctInteger();
            }

            if (symbol == IntegerConstants.BinSymbol)
            {
                return new BinInteger();
            }

            if (symbol == IntegerConstants.MinusSign || symbol == IntegerConstants.PlusSign)
            {
                return new IntegerSign();
            }

            return new ErrorState();
        }
    }
}