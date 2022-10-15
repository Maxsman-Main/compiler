using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringStartState : IState
{
    public IState GetNextState(char symbol)
    {
        if (symbol != StringConstants.StringSymbol)
        {
            return new StringStartWordState();
        }
        
        return new CharState();
    }
}