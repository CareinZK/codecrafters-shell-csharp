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
                        Console.WriteLine($"{argument} is a shell builtin");
                    else 
                        Console.WriteLine($"{argument}: not found");
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;

            }

        }

    }
}
