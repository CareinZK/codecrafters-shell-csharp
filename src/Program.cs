using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    private static readonly HashSet<string> BuiltinCommands = new(StringComparer.Ordinal)
{
"exit",
"echo",
"type",
"pwd",
"cd"
};

    private static readonly List<string> AutoCompleteCommands = new()
{
    "echo",
    "exit"
};

    static void Main()
    {
        bool isWorking = true;
        string? pathVariable = Environment.GetEnvironmentVariable("PATH");

        while (isWorking)
        {
            string? input = ReadLineWithAutocomplete("$ ", AutoCompleteCommands);

            if (input is null)
            {
                break;
            }

            if (input.Length == 0)
            {
                continue;
            }

            string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                continue;
            }

            string command = parts[0];
            string argument = parts.Length > 1 ? parts[1] : string.Empty;

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
                    if (argument == "~" || string.IsNullOrEmpty(argument))
                    {
                        argument = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    }

                    if (Directory.Exists(argument))
                    {
                        Directory.SetCurrentDirectory(argument);
                    }
                    else
                    {
                        Console.WriteLine($"cd: {argument}: No such file or directory");
                    }
                    break;

                case "type":
                    if (string.IsNullOrEmpty(argument))
                    {
                        break;
                    }

                    if (BuiltinCommands.Contains(argument))
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
                        using Process? process = Process.Start(new ProcessStartInfo
                        {
                            FileName = command,
                            Arguments = argument,
                            UseShellExecute = false
                        });

                        process?.WaitForExit();
                    }
                    else
                    {
                        Console.WriteLine($"{command}: not found");
                    }
                    break;


            }
        }
    }

    private static string? ReadLineWithAutocomplete(string prompt, List<string> autoCompleteCommands)
    {
        Console.Write(prompt);

        var buffer = new StringBuilder();

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return buffer.ToString();
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length > 0)
                {
                    buffer.Length--;
                    Console.Write("\b \b");
                }

                continue;
            }

            if (keyInfo.Key == ConsoleKey.Tab)
            {
                TryAutocomplete(buffer, autoCompleteCommands);
                continue;
            }

            char ch = keyInfo.KeyChar;
            if (!char.IsControl(ch))
            {
                buffer.Append(ch);
                Console.Write(ch);
            }
        }
    }

    private static void TryAutocomplete(StringBuilder buffer, List<string> autoCompleteCommands)
    {
        string current = buffer.ToString();

        int firstSpaceIndex = current.IndexOf(' ');
        string prefix = firstSpaceIndex >= 0 ? current[..firstSpaceIndex] : current;

        if (string.IsNullOrEmpty(prefix))
        {
            return;
        }

        List<string> matches = autoCompleteCommands
            .Where(command => command.StartsWith(prefix, StringComparison.Ordinal))
            .ToList();

        if (matches.Count != 1)
        {
            return;
        }

        string completion = matches[0];

        if (prefix == completion)
        {
            if (current.Length == completion.Length)
            {
                buffer.Append(' ');
                Console.Write(' ');
            }

            return;
        }

        string suffix = completion.Substring(prefix.Length);
        buffer.Append(suffix);
        Console.Write(suffix);

        buffer.Append(' ');
        Console.Write(' ');
    }

    private static string? FindExecutable(string? pathVariable, string command)
    {
        if (string.IsNullOrEmpty(pathVariable))
        {
            return null;
        }

        foreach (string directory in pathVariable.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
        {
            string fullPath = Path.Combine(directory, command);

            if (!File.Exists(fullPath))
            {
                continue;
            }

            if (!IsExecutable(fullPath))
            {
                continue;
            }

            return fullPath;
        }

        return null;
    }

    private static bool IsExecutable(string path)
    {
        try
        {
            UnixFileMode mode = File.GetUnixFileMode(path);

            return (mode & (UnixFileMode.UserExecute |
                            UnixFileMode.GroupExecute |
                            UnixFileMode.OtherExecute)) != 0;
        }
        catch
        {
            return false;
        }
    }



    public class Trie
    {

        private class TrieNode()
        {
            public Dictionary<char, TrieNode> children { get; } = new Dictionary<char, TrieNode>();
            public bool isEnd { get; set; }
        }

        private TrieNode _root;
        public Trie()
        {
            _root = new TrieNode();
        }

        public void Insert(string word)
        {
            var current = _root;

            foreach (var ch in word)
            {
                if (!current.children.ContainsKey(ch))
                {
                    current.children[ch] = new TrieNode();
                }
                current = current.children[ch];
            }
            current.isEnd = true;
        }

        public bool Search(string word)
        {
            var trie = FindNode(word);
            return trie != null && trie.isEnd;
        }

        public bool StartsWith(string prefix)
        {
            var trie = FindNode(prefix);
            return trie != null;
        }

        private TrieNode? FindNode(string word)
        {
            var current = _root;

            foreach (var ch in word)
            {
                if (!current.children.ContainsKey(ch))
                {
                    return null;
                }
                current = current.children[ch];
            }
            return current;
        }
    }
}
