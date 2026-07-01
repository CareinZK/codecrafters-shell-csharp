using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCrafters.Shell.src;

public static class AutoCompletionHandler
{
    public static string? ReadLineWithAutocomplete(string prompt, List<string> autoCompleteCommands)
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
}

