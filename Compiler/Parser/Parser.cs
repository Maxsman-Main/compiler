using Compiler.Constants;
using Compiler.Exceptions;
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
                throw new CompilerException("Identifier was expected");
            }

            nextToken = _lexer.CurrentLexeme;
            if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new CompilerException("; was expected");
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
        switch (lexeme)
        {
            case IKeyWordLexeme {Value: KeyWordValue.Type}:
                _lexer.GetLexeme();
                return ParseTypeDeclaration();
            case IKeyWordLexeme {Value: KeyWordValue.Const}:
                _lexer.GetLexeme();
                return ParseConstDeclaration();
            case IKeyWordLexeme{Value: KeyWordValue.Var}:
                _lexer.GetLexeme();
                return ParseVarDeclaration();
            case IKeyWordLexeme {Value: KeyWordValue.Function}:
                _lexer.GetLexeme();
                return ParseFunctionDeclaration();
            default:
                throw new CompilerException("keyword type was expected");
        }
    }

    private INodeDeclaration ParseTypeDeclaration()
    {
        ILexeme lexeme;
        var typesDeclarations = new List<TypeDeclarationPair>();
        do
        {
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIdentifierLexeme identifierLexeme)
            {
                throw new CompilerException("Identifier was expected");
            }
            _lexer.GetLexeme();

            var identifier = new Variable(identifierLexeme.Value);

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
            {
                throw new CompilerException("= was expected");
            }
            _lexer.GetLexeme();

            var type = ParseType();

            lexeme  = _lexer.CurrentLexeme;
            if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new CompilerException("; was expected");
            }
            _lexer.GetLexeme();
            
            TypeDeclarationPair typeDeclaration;
            typeDeclaration.Identifier = identifier;
            typeDeclaration.Type = type;
            typesDeclarations.Add(typeDeclaration);
            
            lexeme = _lexer.CurrentLexeme;
        } while (lexeme is IIdentifierLexeme);

        return new TypeDeclaration(typesDeclarations);
    }

    private INodeDeclaration ParseConstDeclaration()
    {
        ILexeme lexeme;
        var constDeclarations = new List<ConstDeclarationData>();
        do
        {

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIdentifierLexeme identifierLexeme)
            {
                throw new CompilerException("Identifier was expected");
            }
            _lexer.GetLexeme();

            var identifier = new Variable(identifierLexeme.Value);

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
            {
                throw new CompilerException(": was expected");
            }
            _lexer.GetLexeme();

            var type = ParseType();
            
            lexeme  = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
            {
                throw new CompilerException("= was expected");
            }
            _lexer.GetLexeme();

            var expression = ParseExpression();

            lexeme  = _lexer.CurrentLexeme;
            if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new CompilerException("; was expected");
            }
            _lexer.GetLexeme();

            ConstDeclarationData constData;
            constData.Identifier = identifier;
            constData.Type = type;
            constData.Expression = expression;
            constDeclarations.Add(constData);
            
            lexeme = _lexer.CurrentLexeme;
        } while (lexeme is IIdentifierLexeme);

        return new ConstDeclaration(constDeclarations);
    }

    private INodeDeclaration ParseVarDeclaration()
    {
        ILexeme lexeme;
        var varDeclarations = new List<VarDeclarationData>();   
        do
        {
            List<Variable> identifierList = new();
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIdentifierLexeme identifierLexeme)
            {
                throw new CompilerException("Identifier was expected");
            }
            _lexer.GetLexeme();

            identifierList.Add(new Variable(identifierLexeme.Value));

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma})
            {
                do
                {
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Comma})
                    {
                        throw new CompilerException(", was expected");
                    }
                    _lexer.GetLexeme();
                    
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not IIdentifierLexeme identifierLexemeCycle)
                    {
                        throw new CompilerException("Identifier was expected");
                    }
                    _lexer.GetLexeme();
                
                    var identifier = new Variable(identifierLexemeCycle.Value);
                    identifierList.Add(identifier);
                    
                    lexeme = _lexer.CurrentLexeme;
                } while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma});

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
                {
                    throw new CompilerException(": was expected");
                }
                _lexer.GetLexeme();

                var type = ParseType();

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
                {
                    throw new CompilerException("; was expected");
                }
                _lexer.GetLexeme();

                foreach (var identifier in identifierList)
                {
                    VarDeclarationData varDeclaration;
                    varDeclaration.Identifier = identifier;
                    varDeclaration.Type = type;
                    varDeclaration.Expression = null;
                    varDeclarations.Add(varDeclaration);
                }

                lexeme = _lexer.CurrentLexeme;
            }
            else
            {
                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
                {
                    throw new CompilerException(": was expected");
                }
                _lexer.GetLexeme();

                var type = ParseType();

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
                {
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
                    {
                        throw new CompilerException("; was expected");
                    }
                    _lexer.GetLexeme();
                    
                    foreach (var identifier in identifierList)
                    {
                        VarDeclarationData varDeclaration;
                        varDeclaration.Identifier = identifier;
                        varDeclaration.Type = type;
                        varDeclaration.Expression = null;
                        varDeclarations.Add(varDeclaration);
                    }

                    lexeme = _lexer.CurrentLexeme;
                }
                else
                {
                    _lexer.GetLexeme();
                    
                    var expression = ParseExpression();

                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
                    {
                        throw new CompilerException("; was expected");
                    }
                    _lexer.GetLexeme();

                    foreach (var identifier in identifierList)
                    {
                        VarDeclarationData varDeclaration;
                        varDeclaration.Identifier = identifier;
                        varDeclaration.Type = type;
                        varDeclaration.Expression = expression;
                        varDeclarations.Add(varDeclaration);
                    }

                    lexeme = _lexer.CurrentLexeme;
                }
            }
        } while (lexeme is IIdentifierLexeme);

        return new VarDeclaration(varDeclarations);
    }

    private INodeDeclaration ParseFunctionDeclaration()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIdentifierLexeme identifierLexeme)
        {
            throw new CompilerException("identifier was expected");
        }
        _lexer.GetLexeme();

        var identifier = new Variable(identifierLexeme.Value);

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
        {
            throw new CompilerException("( was expected");
        }
        _lexer.GetLexeme();

        var parameters = ParseParameters();
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            throw new CompilerException(") was expected");
        }
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
        {
            throw new CompilerException(": was expected");
        }
        _lexer.GetLexeme();
        
        var type = ParseType();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException("; was expected");
        }
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        List<INodeDeclaration> declarations = new();
        while (lexeme is IKeyWordLexeme {Value: KeyWordValue.Const or KeyWordValue.Var or KeyWordValue.Type})
        {
            switch (lexeme)
            {
                case IKeyWordLexeme {Value: KeyWordValue.Const}:
                {
                    _lexer.GetLexeme();
                    var declaration = ParseConstDeclaration();
                    declarations.Add(declaration);
                    break;
                }
                case IKeyWordLexeme {Value: KeyWordValue.Var}:
                {
                    _lexer.GetLexeme();
                    var declaration = ParseVarDeclaration();
                    declarations.Add(declaration);
                    break;
                }
                case IKeyWordLexeme {Value: KeyWordValue.Type}:
                {
                    _lexer.GetLexeme();
                    var declaration = ParseTypeDeclaration();
                    declarations.Add(declaration);
                    break;
                }
            }

            lexeme = _lexer.CurrentLexeme;
        }

        var statement = ParseStatement();
        return new FunctionDeclaration(identifier, parameters, type, declarations, statement);
    }

    private List<Parameter> ParseParameters()
    {
        List<Parameter> parametersResult = new();
        
        var parameters = ParseParameter();
        if (parameters.Count == 0)
        {
            return parametersResult;
        }
        
        foreach (var parameter in parameters)
        {
            parametersResult.Add(parameter);
        }
        
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma})
        {
            _lexer.GetLexeme();
            parameters = ParseParameter();
            foreach (var parameter in parameters)
            {
                parametersResult.Add(parameter);
            }

            lexeme = _lexer.CurrentLexeme;
        }

        return parametersResult;
    }

    private List<Parameter> ParseParameter()
    {
        var identifierList = ParseIdentifierList();
        List<Parameter> parameters = new();

        if (identifierList.Count == 0)
        {
            return parameters;
        }
        
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
        {
            throw new CompilerException(": was expected");
        }
        _lexer.GetLexeme();

        var type = ParseType();

        foreach (var identifier in identifierList)
        {
            Parameter parameter;
            parameter.Identifier = identifier;
            parameter.Type = type;
            parameters.Add(parameter);
        }

        return parameters;
    }

    private List<Variable> ParseIdentifierList()
    {
        var lexeme = _lexer.CurrentLexeme;
        List<Variable> identifiers = new();
        if (lexeme is not IIdentifierLexeme identifierLexeme)
        {
            return identifiers;
        }
        _lexer.GetLexeme();
        
        identifiers.Add(new Variable(identifierLexeme.Value));
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma})
        {
            _lexer.GetLexeme();

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIdentifierLexeme identifierLexemeCycle)
            {
                throw new CompilerException("identifier was expected");
            }
            _lexer.GetLexeme();
            
            identifiers.Add(new Variable(identifierLexemeCycle.Value));
            lexeme = _lexer.CurrentLexeme;
        }
        
        return identifiers;
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
            throw new CompilerException("keyword integer was expected");
        }

        return type;
    }

    private IMainBlockNode ParseMainBlock()
    {
        return null;
    }

    private INodeStatement ParseStatement()
    {
        return null;
    }
    
    private INodeExpression ParseExpression()
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

    private INodeExpression ParseTerm()
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

    private INodeExpression ParseFactor()
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
                    throw new CompilerException("Factor was expected");
                }
                _lexer.GetLexeme();
                var expression = ParseExpression();
                var nextToken = _lexer.CurrentLexeme;
                if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    throw new CompilerException("No right bracket");
                }
                
                _lexer.GetLexeme();

                return expression;
        }
    }
}