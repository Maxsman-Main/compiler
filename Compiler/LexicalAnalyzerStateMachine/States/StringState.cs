using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States;

public class StringState : IState
{
    public IState GetNextState(int symbol)
    {
        if (symbol == LexemesSeparators.EndOfFile)
        {
            return new ErrorState("Incorrect lexeme end of file found in string");
        }
        
        if (symbol == StringConstants.StringSymbol)
        {
            return new StringWordEndState();
        }

        return new StringState();
    }
}