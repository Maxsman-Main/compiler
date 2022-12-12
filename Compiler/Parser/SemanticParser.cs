using System.Collections.Specialized;
using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Parser.Tree;
using Compiler.Semantic;
using Char = Compiler.Parser.Tree.Char;
using CompoundStatement = Compiler.Parser.Tree.CompoundStatement;
using String = Compiler.Parser.Tree.String;

namespace Compiler.Parser;

public class SemanticParser
{
    private readonly LexicalAnalyzer _lexer;
    private readonly SymbolTableStack _stack;
    
    public SemanticParser(LexicalAnalyzer lexer)
    {
        _lexer = lexer;
        _lexer.GetLexeme();
        _stack = new SymbolTableStack();
    }

    public void ParseProgram()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is IKeyWordLexeme {Value: KeyWordValue.Program})
        {
            _lexer.GetLexeme();
            var nextToken = _lexer.CurrentLexeme;
            if (nextToken is IIdentifierLexeme)
            {
                _lexer.GetLexeme();
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

        ParseDeclaration();
        ParseCompoundStatement();
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Point})
        {
            throw new CompilerException(_lexer.Coordinate + " . was expected");
        }
        _lexer.GetLexeme();
    }

    private void ParseDeclaration()
    {
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is IKeyWordLexeme
               {
                   Value: KeyWordValue.Type or /*KeyWordValue.Const or*/ KeyWordValue.Var or KeyWordValue.Function
                   or KeyWordValue.Procedure
               })
        {
            switch (lexeme)
            {
                case IKeyWordLexeme {Value: KeyWordValue.Type}:
                    _lexer.GetLexeme();
                    ParseTypeDeclaration();
                    break;
                /*
                case IKeyWordLexeme {Value: KeyWordValue.Const}:
                    _lexer.GetLexeme();
                    ParseConstDeclaration();
                    break;
                */
                case IKeyWordLexeme {Value: KeyWordValue.Var}:
                    _lexer.GetLexeme();
                    ParseVarDeclaration();
                    break;
                case IKeyWordLexeme {Value: KeyWordValue.Function}:
                    _lexer.GetLexeme();
                    ParseFunctionDeclaration();
                    break;
                case IKeyWordLexeme {Value: KeyWordValue.Procedure}:
                    _lexer.GetLexeme();
                    ParseProcedureDeclaration();
                    break;
            }
        }
    }

    private void ParseTypeDeclaration()
    {
        ILexeme lexeme;
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
            
            _stack.Add(identifier.Name, new SymbolTypeAlias(identifier.Name, type));
            
            lexeme = _lexer.CurrentLexeme;
        } while (lexeme is IIdentifierLexeme);

    }

    /*
    private void ParseConstDeclaration()
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

    }
    */

    private void ParseVarDeclaration()
    {
        ILexeme lexeme;
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
                    _stack.Add(identifier.Name, new SymbolVariable(identifier.Name, type));
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
                        _stack.Add(identifier.Name, new SymbolVariable(identifier.Name, type));
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
                        _stack.Add(identifier.Name, new SymbolVariable(identifier.Name, type));
                    }

                    lexeme = _lexer.CurrentLexeme;
                }
            }
        } while (lexeme is IIdentifierLexeme);
    }

    private void ParseFunctionDeclaration()
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
            throw new CompilerException(_lexer.Coordinate + " ) was expected");
        }
        _lexer.GetLexeme();

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
            throw new CompilerException(_lexer.Coordinate + "; was expected");
        }
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        List<INodeDeclaration> declarations = new();
        ParseDeclaration();
        var statement = ParseCompoundStatement();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException(_lexer.Coordinate + " ; was expected");
        }
        _lexer.GetLexeme();
    }

    private void ParseProcedureDeclaration()
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

        
        ParseDeclaration();
        var statement = ParseCompoundStatement();
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            throw new CompilerException(_lexer.Coordinate + " ; was expected");
        }
        _lexer.GetLexeme();
        
        _stack.Add(identifier.Name, new SymbolProcedure(identifier.Name, parameters,  parameters, statement));
    }
    
    private SymbolTable ParseParameters()
    {
        OrderedDictionary parametersResult = new();
        var lexeme = _lexer.CurrentLexeme;
        List<Symbol> parameters;
        
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
            return new SymbolTable(parametersResult);
        }
        
        foreach (var parameter in parameters)
        {
            parametersResult.Add(parameter.Name, parameter);
        }
        
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            _lexer.GetLexeme();
            lexeme = _lexer.CurrentLexeme;
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
                parametersResult.Add(parameter.Name, parameter);
            }

            lexeme = _lexer.CurrentLexeme;
        }

        return new SymbolTable(parametersResult);
    }

    private List<Symbol> ParseValueParameter()
    {
        var identifierList = ParseIdentifierList();
        List<Symbol> parameters = new();

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

        parameters.AddRange(identifierList.Select(identifier => new SymbolVariable(identifier.Name, type)).Cast<Symbol>());
        return parameters;
    }

    private List<Symbol> ParseVarParameter()
    {
        var identifierList = ParseIdentifierList();
        List<Symbol> parameters = new();

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

        parameters.AddRange(identifierList.Select(identifier => new SymbolVariable(identifier.Name, type)).Cast<Symbol>());
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
    
    private SymbolType ParseType()
    {
        var lexeme = _lexer.CurrentLexeme;
        switch (lexeme)
        {
            case IKeyWordLexeme {Value: KeyWordValue.Integer}:
                _lexer.GetLexeme();
                return new SymbolInteger("Integer");
            case IKeyWordLexeme {Value: KeyWordValue.Double}:
                _lexer.GetLexeme();
                return new SymbolDouble("Double");
            case IKeyWordLexeme {Value: KeyWordValue.String}:
                _lexer.GetLexeme();
                return new SymbolString("String");
            case IKeyWordLexeme {Value: KeyWordValue.Char}:    
                _lexer.GetLexeme();
                return new SymbolChar("Char");
            case IKeyWordLexeme{Value: KeyWordValue.Array}:
                return ParseArrayType();
            case IKeyWordLexeme{Value: KeyWordValue.Record}:
                return ParseRecordType();
            case IIdentifierLexeme identifierLexeme:
                _lexer.GetLexeme();
                var symbol = _stack.Get(identifierLexeme.Value);
                if (symbol is not IAlias alias)
                {
                    throw new CompilerException(_lexer.Coordinate + " " + identifierLexeme.Value + " isn't alias");
                }

                return alias.Original;
            default:
                throw new CompilerException(_lexer.Coordinate + " can't find type for this lexeme");
        }
    }

    private SymbolRecord ParseRecordType()
    {
        _lexer.GetLexeme();

        var fields = ParseRecordSections();

        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.End})
        {
            throw new CompilerException(_lexer.Coordinate + " end was expected");
        }
        _lexer.GetLexeme();
        
        return new SymbolRecord("Record", fields);
    }

    private SymbolTable ParseRecordSections()
    {
        var lexeme = _lexer.CurrentLexeme;
        OrderedDictionary fields = new();
        if (lexeme is not IIdentifierLexeme)
        {
            return new SymbolTable(fields);
        }

        var fieldsPart = ParseRecordSection();
        foreach (var field in fieldsPart)
        {
            fields.Add(field.Key, field.Value);
        }
        
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            _lexer.GetLexeme();
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIdentifierLexeme)
            {
                return new SymbolTable(fields);
            }
            fieldsPart = ParseRecordSection();
            foreach (var field in fieldsPart)
            {
                fields.Add(field.Key, field.Value);
            }
            lexeme = _lexer.CurrentLexeme;
        }

        return new SymbolTable(fields);
    }

    private List<Pair> ParseRecordSection()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIdentifierLexeme)
        {
            throw new CompilerException(_lexer.Coordinate + " identifier was expected");
        }
        
        var identifierList = ParseIdentifierList();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme {Value: OperatorValue.DoublePoint})
        {
            throw new CompilerException(_lexer.Coordinate + " : was expected");
        }
        _lexer.GetLexeme();

        var type = ParseType();

        return identifierList.Select(identifier => new Pair {Key = identifier.Name, Value = type}).ToList();
    }
    
    private SymbolArray ParseArrayType()
    {
        _lexer.GetLexeme();
                
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.SquareLeftBracket})
        {
            throw new CompilerException(_lexer.Coordinate + " [ was expected");
        }
        _lexer.GetLexeme();

        var bounds = ParseBounds();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.SquareRightBracket})
        {
            throw new CompilerException(_lexer.Coordinate + " ] was expected");
        }
        _lexer.GetLexeme();
                
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme{Value: KeyWordValue.Of})
        {
            throw new CompilerException(_lexer.Coordinate + " of was expected");
        }
        _lexer.GetLexeme();

        var type = ParseType();
        var lastElement = bounds.Count - 1;
        var preLastElement = bounds.Count - 2;
        var arrayType = new SymbolArray("", type, bounds[lastElement].LeftBound, bounds[lastElement].RightBound);
        for (var i = preLastElement; i >= 0; i--)
        {
            arrayType = new SymbolArray("Array", arrayType, bounds[i].LeftBound, bounds[i].RightBound);
        }

        return arrayType;
    }
    
    private List<IBound> ParseBounds()
    {
        List<IBound> indexes = new();
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIntegerLexeme leftBound)
        {
            throw new CompilerException(_lexer.Coordinate + " integer was expected");
        }
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme {Value: OperatorValue.Range})
        {
            throw new CompilerException(_lexer.Coordinate + " .. was expected");
        }
        _lexer.GetLexeme();
            
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIntegerLexeme rightBound)
        {
            throw new CompilerException(_lexer.Coordinate + " integer was expected");
        }
        _lexer.GetLexeme();
            
        lexeme = _lexer.CurrentLexeme;

        indexes.Add(new Bound(leftBound.Value, rightBound.Value));
        
        while(lexeme is ISeparatorLexeme{Value: SeparatorValue.Comma})
        {
            _lexer.GetLexeme();
            
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIntegerLexeme leftBoundCycle)
            {
                throw new CompilerException(_lexer.Coordinate + " integer was expected");
            }
            _lexer.GetLexeme();

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IOperatorLexeme {Value: OperatorValue.Range})
            {
                throw new CompilerException(_lexer.Coordinate + " .. was expected");
            }
            _lexer.GetLexeme();
            
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIntegerLexeme rightBoundCycle)
            {
                throw new CompilerException(_lexer.Coordinate + " integer was expected");
            }
            _lexer.GetLexeme();
            
            lexeme = _lexer.CurrentLexeme;

            indexes.Add(new Bound(leftBoundCycle.Value, rightBoundCycle.Value));
        }

        return indexes;
    }

    private List<INodeStatement> ParseStatementSequence()
    {
        var statementFirst = ParseStatement();
        List<INodeStatement> statements = new();
        if (statementFirst is not NullStatement)
        {
            statements.Add(statementFirst);
        }
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon})
        {
            _lexer.GetLexeme();
            var statement = ParseStatement();
            if (statement is not NullStatement)
            {
                statements.Add(statement);
            }
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
            _ => new NullStatement()
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
        
        lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            var expressionList = ParseExpressionList();
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
            {
                throw new CompilerException(_lexer.Coordinate + " ) was expected");
            }
            _lexer.GetLexeme();

            return new ProcedureStatement(identifier, expressionList);
        }

        _lexer.GetLexeme();
        return new ProcedureStatement(identifier, new List<INodeExpression>());
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
    
    private INodeExpression ParseExpression()
    {
        var leftSimpleExpression = ParseSimpleExpression();
        var lexeme = _lexer.CurrentLexeme;
        switch (lexeme)
        {
            case OperatorLexeme
            {
                Value: OperatorValue.Equal or OperatorValue.More or OperatorValue.Less or OperatorValue.MoreEqual
                or OperatorValue.LessEqual
            } operatorLexeme:
            {
                _lexer.GetLexeme();

                var rightSimpleExpression = ParseSimpleExpression();

                return new BinOperation(operatorLexeme.Value, leftSimpleExpression, rightSimpleExpression);
            } 
        }

        return leftSimpleExpression;

    }

    private INodeExpression ParseSimpleExpression()
    {
        var lexeme = _lexer.CurrentLexeme;
        var counter = 0;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus})
        {
            if (lexeme is IOperatorLexeme {Value: OperatorValue.Minus})
            {
                counter += 1;
            }
            _lexer.GetLexeme();
            lexeme = _lexer.CurrentLexeme;
        }

        INodeExpression left;
        if (counter > 0 && counter % 2 != 0)
        {
            left = new UnaryOperation(OperatorValue.Minus, ParseTerm());
        }
        else
        {
            left = ParseTerm();
        }
        
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Plus or OperatorValue.Minus})
        {
            counter = 0;
            while (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus})
            {
                if (lexeme is IOperatorLexeme {Value: OperatorValue.Minus})
                {
                    counter += 1;
                }
                _lexer.GetLexeme();
                lexeme = _lexer.CurrentLexeme;
            }

            OperatorValue operatorValue;
            if (counter > 0 && counter % 2 != 0)
            {
                operatorValue = OperatorValue.Minus;
            }
            else
            {
                operatorValue = OperatorValue.Plus;
            }
            left = new BinOperation(operatorValue, left, ParseTerm());
            lexeme = _lexer.CurrentLexeme;
        }

        while (lexeme is IKeyWordLexeme {Value: KeyWordValue.Or})
        {
            _lexer.GetLexeme();
            left = new BinOperation(OperatorValue.Or, left, ParseTerm());
            lexeme = _lexer.CurrentLexeme;
        }

        return left;
    }

    private INodeExpression ParseTerm()
    {
        var left = ParseFactor();
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Multiplication or OperatorValue.Div} | lexeme is IKeyWordLexeme{Value: KeyWordValue.And} | lexeme is SeparatorLexeme{Value: SeparatorValue.Point})
        {
            var counter = 0;
            switch (lexeme)
            {  
                case IKeyWordLexeme:
                    _lexer.GetLexeme();
                    lexeme = _lexer.CurrentLexeme;
                    while (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus} plusMinusOperator)
                    {
                        _lexer.GetLexeme();
                        if (plusMinusOperator.Value is OperatorValue.Minus)
                        {
                            counter += 1;
                        }
                        lexeme = _lexer.CurrentLexeme;
                    }

                    if (counter > 0 && counter % 2 != 0)
                    {
                        left = new BinOperation(OperatorValue.And, left, new UnaryOperation(OperatorValue.Minus, ParseFactor()));
                    }
                    else
                    {
                        left = new BinOperation(OperatorValue.And, left, ParseFactor());
                    }
                    break;
                case IOperatorLexeme operatorLexeme:
                    _lexer.GetLexeme();
                    lexeme = _lexer.CurrentLexeme;
                    while (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus} plusMinusOperator)
                    {
                        _lexer.GetLexeme();
                        if (plusMinusOperator.Value is OperatorValue.Minus)
                        {
                            counter += 1;
                        }
                        lexeme = _lexer.CurrentLexeme;
                    }

                    if (counter > 0 && counter % 2 != 0)
                    {
                        left = new BinOperation(operatorLexeme.Value, left, new UnaryOperation(OperatorValue.Minus, ParseFactor()));
                    }
                    else
                    {
                        left = new BinOperation(operatorLexeme.Value, left, ParseFactor());
                    }
                    break;
                case ISeparatorLexeme:
                {
                    _lexer.GetLexeme();
                
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not IIdentifierLexeme identifierLexeme)
                    {
                        throw  new CompilerException(_lexer.Coordinate + " identifier was expected");
                    }
                    _lexer.GetLexeme();

                    left = new RecordAccess(left, identifierLexeme.Value);
                    break;
                }
            }

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
            case IFloatLexeme floatLexeme:
                _lexer.GetLexeme();
                return new DoubleNumber(floatLexeme.Value);
            case IStringLexeme stringLexeme:
                _lexer.GetLexeme();
                return new String(stringLexeme.Value);
            case ICharLexeme charLexeme:
                _lexer.GetLexeme();
                return new Char(charLexeme.Value);
            case IIdentifierLexeme identifierLexeme:
                _lexer.GetLexeme();

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.Point})
                {
                    _lexer.GetLexeme();

                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not IIdentifierLexeme identifierLexemeRecord)
                        throw new CompilerException(_lexer.Coordinate + " identifier was expected");
                    _lexer.GetLexeme();
                    return new RecordAccess(new Variable(identifierLexeme.Value), identifierLexemeRecord.Value);
                }

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.SquareLeftBracket})
                {
                    _lexer.GetLexeme();
                    
                    var expressionList = ParseExpressionList();

                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.SquareRightBracket})
                    {
                        throw new CompilerException(_lexer.Coordinate + " ] was expected");
                    }
                    _lexer.GetLexeme();

                    return new Variable(identifierLexeme.Value, expressionList);
                }
                
                
                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
                    return new Variable(identifierLexeme.Value);
                _lexer.GetLexeme();

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    _lexer.GetLexeme();
                    return new Call(identifierLexeme.Value, new List<INodeExpression>());
                }
                
                List<INodeExpression> expressions = new() {ParseExpression()};

                lexeme = _lexer.CurrentLexeme;
                while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma})
                {
                    _lexer.GetLexeme();
                    expressions.Add(ParseExpression());
                    lexeme = _lexer.CurrentLexeme;
                }

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    throw new CompilerException(_lexer.Coordinate + " ) was expected");
                }
                _lexer.GetLexeme();

                return new Call(identifierLexeme.Value, expressions);
            default:
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
                {
                    throw new FactorException(_lexer.Coordinate + " factor was expected");
                }
                _lexer.GetLexeme();
                
                var expression = ParseExpression();
                
                var nextToken = _lexer.CurrentLexeme;
                if (nextToken is not ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    throw new CompilerException(_lexer.Coordinate + " ) was expected");
                }
                _lexer.GetLexeme();

                nextToken = _lexer.CurrentLexeme;
                if (nextToken is ISeparatorLexeme {Value: SeparatorValue.Point})
                {
                    _lexer.GetLexeme();
                    nextToken = _lexer.CurrentLexeme;
                    if (nextToken is not IIdentifierLexeme identifierLexeme)
                    {
                        throw new CompilerException(_lexer.Coordinate + " identifier was expected");
                    }
                    _lexer.GetLexeme();
                    
                    return new RecordAccess(expression, identifierLexeme.Value);
                }
                
                return expression;
        }
    }

    private List<INodeExpression> ParseExpressionList()
    {
        
        var expression = ParseExpression();
        List<INodeExpression> expressions = new() {expression};

        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma})
        {
            _lexer.GetLexeme();
            expressions.Add(ParseExpression());
            lexeme = _lexer.CurrentLexeme;
        }

        return expressions;
    }
}