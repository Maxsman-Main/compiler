using Compiler.Constants;

namespace Compiler.LexicalAnalyzerStateMachine.States
{

    public class StartState : IState
    {
        public IState GetNextState(int symbol)
        {
            if (symbol == LexemesSeparators.EndOfFile)
            {
                return new EndOfFileState();
            }
            
            if (IntegerConstants.NumbersDecimal.Contains((char)symbol))
            {
                return new DecimalInteger();
            }
            if (symbol == IntegerConstants.HexSymbol)
            {
                return new HexInteger();
            }

            if (symbol == IntegerConstants.OctSymbol)
            {
                return new OctInteger();
            }

            if (symbol == IntegerConstants.BinSymbol)
            {
                return new BinInteger();
            }

            if (symbol == StringConstants.StringSymbol)
            {
                return new StringStartState();
            }

            if (symbol == OperatorConstants.Column)
            {
                return new ColumnOperatorState();
            }

            if (symbol == OperatorConstants.MoreSign)
            {
                return new MoreOperatorState();
            }
            
            if (symbol == OperatorConstants.LessSign)
            {
                return new LessOperatorState();
            }

            if ((char)symbol == '/')
            {
                return new SlashState();
            }
            
            if (OperatorConstants.Operators.Contains((char)symbol))
            {
                return new OperatorState();
            }

            if (IdentifierConstants.StartSymbols.Contains((char)symbol))
            {
                return new IdentifierStartState();
            }

            if (LexemesSeparators.VisibleSeparators.Contains((char)symbol))
            {
                return new SeparatorState();
            }

            if (LexemesSeparators.ContainSymbol(symbol))
            {
                return new StartState();
            }
            
            return new ErrorState("Cannot find start state in lexeme analyzer for " + (char)symbol);
        }
    }
}