using Compiler.Constants;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Parser.Tree;
using Type = Compiler.Parser.Tree.Type;

namespace Compiler.Parser;

public class Parser
{
    private readonly LexicalAnalyzer _lexer;

    public Parser(LexicalAnalyzer lexer)
    {
        _lexer = lexer;
        _lexer.GetLexeme();
    }

    public INode ParseProgram()
    {
        Variable? identifier = null;
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is IKeyWordLexeme {Value: KeyWordValue.Program})
        {
            _lexer.GetLexeme();
            var nextToken = _lexer.CurrentLexeme;
            if (nextToken is IIdentifierLexeme identifierLexeme)
            {
                _lexer.GetLexeme();
                identifier = new Variable(identifierLexeme.Value);
            }
            else
            {
                throw new Exception("Identifier was expected");
            }

            nextToken = _lexer.CurrentLexeme;
            if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new Exception("; was expected");
            }

            _lexer.GetLexeme();
        }

        lexeme = _lexer.CurrentLexeme;
        List<INodeDeclaration> declarations = new();
        while (lexeme is IKeyWordLexeme
               {
                   Value: 
                   KeyWordValue.Const or
                   KeyWordValue.Var or
                   KeyWordValue.Type or
                   KeyWordValue.Function or
                   KeyWordValue.Procedure
               })
        {
            declarations.Add(ParseDeclaration());
            lexeme = _lexer.CurrentLexeme;
        }

        var mainBlock = ParseMainBlock();

        return identifier is null ? new Tree.Program(declarations, mainBlock) : new Tree.Program(identifier, declarations, mainBlock);
    }

    private INodeDeclaration ParseDeclaration()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is IKeyWordLexeme {Value: KeyWordValue.Type})
        {
            _lexer.GetLexeme();
            var typesDeclarations = new List<TypeDeclarationPair>();
            do
            {
                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IIdentifierLexeme identifierLexeme)
                {
                    throw new Exception("Identifier was expected");
                }
                _lexer.GetLexeme();

                var identifier = new Variable(identifierLexeme.Value);

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
                {
                    throw new Exception("= was expected");
                }
                _lexer.GetLexeme();

                var type = ParseType();
                TypeDeclarationPair typeDeclaration;
                typeDeclaration.Identifier = identifier;
                typeDeclaration.Type = type;
                typesDeclarations.Add(typeDeclaration);
                
                lexeme  = _lexer.CurrentLexeme;
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
                {
                    throw new Exception("; was expected");
                }
                _lexer.GetLexeme();
                lexeme = _lexer.CurrentLexeme;
            } while (lexeme is IIdentifierLexeme);

            return new TypeDeclaration(typesDeclarations);
        }

        throw new Exception("keyword type was expected");
    }

    private INodeType ParseType()
    {
        var lexeme = _lexer.CurrentLexeme;
        Type type;
        if (lexeme is IKeyWordLexeme {Value: KeyWordValue.Integer})
        {
            _lexer.GetLexeme();
            type = new Type(KeyWordValue.Integer);
        }
        else
        {
            throw new Exception("keyword integer was expected");
        }
        
        return type;
    }

    private IMainBlockNode ParseMainBlock()
    {
        return null;
    }
    
    private INode ParseExpression()
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

    private INode ParseTerm()
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

    private INode ParseFactor()
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
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
                {
                    throw new Exception("Factor was expected");
                }
                _lexer.GetLexeme();
                var expression = ParseExpression();
                var nextToken = _lexer.CurrentLexeme;
                if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    throw new Exception("No right bracket");
                }
                
                _lexer.GetLexeme();

                return expression;
        }
    }
}