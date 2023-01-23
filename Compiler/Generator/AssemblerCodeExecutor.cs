using System.Diagnostics;

namespace Compiler.Generator;

public class AssemblerCodeExecutor
{
    /*var process = new Process();
                    process.StartInfo.FileName = "cmd";
                    process.StartInfo.Arguments = "/K gcc -m32 -mconsole -o main Project/main.obj";
                    process.Start();
                    process.Kill();
                    process.StartInfo.Arguments = "/K main.exe";
                    process.Start();
                    */

    public void RunAssemblerCode(string name)
    {
        var onlyName = GetNameWithoutFormat(name);
        RunExe(onlyName);
    }
    
    public void GenerateFiles(string name)
    {
        var onlyName = GetNameWithoutFormat(name);
        File.WriteAllText("./generate.cmd",
            $"nasm -f win32 {onlyName}.asm\n" +
            $"gcc -m32 -mconsole {onlyName}.obj -o {onlyName}\n" +
            $"move ./{onlyName}.asm ./{onlyName}\n" +
            $"move ./{onlyName}.obj ./{onlyName}\n" +
            $"move ./{onlyName}.exe ./{onlyName}\n"
        );
        var process = new Process();
        process.StartInfo.FileName = "generate.cmd";
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        process.WaitForExit();
    }

    public string RunCodeFotTest(string name)
    {
        var onlyName = GetNameWithoutFormat(name);
        File.WriteAllText("./run.cmd",
            $"@cd ./{onlyName}\n" +
            $"@{onlyName}.exe\n"
        );
        var process = new Process();
        process.StartInfo.FileName = "run.cmd";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        process.WaitForExit();
        var stream = process.StandardOutput;
        var output = stream.ReadToEnd();
        return output;
    }

    private void RunExe(string onlyName)
    {
        File.WriteAllText("./run.cmd",
            $"@cd ./{onlyName}\n" +
            $"@{onlyName}.exe\n"
        );
        var process = new Process();
        process.StartInfo.FileName = "run.cmd";
        process.Start();
        process.WaitForExit();
    }

    private string GetNameWithoutFormat(string name)
    {
        var nameWithoutFormat = name.TakeWhile(lit => lit != '.').Aggregate("", (current, lit) => current + lit);
        return nameWithoutFormat;
    }
}