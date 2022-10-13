using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public class StartState : IState
    {
        public IState GetNextState(char symbol)
        {
            if (Constant.NumbersDecimal.Contains(symbol))
            {
                return new DecimalInteger();
            }
            if(symbol == Constant.HexSymbol)
            {
                return new HexInteger();
            }

            if (symbol == Constant.OctSymbol)
            {
                return new OctInteger();
            }

            return new ErrorState();
        }
    }
}