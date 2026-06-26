class Program
{
    static void Main()
    {
        var commandsList = new List<string>();
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
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;

            }

        }

    }
}
