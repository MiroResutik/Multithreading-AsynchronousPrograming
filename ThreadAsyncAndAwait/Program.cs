namespace ThreadAsyncAndAwait
{
    internal class Program
    {
        // This is the main entry point of the application.
        // async Task Main is used to allow the use of await in the main method.
        static async Task Main(string[] args)
        {
            Console.WriteLine($"\n1. Main thread id:{Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("\nStarting to do work.....");

            // await sets out the work to be done
            // asynchronously and allows the main thread to continue executing.
            // await returns a value when the asynchronous operation is complete,
            // and the main thread can continue executing after that.
            var data = await FetchDataAsync();
            Console.WriteLine($"\nData is fetched: {data}");

            Console.WriteLine($"\n2. Thread id:{Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("\nPress enter to exit.");
            Console.ReadLine();
        }

        // This method simulates an asynchronous operation that takes some time to complete.
        static async Task<string> FetchDataAsync()
        {
            // main thread will continue executing while this method is running asynchronously
            Console.WriteLine($"\n3. Thread id:{Thread.CurrentThread.ManagedThreadId}");

            await Task.Delay(2000);

            Console.WriteLine($"\n4. Thread id:{Thread.CurrentThread.ManagedThreadId}");

            return "Returned Complex data.";
        }
    }
}
