﻿using Compiler.Constants;
using Compiler.Exceptions;
using Compiler.Lexeme;
using Compiler.LexicalAnalyzerStateMachine;
using Compiler.Parser.Tree;
using Char = Compiler.Parser.Tree.Char;
using CompoundStatement = Compiler.Parser.Tree.CompoundStatement;
using String = Compiler.Parser.Tree.String;
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
        Variable? variable = null;
        
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is IKeyWordLexeme {Value: KeyWordValue.Program})
        {
            _lexer.GetLexeme();
            var identifier = RequireIdentifier();
            RequireSeparator(SeparatorValue.Semicolon);
            variable = new Variable(identifier.Value);
        }

        var declarations = ParseDeclarations();
        var mainBlock = ParseCompoundStatement();
        RequireSeparator(SeparatorValue.Point);

        return new Tree.Program(variable, declarations, mainBlock);
    }

    private List<INodeDeclaration> ParseDeclarations()
    {
        List<INodeDeclaration> declarations = new();
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is IKeyWordLexeme
               {
                   Value: KeyWordValue.Type or KeyWordValue.Const or KeyWordValue.Var or KeyWordValue.Function
                   or KeyWordValue.Procedure
               })
        {
            switch (lexeme)
            {
                case IKeyWordLexeme {Value: KeyWordValue.Type}:
                    _lexer.GetLexeme();
                    declarations.Add(ParseTypeDeclaration());
                    lexeme = _lexer.CurrentLexeme;
                    break;
                case IKeyWordLexeme {Value: KeyWordValue.Const}:
                    _lexer.GetLexeme();
                    declarations.Add(ParseConstDeclaration());
                    lexeme = _lexer.CurrentLexeme;
                    break;
                case IKeyWordLexeme{Value: KeyWordValue.Var}:
                    _lexer.GetLexeme();
                    declarations.Add(ParseVarDeclaration());
                    lexeme = _lexer.CurrentLexeme;
                    break;
                case IKeyWordLexeme {Value: KeyWordValue.Function}:
                    _lexer.GetLexeme();
                    declarations.Add(ParseFunctionDeclaration());
                    lexeme = _lexer.CurrentLexeme;
                    break;
                case IKeyWordLexeme{Value: KeyWordValue.Procedure}:
                    _lexer.GetLexeme();
                    declarations.Add(ParseProcedureDeclaration());
                    lexeme = _lexer.CurrentLexeme;
                    break;
            }   
        }

        return declarations;
    }

    private INodeDeclaration ParseTypeDeclaration()
    {
        ILexeme lexeme;
        var typesDeclarations = new List<TypeDeclarationPair>();
        do
        {
            var identifierLexeme = RequireIdentifier();
            RequireOperator(OperatorValue.Equal);
            var type = ParseType();
            RequireSeparator(SeparatorValue.Semicolon);
            
            var variable = new Variable(identifierLexeme.Value);
            TypeDeclarationPair typeDeclaration = new(){Identifier = variable, Type = type};
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

            var identifierLexeme = RequireIdentifier();
            RequireOperator(OperatorValue.DoublePoint);
            var type = ParseType();
            RequireOperator(OperatorValue.Equal);
            var expression = ParseExpression();
            RequireSeparator(SeparatorValue.Semicolon);

            var variable = new Variable(identifierLexeme.Value);
            ConstDeclarationData constData = new(){Identifier = variable, Type = type, Expression = expression};
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
            var identifierList = ParseIdentifierList();
            if (identifierList.Count == 0)
            {
                throw new CompilerException(_lexer.Coordinate + " identifier was expected");
            }
            RequireOperator(OperatorValue.DoublePoint);
            var type = ParseType();
            
            INodeExpression? expression = null;
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is IOperatorLexeme {Value: OperatorValue.Equal})
            {
                _lexer.GetLexeme();
                expression = ParseExpression();
            }
            
            RequireSeparator(SeparatorValue.Semicolon);
            varDeclarations.AddRange(identifierList.Select(variable => new VarDeclarationData() {Identifier = variable, Type = type, Expression = expression}));

            lexeme = _lexer.CurrentLexeme;
        } while (lexeme is IIdentifierLexeme);

        return new VarDeclaration(varDeclarations);
    }

    private INodeDeclaration ParseFunctionDeclaration()
    {
        var identifierLexeme = RequireIdentifier();
        var variable = new Variable(identifierLexeme.Value);
        RequireSeparator(SeparatorValue.LeftBracket);
        var parameters = ParseParameters();
        RequireSeparator(SeparatorValue.RightBracket);
        RequireOperator(OperatorValue.DoublePoint);
        var type = ParseType();
        RequireSeparator(SeparatorValue.Semicolon);
        var declarations = ParseDeclarations();
        var statement = ParseCompoundStatement();
        RequireSeparator(SeparatorValue.Semicolon);
        
        return new FunctionDeclaration(variable, parameters, type, declarations, statement);
    }

    private INodeDeclaration ParseProcedureDeclaration()
    {
        var identifierLexeme = RequireIdentifier();
        var variable = new Variable(identifierLexeme.Value);
        RequireSeparator(SeparatorValue.LeftBracket);
        var parameters = ParseParameters();
        RequireSeparator(SeparatorValue.RightBracket);
        RequireSeparator(SeparatorValue.Semicolon);
        var declarations = ParseDeclarations();
        var statement = ParseCompoundStatement();
        RequireSeparator(SeparatorValue.Semicolon);

        return new ProcedureDeclaration(variable, parameters, declarations, statement);
    }
    
    private List<Parameter> ParseParameters()
    {
        ILexeme lexeme;
        var counter = 0;
        List<Parameter> parametersResult = new();
        do
        {
            if (counter != 0)
            {
                _lexer.GetLexeme();
            }

            List<Parameter> parameters;
            lexeme = _lexer.CurrentLexeme;
            if (lexeme is KeyWordLexeme {Value: KeyWordValue.Var})
            {
                _lexer.GetLexeme();
                parameters = ParseVarParameter();
                if (parameters.Count == 0)
                {
                    throw new CompilerException(_lexer.Coordinate + " identifier was expected");
                }
            }
            else
            {
                parameters = ParseValueParameter();
                if (parameters.Count == 0 && counter == 0)
                {
                    return parametersResult;
                }
            }

            parametersResult.AddRange(parameters);
            lexeme = _lexer.CurrentLexeme;
            counter += 1;
        } while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon});

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
        
        RequireOperator(OperatorValue.DoublePoint);
        var type = ParseType();
        
        parameters.AddRange(identifierList.Select(identifier => new ValueParameter(identifier, type)));
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
        
        RequireOperator(OperatorValue.DoublePoint);
        var type = ParseType();

        parameters.AddRange(identifierList.Select(identifier => new VarParameter(identifier, type)));
        return parameters;
    }

    private List<Variable> ParseIdentifierList()
    {
        var lexeme = _lexer.CurrentLexeme;
        List<Variable> variables = new();
        if (lexeme is not IIdentifierLexeme identifierLexeme)
        {
            return variables;
        }
        _lexer.GetLexeme();
        
        variables.Add(new Variable(identifierLexeme.Value));
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Comma})
        {
            _lexer.GetLexeme();
            
            var identifierLexemeCycle = RequireIdentifier();
            
            variables.Add(new Variable(identifierLexemeCycle.Value));
            lexeme = _lexer.CurrentLexeme;
        }
        
        return variables;
    }
    
    private INodeType ParseType()
    {
        var lexeme = _lexer.CurrentLexeme;
        Type type;
        switch (lexeme)
        {
            case IKeyWordLexeme {Value: KeyWordValue.Integer}:
                _lexer.GetLexeme();
                type = new Type(KeyWordValue.Integer);
                break;
            case IKeyWordLexeme {Value: KeyWordValue.Double}:
                _lexer.GetLexeme();
                type = new Type(KeyWordValue.Double);
                break;
            case IKeyWordLexeme {Value: KeyWordValue.String}:
                _lexer.GetLexeme();
                type = new Type(KeyWordValue.String);
                break;
            case IKeyWordLexeme {Value: KeyWordValue.Char}:    
                _lexer.GetLexeme();
                type = new Type(KeyWordValue.Char);
                break;
            case IKeyWordLexeme{Value: KeyWordValue.Array}:
                _lexer.GetLexeme();
                return ParseArrayType();
            case IKeyWordLexeme{Value: KeyWordValue.Record}:
                _lexer.GetLexeme();
                return ParseRecordType();
            case IIdentifierLexeme identifierLexeme:
                _lexer.GetLexeme();
                return new IdentifierType(new Variable(identifierLexeme.Value));
            default:
                throw new CompilerException(_lexer.Coordinate + " can't find type for this lexeme");
        }

        return type;
    }

    private INodeType ParseRecordType()
    {
        var recordSections = ParseRecordSections();
        RequireKeyWord(KeyWordValue.End);
        
        return new RecordType(recordSections);
    }

    private List<INode> ParseRecordSections()
    {
        List<INode> fieldPart = new();
        ILexeme lexeme;
        var counter = 0;
        do
        {
            if (counter != 0)
            {
                _lexer.GetLexeme();
            }

            lexeme = _lexer.CurrentLexeme;
            if (lexeme is not IIdentifierLexeme)
            {
                return fieldPart;
            }
            
            var recordSection = ParseRecordSection();
            fieldPart.Add(recordSection);
            lexeme = _lexer.CurrentLexeme;
            counter += 1;
        } while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon});

        return fieldPart;
    }

    private INode ParseRecordSection()
    {
        var identifierList = ParseIdentifierList();
        RequireOperator(OperatorValue.DoublePoint);
        var type = ParseType();

        return new RecordSection(identifierList, type);
    }
    
    private INodeType ParseArrayType()
    {   
        RequireSeparator(SeparatorValue.SquareLeftBracket);
        var bounds = ParseBounds();
        RequireSeparator(SeparatorValue.SquareRightBracket);
        RequireKeyWord(KeyWordValue.Of);
        var type = ParseType();

        return new ArrayType(type, bounds);
    }
    
    private List<INode> ParseBounds()
    {
        List<INode> indexes = new();
        ILexeme lexeme;
        var counter = 0;
        do
        {
            if (counter != 0)
            {
                _lexer.GetLexeme();
            }
            
            var leftBound = RequireInteger();
            RequireOperator(OperatorValue.Range);
            var rightBound = RequireInteger();

            indexes.Add(new Bound(leftBound.Value, rightBound.Value));

            lexeme = _lexer.CurrentLexeme;
            counter += 1;
        } while (lexeme is ISeparatorLexeme{Value: SeparatorValue.Comma});

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
        var variable = new Variable(((IIdentifierLexeme) lexeme).Value);
        _lexer.GetLexeme();

        lexeme = _lexer.CurrentLexeme;
        return lexeme switch
        {
            IOperatorLexeme {Value: OperatorValue.Assignment} => ParseAssignmentStatement(variable),
            _ => ParseProcedureStatement(variable)
        };
    }

    private INodeStatement ParseProcedureStatement(Variable variable)
    {
        List<INodeExpression> expressionList = new(); 

        RequireSeparator(SeparatorValue.LeftBracket);
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not SeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            expressionList = ParseExpressionList();
        }
        RequireSeparator(SeparatorValue.RightBracket);

        return new ProcedureStatement(variable, expressionList);
        
    }

    private INodeStatement ParseAssignmentStatement(Variable variable)
    {
        RequireOperator(OperatorValue.Assignment);
        var expression = ParseExpression();
        return new AssignmentStatement(variable, expression);

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
        RequireKeyWord(KeyWordValue.Begin);
        var statements = ParseStatementSequence();
        RequireKeyWord(KeyWordValue.End);

        return new CompoundStatement(statements);
    }

    private INodeStatement ParseWhileStatement()
    {
        _lexer.GetLexeme();

        var expression = ParseExpression();
        RequireKeyWord(KeyWordValue.Do);
        var statement = ParseStatement();

        return new WhileStatement(expression, statement);
    }

    private INodeStatement ParseForStatement()
    {
        _lexer.GetLexeme();

        var identifierLexeme = RequireIdentifier();
        RequireOperator(OperatorValue.Assignment);
        var startExpression = ParseExpression();
        RequireKeyWord(KeyWordValue.To);
        var endExpression = ParseExpression();
        RequireKeyWord(KeyWordValue.Do);
        var statement = ParseStatement();
        
        var variable = new Variable(identifierLexeme.Value);
        
        return new ForStatement(variable, startExpression, endExpression, statement);
    }

    private INodeStatement ParseIfStatement()
    {
        _lexer.GetLexeme();

        INodeStatement? elsePart = null;
        
        var expression = ParseExpression();
        RequireKeyWord(KeyWordValue.Then);
        var statement = ParseStatement();
        
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme {Value: KeyWordValue.Else})
            return new IfStatement(expression, statement, elsePart);
        
        _lexer.GetLexeme();
        elsePart = ParseStatement();

        return new IfStatement(expression, statement, elsePart);
    }
    
    public INodeExpression ParseExpression()
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
        var sign = OperatorValue.Plus;
        if (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus})
        {
            sign = ConvertPlusMinusRow();
        }
        var left = sign == OperatorValue.Minus ? new UnaryOperation(OperatorValue.Minus, ParseTerm()) : ParseTerm();
        
        lexeme = _lexer.CurrentLexeme;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Plus or OperatorValue.Minus})
        {
            var operatorValue = ConvertPlusMinusRow();
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
            var sign = OperatorValue.Plus;
            switch (lexeme)
            {
                case IKeyWordLexeme or IOperatorLexeme:
                {
                    var operatorLexeme = lexeme;
                    _lexer.GetLexeme();
                    lexeme = _lexer.CurrentLexeme;
                    if (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus})
                    {
                        sign = ConvertPlusMinusRow();
                    }

                    var operatorValue = OperatorValue.And;
                    if (operatorLexeme is IOperatorLexeme)
                    {
                        operatorValue = ((IOperatorLexeme)operatorLexeme).Value;
                    }
                
                    left = sign == OperatorValue.Minus ? new BinOperation(operatorValue, left, new UnaryOperation(OperatorValue.Minus, ParseFactor())) : new BinOperation(operatorValue, left, ParseFactor());
                    break;
                }
                case ISeparatorLexeme:
                {
                    _lexer.GetLexeme();
                    var identifierLexeme = RequireIdentifier();
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
                    var identifierLexemeRecord = RequireIdentifier();
                    return new RecordAccess(new Variable(identifierLexeme.Value), identifierLexemeRecord.Value);
                }

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.SquareLeftBracket})
                {
                    _lexer.GetLexeme();
                    var expressionList = ParseExpressionList();
                    RequireSeparator(SeparatorValue.SquareRightBracket);
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
                RequireSeparator(SeparatorValue.RightBracket);
                return new Call(identifierLexeme.Value, expressions);
            default:
                try
                {
                    RequireSeparator(SeparatorValue.LeftBracket);
                }
                catch (CompilerException)
                {
                    throw new FactorException(_lexer.Coordinate + " factor was expected");
                }
                var expression = ParseExpression();
                RequireSeparator(SeparatorValue.RightBracket);

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.Point})
                {
                    _lexer.GetLexeme();
                    var identifierLexeme = RequireIdentifier();
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

    private void RequireSeparator(SeparatorValue separatorValue)
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not ISeparatorLexeme separatorLexeme)
            throw new CompilerException(_lexer.Coordinate + " " + LexemesSeparators.SeparatorSymbols[separatorValue] + " was expected");
        
        if (separatorLexeme.Value != separatorValue)
        {
            throw new CompilerException(_lexer.Coordinate + " " + LexemesSeparators.SeparatorSymbols[separatorValue] + " was expected");
        }

        _lexer.GetLexeme();
    }

    private void RequireOperator(OperatorValue operatorValue)
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IOperatorLexeme operatorLexeme)
            throw new CompilerException(_lexer.Coordinate + " " + OperatorConstants.OperatorSymbols[operatorValue] + " was expected");
        
        if (operatorLexeme.Value != operatorValue)
        {
            throw new CompilerException(_lexer.Coordinate + " " + OperatorConstants.OperatorSymbols[operatorValue] + " was expected");
        }

        _lexer.GetLexeme();
    }

    private void RequireKeyWord(KeyWordValue keyWordValue)
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IKeyWordLexeme keyWordLexeme)
            throw new CompilerException(_lexer.Coordinate + " " + KeyWordsConstants.KeyWordStrings[keyWordValue] + " was expected");
        
        if (keyWordLexeme.Value != keyWordValue)
        {
            throw new CompilerException(_lexer.Coordinate + " " + KeyWordsConstants.KeyWordStrings[keyWordValue] + " was expected");
        }

        _lexer.GetLexeme();
    }

    private IIdentifierLexeme RequireIdentifier()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIdentifierLexeme identifierLexeme)
            throw new CompilerException(_lexer.Coordinate + " identifier was expected");
        _lexer.GetLexeme();

        return identifierLexeme;
    }

    private IIntegerLexeme RequireInteger()
    {
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not IIntegerLexeme integerLexeme)
            throw new CompilerException(_lexer.Coordinate + " integer was expected");
        _lexer.GetLexeme();

        return integerLexeme;
    }

    private OperatorValue ConvertPlusMinusRow()
    {
        var counter = 0;
        var lexeme = _lexer.CurrentLexeme;
        while (lexeme is IOperatorLexeme {Value: OperatorValue.Minus or OperatorValue.Plus})
        {
            if (lexeme is IOperatorLexeme {Value: OperatorValue.Minus})
            {
                counter += 1;
            }
            _lexer.GetLexeme();
            lexeme = _lexer.CurrentLexeme;
        }

        if (counter > 0 && counter % 2 != 0)
        {
            return OperatorValue.Minus;
        }

        return OperatorValue.Plus;
    }
}