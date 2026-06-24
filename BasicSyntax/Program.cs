namespace BasicSyntax
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Multithreading-Asynchronous Programing\n");

            void WriteThreadId()
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Thread Id is: " +Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(1000);
                }
                
            }

           

            // Create new thread
            Thread thread1 = new Thread(WriteThreadId);
            Thread thread2 = new Thread(WriteThreadId);
            Thread thread3 = new Thread(WriteThreadId);

            //Set the thread Priority
            thread1.Priority = ThreadPriority.Highest;
            thread2.Priority = ThreadPriority.Lowest;
            thread3.Priority = ThreadPriority.BelowNormal;
            Thread.CurrentThread.Priority = ThreadPriority.Normal;

            thread1.Start();
            thread2.Start();
            thread3.Start();

            Console.WriteLine("\nThread 1 Id is: " + thread1.ManagedThreadId);

            Console.WriteLine("\nThread 2 Id is: " + thread2.ManagedThreadId);

            Console.WriteLine("\nThread 3 Id is: " + thread3.ManagedThreadId);




            Console.ReadLine();

        }

    }
}
