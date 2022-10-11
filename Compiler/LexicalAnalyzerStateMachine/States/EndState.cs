namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public class EndState : IState
    {
        public IState GetNextState(char symbol)
        {
            return new EndState();
        }
    }
}