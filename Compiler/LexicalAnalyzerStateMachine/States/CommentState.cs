using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class CommentState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new CommentEndState();
        }
        
        if (LexemesSeparators.NewLineSeparators.Contains((char)symbol))
        {
            return new CommentEndState();
        }

        return new CommentState();
    }
}