namespace Compiler.LexicalAnalyzerStateMachine.States;

public class SlashState : IState
{
    public IState GetNextState(int symbol)
    {
        if ((char) symbol == '/')
        {
            return new CommentState();
        }

        return new OperatorState();
    }
}