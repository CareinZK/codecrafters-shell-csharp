class Program
{
    static void Main()
    {
        var commandsList = new List<string>();
        while (true)
        {
            Console.Write("$ ");
            string command = Console.ReadLine();

            switch (command)
            {
                case "exit":
                    goto endLoop;
                break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;

            }

        }
    endLoop:
    }
}
