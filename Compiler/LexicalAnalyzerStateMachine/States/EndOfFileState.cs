using Compiler.Lexeme;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class EndOfFileState : IState, IEndState
{
    public IState GetNextState(int symbol)
    {
        return new EndOfFileState();
    }
}