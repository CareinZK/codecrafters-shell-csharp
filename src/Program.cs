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
            string command = splitInput[0];

            switch (command)
            {
                case "exit":
                    isWorking = false;
                break;
                case "echo":
                    Console.WriteLine(string.Join(" ", splitInput.Skip(1)));
                    break;
                case "type":
                    if (builtinCommands.Contains(splitInput[1]))
                        Console.WriteLine($"{splitInput[1]} is a shell builtin");
                    else 
                        Console.WriteLine($"{splitInput[1]}: not found");
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;

            }

        }

    }
}
