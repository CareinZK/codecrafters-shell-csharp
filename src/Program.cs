class Program
{
    static void Main()
    {
        var commandsList = new List<string>();
        bool isWorking = true;
        while (isWorking)
        {
            Console.Write("$ ");
            string command = Console.ReadLine();

            switch (command)
            {
                case "exit":
                    isWorking = false;
                break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;

            }

        }

    }
}
