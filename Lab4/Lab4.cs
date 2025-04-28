using System.Threading;

namespace Lab4
{
    internal class Program
    {
        unsafe static void Main(string[] args)
        {
            var interprocessMutex = new Mutex(false, "Global\\Lab4Mutex");
            if (!interprocessMutex.WaitOne(100))
            {
                Console.WriteLine("Another instance is running");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            interprocessMutex.ReleaseMutex();
        }
    }
}