
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

        while (isWorking)
        {
            Console.Write("$ ");
            string input = Console.ReadLine();
            var splitInput = input.Split(" ");
            string argument = string.Join(" ", splitInput.Skip(1));
            string command = input.Split(" ").First();

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

                    string? pathVariable = Environment.GetEnvironmentVariable("PATH");

                    if (pathVariable is null)
                    {
                        Console.WriteLine($"{argument}: not found");
                        break;
                    }

                    bool found = false;

                    foreach (string directory in pathVariable.Split(Path.PathSeparator))
                    {
                        string fullPath = Path.Combine(directory, argument);

                        if (!File.Exists(fullPath))
                            continue;

                        // Linux / macOS                     
                        if (!IsExecutable(fullPath))
                            continue;

                        Console.WriteLine($"{argument} is {fullPath}");
                        found = true;
                        break;
                    }

                    if (!found)
                        Console.WriteLine($"{argument}: not found");

                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;

            }

            static bool IsExecutable(string path)
            {
                var mode = File.GetUnixFileMode(path);

                return (mode & (UnixFileMode.UserExecute |
                                UnixFileMode.GroupExecute |
                                UnixFileMode.OtherExecute)) != 0;
            }
        }

    }
}
