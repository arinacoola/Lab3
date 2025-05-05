using System.Threading;

namespace Lab4
{
    internal class Program
    {
        unsafe static int Main(string[] args)
        {
            var interprocessMutex = new Mutex(false, "Global\\Lab4Mutex");
            if (!interprocessMutex.WaitOne(100))
            {
                Console.WriteLine("Another instance is running");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return 1;
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            interprocessMutex.ReleaseMutex();
            return 0;
        }
    }
}