class Program
{
    static void Main()
    {
        var commandsList = new List<string>();
        while (true)
        {
            Console.Write("$ ");
            string command = Console.ReadLine();

            if (commandsList.Contains(command))
            {

            }
            else
            {
                Console.WriteLine($"{command}: command not found");
            }
        }
    }
}
