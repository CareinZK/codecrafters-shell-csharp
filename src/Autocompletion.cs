/*
using System;
using System.Collections.Generic;
using System.Text;
using ReadLine;

namespace CodeCrafters.Shell.src;

public class AutoCompletionHandler : IAutoCompleteHandler
{
    private static readonly string[] Commands = { "echo", "exit" };

    public char[] Separators { get; set; } = new[] { ' ' };

    public string[] GetSuggestions(string text, int index)
    {
        string currentWord;

        if (index < 0 || index >= text.Length)
            currentWord = text;
        else
            currentWord = text[(index + 1)..];

        return Commands
            .Where(command => command.StartsWith(currentWord, StringComparison.Ordinal))
            .ToArray();
    }
}
*/