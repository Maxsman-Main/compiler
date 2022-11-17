namespace Compiler.Parser.Tree;

public abstract class Node
{
    public abstract int Calc();
    public abstract string GetPrint(int level);
}