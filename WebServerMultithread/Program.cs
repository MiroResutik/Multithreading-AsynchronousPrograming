// Queue declaration
Queue<string?> requestQueue = new Queue<string?>();

// 2. Star the request queue monitoring thread
Thread monitoringThread = new Thread(MonitorQueue);
monitoringThread.Start();

// 1. Enqueue the requests
Console.WriteLine("Server is running. Type 'exit' to stop.");

while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        break;
    }
    // enqueue the input if exit is not enered
    requestQueue.Enqueue(input);
}

// Monitor queue
void MonitorQueue()
{
    while (true)
    {
        if (requestQueue.Count > 0) 
        {
            string? input = requestQueue.Dequeue();
            Thread processingThread = new Thread(() => ProcessInput(input));
            processingThread.Start();
        }
        Thread.Sleep(100);
    }
}
// 3. Processing Input
static void ProcessInput(string input)
{
    Thread.Sleep(2000);
    Console.WriteLine($"Processed input: {input}");

    Console.ReadLine();
}
