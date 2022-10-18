using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CommentState : IState
{
    public IState GetNextState(int symbol)
    {
        if (LexemesSeparators.NewLineSeparators.Contains((char)symbol) || symbol == LexemesSeparators.EndOfFile)
        {
            return new CommentEndState();
        }

        return new CommentState();
    }
}