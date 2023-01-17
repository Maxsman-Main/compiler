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

public struct Program
{
    public SymbolTableStack Stack;
    public INodeStatement MainBlock;
    public INode SyntaxTree;
}

public class Parser
{
    private readonly LexicalAnalyzer _lexer;
    private readonly SymbolTableStack _stack;

    public Parser(LexicalAnalyzer lexer)
    {
        _lexer = lexer;
        _lexer.GetLexeme(); 
        _stack = new SymbolTableStack();
    }

    public Program ParseProgram()
    {
        Variable? variable = null;
        
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is IKeyWordLexeme {Value: KeyWordValue.Program})
        {
            _lexer.GetLexeme();
            var identifier = RequireIdentifier();
            RequireSeparator(SeparatorValue.Semicolon);
            variable = new Variable(identifier.Value);

            var programVariable = new SymbolVariable(identifier.Value, new SymbolProgram("Program"), null);
            _stack.Add(identifier.Value, programVariable);
        }

        var declarations = ParseDeclarations();
        var mainBlock = ParseCompoundStatement();
        RequireSeparator(SeparatorValue.Point);

        var syntaxTree = new Tree.Program(variable, declarations, mainBlock);

        var program = new Program() {Stack = _stack, MainBlock = mainBlock, SyntaxTree = syntaxTree};
        
        return program;
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
            _stack.Add(variable.Name, new SymbolTypeAlias(variable.Name, type));

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
            var symbolVariable = new SymbolVariable(variable.Name, type, expression);
            CheckVariableTypeDeclaration(symbolVariable, expression);
            _stack.Add(variable.Name, symbolVariable);
            
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
            foreach (var identifier in identifierList)
            {
                var variable = new SymbolVariable(identifier.Name, type, expression);
                CheckVariableTypeDeclaration(variable, expression);
                _stack.Add(identifier.Name, variable);
            }
            
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
        var parametersTable = CreateTableByParameters(parameters);
        RequireSeparator(SeparatorValue.RightBracket);
        RequireOperator(OperatorValue.DoublePoint);
        var type = ParseType();
        RequireSeparator(SeparatorValue.Semicolon);
        _stack.Push(new SymbolTable());
        var declarations = ParseDeclarations();
        var locals = _stack.Pop();
        var variables = parametersTable.Merge(locals);
        _stack.Push(variables);
        var statement = ParseCompoundStatement();
        RequireSeparator(SeparatorValue.Semicolon);
        RequireKeyWord(KeyWordValue.Return);
        var returnExpression = ParseExpression();
        RequireSeparator(SeparatorValue.Semicolon);
        _stack.Pop();

        var function = new SymbolFunction(variable.Name, parametersTable, locals, statement, type, returnExpression, _stack);
        _stack.Add(variable.Name, function);
        
