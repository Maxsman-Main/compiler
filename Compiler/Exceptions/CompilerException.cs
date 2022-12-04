namespace Compiler.Exceptions;

public class CompilerException : Exception
{
    protected CompilerException() { }

    public CompilerException(string message)
        : base(message) { }

    protected CompilerException(string message, Exception inner)
        : base(message, inner) { }
}

public class EmptyBlockException : CompilerException
{
    public EmptyBlockException(string message)
        : base(message) { }
}

public class EmptyArgumentsException : CompilerException
{
    public EmptyArgumentsException(string message):
        base(message){}
}