using System.Diagnostics;
using System.IO;
class Program
{
    static void Main()
    {
        var builtinCommands = new List<string>()
        {
            "exit",
            "echo",
            "type",
            "pwd",
            "cd"
        };
        bool isWorking = true;
        string? pathVariable = Environment.GetEnvironmentVariable("PATH");

        while (isWorking)
        {
            Console.Write("$ ");
            Console.Out.Flush();
            string? input = Console.ReadLine();
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
                case "pwd":
                    Console.WriteLine(Directory.GetCurrentDirectory());
                    break;
                case "cd":
                    if (argument == "~")
                    {
                        argument = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    }
                    if (Directory.Exists(argument))
                    {
                        Directory.SetCurrentDirectory(argument); // automatically handles both absolute and relative paths
                    }
                    else
                    Console.WriteLine($"cd: {argument}: No such file or directory");
                    break;
                case "type":
                    if (builtinCommands.Contains(argument))
                    {
                        Console.WriteLine($"{argument} is a shell builtin");
                        break;
                    }
                    string? fileType = FindExecutable(pathVariable, argument);

                    if (fileType is not null)
                    {
                        Console.WriteLine($"{argument} is {fileType}");
                    }
                    else
                    {
                        Console.WriteLine($"{argument}: not found");
                    }
                    break;

                default:
                    string? file = FindExecutable(pathVariable, command);

                    if (file is not null)
                    {
                        using Process process = Process.Start(new ProcessStartInfo
                        {
                            FileName = command,
                            WorkingDirectory = Path.GetDirectoryName(file)!,
                            Arguments = argument,
                            UseShellExecute = false
                        })!;
                        process.WaitForExit();
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"{command}: not found");
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

            static string? FindExecutable(string pathVariable, string command)
            {
                foreach (string directory in pathVariable.Split(Path.PathSeparator))
                {
                    string fullPath = Path.Combine(directory, command);

                    if (!File.Exists(fullPath))
                        continue;

                    if (!IsExecutable(fullPath))
                        continue;

                    return fullPath;
                }

                return null;
            }
        }
        }
}
