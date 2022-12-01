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
                throw new CompilerException(_lexer.Coordinate + " identifier was expected");
            }

            nextToken = _lexer.CurrentLexeme;
            if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new CompilerException(_lexer.Coordinate + " ; was expected");
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

        var mainBlock = ParseCompoundStatement();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Point})
        {
            throw new CompilerException(_lexer.Coordinate + " . was expected");
        }
        _lexer.GetLexeme();

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
            case IKeyWordLexeme{Value: KeyWordValue.Procedure}:
                _lexer.GetLexeme();
                return ParseProcedureDeclaration();
            default:
                throw new CompilerException(_lexer.Coordinate + " keyword type was expected");
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
                throw new CompilerException(_lexer.Coordinate + " identifier was expected");
            }
            _lexer.GetLexeme();

            var identifier = new Variable(identifierLexeme.Value);

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
            {
                throw new CompilerException(_lexer.Coordinate + " = was expected");
            }
            _lexer.GetLexeme();

            var type = ParseType();

            lexeme  = _lexer.CurrentLexeme;
            if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new CompilerException(_lexer.Coordinate + " ; was expected");
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
                throw new CompilerException(_lexer.Coordinate + " Identifier was expected");
            }
            _lexer.GetLexeme();

            var identifier = new Variable(identifierLexeme.Value);

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
            {
                throw new CompilerException(_lexer.Coordinate + " : was expected");
            }
            _lexer.GetLexeme();

            var type = ParseType();
            
            lexeme  = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
            {
                throw new CompilerException(_lexer.Coordinate + " = was expected");
            }
            _lexer.GetLexeme();

            var expression = ParseExpression();

            lexeme  = _lexer.CurrentLexeme;
            if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
            {
                throw new CompilerException(_lexer.Coordinate + " ; was expected");
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
                throw new CompilerException(_lexer.Coordinate + " identifier was expected");
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
                        throw new CompilerException(_lexer.Coordinate + " , was expected");
                    }
                    _lexer.GetLexeme();
                    
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not IIdentifierLexeme identifierLexemeCycle)
                    {
                        throw new CompilerException(_lexer.Coordinate + " identifier was expected");
                    }
                    _lexer.GetLexeme();
                
                    var identifier = new Variable(identifierLexemeCycle.Value);
                    identifierList.Add(identifier);
                    
                    lexeme = _lexer.CurrentLexeme;
                } while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma});

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
                {
                    throw new CompilerException(_lexer.Coordinate + " : was expected");
                }
                _lexer.GetLexeme();

                var type = ParseType();

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
                {
                    throw new CompilerException(_lexer.Coordinate + " ; was expected");
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
                    throw new CompilerException(_lexer.Coordinate + " : was expected");
                }
                _lexer.GetLexeme();

                var type = ParseType();

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not IOperatorLexeme {Value: OperatorValue.Equal})
                {
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
                    {
                        throw new CompilerException(_lexer.Coordinate + " ; was expected");
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
                        throw new CompilerException(_lexer.Coordinate + " ; was expected");
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
            throw new CompilerException(_lexer.Coordinate + " identifier was expected");
        }
        _lexer.GetLexeme();

        var identifier = new Variable(identifierLexeme.Value);

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
        {
            throw new CompilerException(_lexer.Coordinate + " ( was expected");
        }
        _lexer.GetLexeme();

        var parameters = ParseParameters();
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            throw new CompilerException(_lexer.Coordinate + ") was expected");
        }
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
        {
            throw new CompilerException(_lexer.Coordinate + ": was expected");
        }
        _lexer.GetLexeme();
        
        var type = ParseType();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException(_lexer.Coordinate + "; was expected");
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

        var statement = ParseCompoundStatement();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException(_lexer.Coordinate + " ; was expected");
        }
        _lexer.GetLexeme();
        
        return new FunctionDeclaration(identifier, parameters, type, declarations, statement);
    }

    private INodeDeclaration ParseProcedureDeclaration()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIdentifierLexeme identifierLexeme)
        {
            throw new CompilerException(_lexer.Coordinate + " identifier was expected");
        }
        _lexer.GetLexeme();

        var identifier = new Variable(identifierLexeme.Value);

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
        {
            throw new CompilerException(_lexer.Coordinate + " ( was expected");
        }
        _lexer.GetLexeme();

        var parameters = ParseParameters();
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            throw new CompilerException(_lexer.Coordinate + ") was expected");
        }
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException(_lexer.Coordinate + "; was expected");
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

        var statement = ParseCompoundStatement();
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException(_lexer.Coordinate + " ; was expected");
        }
        _lexer.GetLexeme();

        return new ProcedureDeclaration(identifier, parameters, declarations, statement);
    }
    
    private List<Parameter> ParseParameters()
    {
        List<Parameter> parametersResult = new();

        var lexeme = _lexer.CurrentLexeme;
        List<Parameter> parameters;
        
        if (lexeme is KeyWordLexeme {Value: KeyWordValue.Var})
        {
            _lexer.GetLexeme();
            parameters = ParseVarParameter();
        }
        else
        {
            parameters = ParseValueParameter();
        }

        if (parameters.Count == 0)
        {
            return parametersResult;
        }
        
        foreach (var parameter in parameters)
        {
            parametersResult.Add(parameter);
        }
        
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            _lexer.GetLexeme();
            if (lexeme is KeyWordLexeme {Value: KeyWordValue.Var})
            {
                _lexer.GetLexeme();
                parameters = ParseVarParameter();
            }
            else
            {
                parameters = ParseValueParameter();
            }
            foreach (var parameter in parameters)
            {
                parametersResult.Add(parameter);
            }

            lexeme = _lexer.CurrentLexeme;
        }

        return parametersResult;
    }

    private List<Parameter> ParseValueParameter()
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
            throw new CompilerException(_lexer.Coordinate + ": was expected");
        }
        _lexer.GetLexeme();

        var type = ParseType();

        foreach (var identifier in identifierList)
        {
            var parameter = new ValueParameter(identifier, type);
            parameters.Add(parameter);
        }

        return parameters;
    }

    private List<Parameter> ParseVarParameter()
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
            throw new CompilerException(_lexer.Coordinate + ": was expected");
        }
        _lexer.GetLexeme();

        var type = ParseType();

        foreach (var identifier in identifierList)
        {
            var parameter = new VarParameter(identifier, type);
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
                throw new CompilerException(_lexer.Coordinate + "identifier was expected");
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
            throw new CompilerException(_lexer.Coordinate + "keyword integer was expected");
        }

        return type;
    }

    private List<INodeStatement> ParseStatementSequence()
    {
        List<INodeStatement> statements = new() {ParseStatement()};
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            _lexer.GetLexeme();
            statements.Add(ParseStatement());
            lexeme = _lexer.CurrentLexeme;
        }

        return statements;
    }

    private INodeStatement ParseStatement()
    {
        var lexeme = _lexer.CurrentLexeme;
        return lexeme switch
        {
            IIdentifierLexeme => ParseSimpleStatement(),
            IKeyWordLexeme {Value: KeyWordValue.Begin or KeyWordValue.While or KeyWordValue.For or KeyWordValue.If} =>
                ParseStructuredStatement(),
            _ => throw new CompilerException(_lexer.Coordinate + " identifier or begin or end or while or for or if was expected")
        };
    }

    private INodeStatement ParseSimpleStatement()
    {
        var lexeme = _lexer.CurrentLexeme;
        var identifier = new Variable(((IIdentifierLexeme) lexeme).Value);
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        return lexeme switch
        {
            IOperatorLexeme {Value: OperatorValue.Assignment} => ParseAssignmentStatement(identifier),
            _ => ParseProcedureStatement(identifier)
        };
    }

    private INodeStatement ParseProcedureStatement(Variable identifier)
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
            return new ProcedureStatement(identifier, null);
        _lexer.GetLexeme();
        var identifierList = ParseIdentifierList();
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            throw new CompilerException(_lexer.Coordinate + " ) was expected");
        }
        _lexer.GetLexeme();

        return new ProcedureStatement(identifier, identifierList);
    }

    private INodeStatement ParseAssignmentStatement(Variable identifier)
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme {Value: OperatorValue.Assignment})
            throw new CompilerException(_lexer.Coordinate + " := was expected");
        _lexer.GetLexeme();
        var expression = ParseExpression();
        return new AssignmentStatement(identifier, expression);

    }
    
    private INodeStatement ParseStructuredStatement()
    {
        var lexeme = _lexer.CurrentLexeme;
        return lexeme switch
        {
            IKeyWordLexeme {Value: KeyWordValue.Begin} => ParseCompoundStatement(),
            IKeyWordLexeme {Value: KeyWordValue.While} => ParseWhileStatement(),
            IKeyWordLexeme {Value: KeyWordValue.For} => ParseForStatement(),
            IKeyWordLexeme {Value: KeyWordValue.If} => ParseIfStatement(),
            _ => throw new CompilerException(_lexer.Coordinate + " can't find statement for this lexeme")
        };
    }

    private INodeStatement ParseCompoundStatement()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not KeyWordLexeme {Value: KeyWordValue.Begin})
        {
            throw new CompilerException(_lexer.Coordinate + " begin was expected");
        }
        _lexer.GetLexeme();

        var statements = ParseStatementSequence();
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not KeyWordLexeme {Value: KeyWordValue.End})
        {
            throw new CompilerException(_lexer.Coordinate + " end was expected");
        }
        _lexer.GetLexeme();

        return new CompoundStatement(statements);
    }

    private INodeStatement ParseWhileStatement()
    {
        _lexer.GetLexeme();

        var expression = ParseExpression();

        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.Do})
        {
            throw new CompilerException(_lexer.Coordinate + " do was expected");
        }
        _lexer.GetLexeme();

        var statement = ParseStatement();

        return new WhileStatement(expression, statement);
    }

    private INodeStatement ParseForStatement()
    {
        _lexer.GetLexeme();

        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IdentifierLexeme identifierLexeme)
        {
            throw new CompilerException(_lexer.Coordinate + " identifier was expected");
        }
        _lexer.GetLexeme();

        var identifier = new Variable(identifierLexeme.Value);

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not OperatorLexeme {Value: OperatorValue.Assignment})
        {
            throw new CompilerException(_lexer.Coordinate + " := was expected");
        }
        _lexer.GetLexeme();

        var startExpression = ParseExpression();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.To})
        {
            throw new CompilerException(_lexer.Coordinate + " to was expected");
        }
        _lexer.GetLexeme();

        var endExpression = ParseExpression();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.Do})
        {
            throw new CompilerException(_lexer.Coordinate + " do was expected");
        }
        _lexer.GetLexeme();

        var statement = ParseStatement();

        return new ForStatement(identifier, startExpression, endExpression, statement);
    }

    private INodeStatement ParseIfStatement()
    {
        _lexer.GetLexeme();

        var expression = ParseExpression();

        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.Then})
        {
            throw new CompilerException(_lexer.Coordinate + " then was expected");
        }
        _lexer.GetLexeme();

        var statement = ParseStatement();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.Else})
            return new IfStatement(expression, statement, null);
        _lexer.GetLexeme();
        var elsePart = ParseStatement();
        return new IfStatement(expression, statement, elsePart);

    }
    
    public INodeExpression ParseExpression()
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
                    throw new CompilerException(_lexer.Coordinate + " factor was expected");
                }
                _lexer.GetLexeme();
                var expression = ParseExpression();
                var nextToken = _lexer.CurrentLexeme;
                if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    throw new CompilerException(_lexer.Coordinate + " ) was expected");
                }
                
                _lexer.GetLexeme();

                return expression;
        }
    }
}