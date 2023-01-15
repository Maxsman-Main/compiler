    namespace Compiler.Generator;

public class AssemblerFileMaker
{
    public void MakeFile(string name, IEnumerable<string> commands)
    {
        var nameWithoutFormat = name.TakeWhile(lit => lit != '.').Aggregate("", (current, lit) => current + lit);
        var result = commands.Aggregate("", (current, command) => current + (command + "\n"));
        Console.WriteLine(result);
        var file = File.Create($"./{name}");
        file.Close();
        var fileWriter = new StreamWriter($"./{name}");
        fileWriter.Write(result);
        fileWriter.Close();
        File.Move($"./{name}", $"./{nameWithoutFormat}.asm", true);
    }
}