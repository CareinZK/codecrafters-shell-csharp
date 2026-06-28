
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCrafters.Shell.src;

public class AutoCompletionHandler : IAutoCompleteHandler
{
    public char[] Separators { get; set; } = new[] { ' ' };

    public string[] GetSuggestions(string text, int index)
    {
        return new[]
        {
        "echo",
        "exit"
    };
    }
}
