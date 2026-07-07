// ThreadPool is a class in the System.Threading namespace that provides a pool of threads
// that can be used to execute tasks concurrently. It manages a pool of worker threads
// and I/O threads, allowing for efficient execution of multiple tasks without
// the overhead of creating and destroying threads for each task.
// ThreadPool Class Provides a pool of threads that can be used to execute tasks,
// post work items, process asynchronous I/O, wait on behalf of other threads,
// and process timers.

ThreadPool.GetMaxThreads(out var maxVorkerThreads, out var maxIOThreads);

Console.WriteLine($"\nMax Worker Threads: {maxVorkerThreads} \nMax IO Threads: {maxIOThreads}");

ThreadPool.GetAvailableThreads(out var availableWorkerThreads, out var availableIOThreads);

Console.WriteLine($"\nAvailable Worker Threads: {availableWorkerThreads} \nAvailable IO Threads: {availableIOThreads}");




Queue<string?> requestQueue = new Queue<string?>();

// 2. Start the requests queue monitoring thread
Thread monitoringThread = new Thread(MonitorQueue);
monitoringThread.Start();

// 1. Enqueue the requests
Console.WriteLine("\nServer is running...... \nType any character to submit a request and press Enter. \nType 'exit' to stop.");
while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        break;
    }

    requestQueue.Enqueue(input);
}

void MonitorQueue()
{
    while (true)
    {
        if (requestQueue.Count > 0)
        {
            string? input = requestQueue.Dequeue();
            ThreadPool.QueueUserWorkItem(ProcessInput, input);
        }
        Thread.Sleep(100);
    }
}

// 3. Processing the requests
void ProcessInput(object? input)
{
    // Simulate processing time    
    Thread.Sleep(2000);
    Console.WriteLine($"\nProcessed input: {input}. \nIs Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
}
