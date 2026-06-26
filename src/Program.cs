using System.Diagnostics;
class Program
{
    static void Main()
    {
        var builtinCommands = new List<string>()
        {
            "exit",
            "echo",
            "type"
        };
        bool isWorking = true;
        string? pathVariable = Environment.GetEnvironmentVariable("PATH");

        while (isWorking)
        {
            Console.Write("$ ");
            string input = Console.ReadLine();
            var splitInput = input.Split(" ");
            string argument = string.Join(" ", splitInput.Skip(1));
            string command = input.Split(" ").First();
            string file = IsExistent(pathVariable, argument);

            switch (command)
            {
                case "exit":
                    isWorking = false;
                break;
                case "echo":
                    Console.WriteLine(argument);
                    break;
                case "type":
                    if (builtinCommands.Contains(argument))
                    {
                        Console.WriteLine($"{argument} is a shell builtin");
                        break;
                    }


                    if (pathVariable is null)
                    {
                        Console.WriteLine($"{argument}: not found");
                        break;
                    }

                    

                    if (file is not null && IsExecutable(file))
                    {
                        Console.WriteLine($"{argument} is {file}");
                        break;
                    }
                    Console.WriteLine($"{argument}: not found");
                    break;

                default:
    
                    if (file is not null && IsExecutable(file))
                    {
                        Process.Start(file);
                    }
                    break;

            }

            static bool IsExecutable(string path)
            {
                var mode = File.GetUnixFileMode(path);

                return (mode & (UnixFileMode.UserExecute |
                                UnixFileMode.GroupExecute |
                                UnixFileMode.OtherExecute)) != 0;
            }

           static string? IsExistent(string pathVariable, string argument)
            {
                string file = null;
                foreach (string directory in pathVariable.Split(Path.PathSeparator))
                {
                    string fullPath = Path.Combine(directory, argument);

                    if (!File.Exists(fullPath))
                        continue;

                    file = fullPath;
                    break;
                }
                return file;
            }
        }

    }
}
