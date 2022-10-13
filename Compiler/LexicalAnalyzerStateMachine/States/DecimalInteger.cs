using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States
{
    public class DecimalInteger : IState
    {
        public IState GetNextState(char symbol)
        {
            if (Constant.NumbersDecimal.Contains(symbol))
            {
                return new DecimalInteger();
            }

            return new ErrorState();
        }
    }
}