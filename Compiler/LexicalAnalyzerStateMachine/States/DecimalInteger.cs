namespace Compiler.LexicalAnalyzerStateMachine.States
{
    public class DecimalInteger : IState
    {
        private readonly List<int> _numbers = new List<int>()
            {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        
        public IState GetNextState(char symbol)
        {
            if (_numbers.Contains(symbol))
            {
                return new DecimalInteger();
            }

            return new ErrorState();
        }
    }
}