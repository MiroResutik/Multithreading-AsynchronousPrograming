
// Queue declaration
Queue<string?> requestQueue = new Queue<string?>();

object queueLock = new object();
using SemaphoreSlim semaphore = new SemaphoreSlim(initialCount: 5, maxCount: 5);

// 2. Star the request queue monitoring thread
Task monitoringTask = new Task(MonitorQueue);
monitoringTask.Start();

// 1. Enqueue the requests
Console.WriteLine("\nServer is running.... \nType 'exit' to stop.");
Console.WriteLine("\nEnter one or more characters to see the queue processing:");

while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        break;
    }

    lock (queueLock)
    {
        requestQueue.Enqueue(input);
    }

}

// Monitor queue
void MonitorQueue()
{
    while (true)
    {
        if (requestQueue.Count > 0)
        {
            string? input;
            lock (queueLock)
            {
                input = requestQueue.Dequeue();
            }
            semaphore.Wait();
            Task processingTask = new Task(() => ProcessInput(input));
            processingTask.Start();
        }
        Thread.Sleep(100);
    }
}

// 3. Processing Input
void ProcessInput(string? input)
{
    try
    {
        // Simulate processing time
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}");
    }
    finally
    {
        var prevCount = semaphore.Release(); // Release method returns previous count of the semaphore
        Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} released the semaphore. Previous count is: {prevCount}");
    }
}

