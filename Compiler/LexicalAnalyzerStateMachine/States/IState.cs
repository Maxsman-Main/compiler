namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public interface IState
    {
        public IState GetNextState(int symbol);
    }
}