        return new FunctionDeclaration(variable, parameters, type, declarations, statement);
    }

    private INodeDeclaration ParseProcedureDeclaration()
    {
        var identifierLexeme = RequireIdentifier();
        var variable = new Variable(identifierLexeme.Value);
        RequireSeparator(SeparatorValue.LeftBracket);
        var parameters = ParseParameters();
        var parametersTable = CreateTableByParameters(parameters);
        RequireSeparator(SeparatorValue.RightBracket);
        RequireSeparator(SeparatorValue.Semicolon);
        _stack.Push(new SymbolTable());
        var declarations = ParseDeclarations();
        var locals = _stack.Pop();
        var variables = parametersTable.Merge(locals);
        _stack.Push(variables);
        var statement = ParseCompoundStatement();
        RequireSeparator(SeparatorValue.Semicolon);
        _stack.Pop();

        var procedure = new SymbolProcedure(variable.Name, parametersTable, locals, statement, _stack);
        _stack.Add(variable.Name, procedure);
        
        return new ProcedureDeclaration(variable, parameters, declarations, statement);
    }

    private SymbolTable CreateTableByParameters(List<Parameter> parameters)
    {
        var table = new SymbolTable();
        foreach (var parameter in parameters)
        {
            var variable = new SymbolVariable(parameter.Identifier.Name, parameter.Type, null);
            if (parameter is VarParameter)
            {
                variable = new SymbolVarVariable(parameter.Identifier.Name, parameter.Type, null);
            }
            
            table.Add(parameter.Identifier.Name, variable);
        }
        
        return table;
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
    
    private SymbolType ParseType()
    {
        var lexeme = _lexer.CurrentLexeme;
        SymbolType type;
        switch (lexeme)
        {
            case IKeyWordLexeme {Value: KeyWordValue.Integer}:
                _lexer.GetLexeme();
                type = new SymbolInteger("Integer");
                break;
            case IKeyWordLexeme {Value: KeyWordValue.Double}:
                _lexer.GetLexeme();
                type = new SymbolDouble("Double");
                break;
            case IKeyWordLexeme {Value: KeyWordValue.String}:
                _lexer.GetLexeme();
                type = new SymbolString("String");
                break;
            case IKeyWordLexeme {Value: KeyWordValue.Char}:    
                _lexer.GetLexeme();
                type = new SymbolChar("Char");
                break;
            case IKeyWordLexeme{Value: KeyWordValue.Array}:
                _lexer.GetLexeme();
                return ParseArrayType();
            case IKeyWordLexeme{Value: KeyWordValue.Record}:
                _lexer.GetLexeme();
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

        return type;
    }

    private SymbolRecord ParseRecordType()
    {
        var fields = ParseRecordSections();
        RequireKeyWord(KeyWordValue.End);
        
        return new SymbolRecord("Record", fields);
    }

    private SymbolTable ParseRecordSections()
    {
        var table = new SymbolTable();
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
                return table;
            }
            
            var fieldsPart = ParseRecordSection();
            foreach (var field in fieldsPart)
            {
                table.Add(field.Key, field.Value);
            }
            lexeme = _lexer.CurrentLexeme;
            counter += 1;
        } while (lexeme is ISeparatorLexeme {Value: SeparatorValue.Semicolon});

        return table;
    }

    private IEnumerable<Pair> ParseRecordSection()
    {
        var identifierList = ParseIdentifierList();
        RequireOperator(OperatorValue.DoublePoint);
        var type = ParseType();

        return (from identifier in identifierList let variable = new SymbolVariable(identifier.Name, type, null) select new Pair() {Key = identifier.Name, Value = variable}).ToList();
    }
    
    private SymbolArray ParseArrayType()
    {   
        RequireSeparator(SeparatorValue.SquareLeftBracket);
        var bounds = ParseBounds();
        RequireSeparator(SeparatorValue.SquareRightBracket);
        RequireKeyWord(KeyWordValue.Of);
        var type = ParseType();

        return CreateArrayByBounds(bounds, type);
    }

    private List<IBound> ParseBounds()
    {
        List<IBound> indexes = new();
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

    private SymbolArray CreateArrayByBounds(IReadOnlyList<IBound> bounds, SymbolType type)
    {
        var lastElementOfArray = bounds.Count - 1;
        var preLastElementOfArray = bounds.Count - 2;
        var arrayType = new SymbolArray("array", type, bounds[lastElementOfArray].LeftBound,
            bounds[lastElementOfArray].RightBound);

        for (var i = preLastElementOfArray; i >= 0; i--)
        {
            arrayType = new SymbolArray("array", arrayType, bounds[i].LeftBound, bounds[i].RightBound);
        }

        return arrayType;
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
            IKeyWordLexeme{Value: KeyWordValue.Write} =>  ParseWriteStatement(),
            IKeyWordLexeme {Value: KeyWordValue.Begin or KeyWordValue.While or KeyWordValue.For or KeyWordValue.If} =>
                ParseStructuredStatement(),
            _ => new NullStatement()
        };
    }

    private INodeStatement ParseWriteStatement()
    {
        List<INodeExpression>? parameter = null;
        
        _lexer.GetLexeme();
        RequireSeparator(SeparatorValue.LeftBracket);
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not SeparatorLexeme{Value: SeparatorValue.RightBracket})
        {
            parameter = ParseExpressionList();
        }
        RequireSeparator(SeparatorValue.RightBracket);

        return new WriteStatement(parameter);
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

        var procedure = GetSymbolProcedure(variable.Name);
        
        RequireSeparator(SeparatorValue.LeftBracket);
        var lexeme = _lexer.CurrentLexeme;
        if (lexeme is not SeparatorLexeme {Value: SeparatorValue.RightBracket})
        {
            expressionList = ParseExpressionList();
        }
        RequireSeparator(SeparatorValue.RightBracket);

        
        CheckProcedureCallAccuracy(procedure, expressionList);
 
        return new ProcedureStatement(procedure, expressionList);
    }

    private INodeStatement ParseAssignmentStatement(Variable variable)
    {
        var variableSymbol = GetSymbolVariable(variable.Name);
        RequireOperator(OperatorValue.Assignment);
        var expression = ParseExpression();
        CheckAssigmentAccuracy(variableSymbol, ref expression);
        return new AssignmentStatement(variableSymbol, expression);
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
        CheckConditionAccuracy(expression, KeyWordValue.While);

        return new WhileStatement(expression, statement);
    }

    private INodeStatement ParseForStatement()
    {
        _lexer.GetLexeme();

        var identifierLexeme = RequireIdentifier();
        var variableSymbol = GetSymbolVariable(identifierLexeme.Value);
        RequireOperator(OperatorValue.Assignment);
        var startExpression = ParseExpression();
        CheckConditionAccuracy(startExpression, KeyWordValue.For);
        RequireKeyWord(KeyWordValue.To);
        var endExpression = ParseExpression();
        CheckConditionAccuracy(endExpression, KeyWordValue.For);
        RequireKeyWord(KeyWordValue.Do);
        var statement = ParseStatement();

        var variable = new Variable(variableSymbol);
        
        return new ForStatement(variable, startExpression, endExpression, statement);
    }

    private INodeStatement ParseIfStatement()
    {
        _lexer.GetLexeme();

        INodeStatement? elsePart = null;
        
        var expression = ParseExpression();
        CheckConditionAccuracy(expression, KeyWordValue.If);
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
                    CheckExpressionTypeRecord(left);
                    var record = left.GetExpressionType() as SymbolRecord;
                    var recordField = GetRecordField(record, identifierLexeme.Value);
                    left = new RecordAccess(left, recordField);
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
                    var variableSymbol = GetSymbolVariable(identifierLexeme.Value);
                    CheckVariableIsRecord(variableSymbol);
                    var variable = new Variable(variableSymbol);
                    var field = GetRecordField(variableSymbol.Type as SymbolRecord, identifierLexemeRecord.Value);
                    return new RecordAccess(variable, field);
                }

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.SquareLeftBracket})
                {
                    _lexer.GetLexeme();
                    var expressionList = ParseExpressionList();
                    RequireSeparator(SeparatorValue.SquareRightBracket);
                    var variable = GetSymbolVariable(identifierLexeme.Value);
                    return new Variable(variable, expressionList);
                }

                lexeme = _lexer.CurrentLexeme;
                if (lexeme is not ISeparatorLexeme {Value: SeparatorValue.LeftBracket})
                {
                    var variable = GetSymbolVariable(identifierLexeme.Value);
                    return new Variable(variable);
                }
                _lexer.GetLexeme();

                lexeme = _lexer.CurrentLexeme;
                SymbolProcedure procedure;
                if (lexeme is ISeparatorLexeme {Value: SeparatorValue.RightBracket})
                {
                    _lexer.GetLexeme();
                    procedure = GetSymbolProcedure(identifierLexeme.Value);
                    CheckProcedureCallAccuracy(procedure, new List<INodeExpression>());
                    return new Call(procedure, new List<INodeExpression>());
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
                procedure = GetSymbolProcedure(identifierLexeme.Value);
                CheckProcedureCallAccuracy(procedure, expressions);
                return new Call(procedure, expressions);
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
                    CheckExpressionTypeRecord(expression);
                    var record = expression.GetExpressionType() as SymbolRecord;
                    var field = GetRecordField(record, identifierLexeme.Value);
                    return new RecordAccess(expression, field);
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

    private SymbolProcedure GetSymbolProcedure(string name)
    {
        var symbol = _stack.Get(name);
        var procedure = symbol as SymbolProcedure ?? throw new CompilerException(name + " isn't procedure");
        return procedure;
    }

    private SymbolVariable GetSymbolVariable(string name)
    {
        var symbol = _stack.Get(name);
        var variable = symbol as SymbolVariable ?? throw new CompilerException(name + " isn't variable");
        return variable;
    }

    private void CheckVariableIsRecord(SymbolVariable variable)
    {
        if (variable.Type is not SymbolRecord)
        {
            throw new CompilerException(variable.Name + " isn't record");
        }
    }
    
    private SymbolVariable GetRecordField(SymbolRecord record, string name)
    {
        var symbol = record.Fields.Get(name);
        var field = symbol as SymbolVariable ?? throw new CompilerException(name + " isn't record field");
        return field;
    }

    private void CheckVariableTypeDeclaration(SymbolVariable variable, INodeExpression? expression)
    {
        if (expression is null)
        {
            return;
        }
        var variableType = variable.Type;
        var expressionType = expression.GetExpressionType();
        if (variableType.GetType() == expressionType.GetType()) return;
        throw new CompilerException(variable.Name + " can't initialize " + expressionType.Name + " type");
    }

    private void CheckProcedureCallAccuracy(SymbolProcedure procedure, IReadOnlyList<INodeExpression> expressions)
    {
        if (procedure.Parameters.Count != expressions.Count)
        {
            throw new CompilerException( procedure.Name + " procedure has " + procedure.Parameters.Count + " parameters, but " +
                                         expressions.Count + " received");
        }
        
        for (var i = 0; i < expressions.Count; i++)
        {
            var expressionType = expressions[i].GetExpressionType();
            var parameterType = (procedure.Parameters.Data[i] as SymbolVariable)?.Type;
            if (expressionType.GetType() != parameterType?.GetType())
            {
                throw new CompilerException(procedure.Name + " parameter type is " + parameterType?.Name + " but " +
                                            expressionType.Name + " received");
            }
        }
    }

    private void CheckAssigmentAccuracy(SymbolVariable variable, ref INodeExpression expression)
    {
        var variableType = variable.Type;
        var expressionType = expression.GetExpressionType();
        if (variableType.GetType() == expressionType.GetType()) return;
        if (variableType is SymbolDouble && expressionType is SymbolInteger)
        {
            expression = new CastToDouble(expression);
        }
        else
        {
            throw new CompilerException(variable.Name + " can't assigment " + expressionType.Name + " type");
        }
    }

    private void CheckConditionAccuracy(INodeExpression expression, KeyWordValue cycleName)
    {
        if (expression.GetExpressionType() is not SymbolInteger)
        {
            throw new CompilerException(KeyWordsConstants.KeyWordStrings[cycleName] + " condition can't be not integer");
        }
    }

    private void CheckExpressionTypeRecord(INodeExpression expression)
    {
        if (expression.GetExpressionType() is not SymbolRecord)
        {
            throw new CompilerException("left part in record access isn't record");
        }
    }
}