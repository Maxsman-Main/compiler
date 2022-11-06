namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public interface IState
    {
        public IState GetNextState(int symbol);
    }
    
    public interface IErrorState
    {
        public string Message { get; }
    }
    
    public interface IEndState
    {
    
    }
}