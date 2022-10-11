namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public class StartState : IState
    {
        public IState GetNextState(char symbol)
        {
            return new EndState();
        }
    }
}