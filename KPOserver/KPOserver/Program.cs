namespace KPOserver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Start(8888);
            Console.WriteLine("Press Enter to stop the server.");
            Console.ReadLine();
            server.Stop();
        }
    }
}