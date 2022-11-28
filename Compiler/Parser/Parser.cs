using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Parser.Tree;

namespace Compiler.Parser;

public class Parser
{
    private LexicalAnalyzer _lexer;

    public Parser(LexicalAnalyzer lexer)
    {
        _lexer = lexer;
        _lexer.GetLexeme();
    }
    
    public Node ParseExpression()
    {
        var left = ParseTerm();
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Plus or OperatorValue.Minus} operatorLexeme)
        {
            _lexer.GetLexeme();
            left = new BinOperation(operatorLexeme.Value, left, ParseTerm());
            lexeme = _lexer.CurrentLexeme;
        }

        return left;
    }

    private Node ParseTerm()
    {
        var left = ParseFactor();
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Multiplication or OperatorValue.Div} operatorLexeme)
        {
            _lexer.GetLexeme();
            left = new BinOperation(operatorLexeme.Value, left, ParseFactor());
            lexeme = _lexer.CurrentLexeme;
        }

        return left;
    }

    private Node ParseFactor()
    {
        var lexeme = _lexer.CurrentLexeme;
        switch (lexeme)
        {
            case IIntegerLexeme integerLexeme:
                _lexer.GetLexeme();
                return new Number(integerLexeme.Value);
            case IIdentifierLexeme identifierLexeme:
                _lexer.GetLexeme();
                return new Variable(identifierLexeme.Value);
            default:
                if (lexeme.Source != "(")
                {
                    throw new Exception("Factor excepted");
                }
                _lexer.GetLexeme();
                var expression = ParseExpression();
                var nextToken = _lexer.CurrentLexeme;
                if (nextToken.Source != ")")
                {
                    throw new Exception("No right bracket");
                }
                
                _lexer.GetLexeme();

                return expression;
        }
    }
}