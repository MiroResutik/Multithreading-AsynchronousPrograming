using System.Collections.Concurrent;

// ============================================================================
// BlockingCollection Demonstration
// ============================================================================

// BlockingCollection<T> is a thread-safe producer-consumer collection.

// It provides:

// Thread-safe Add()
// Thread-safe Take()
// Optional bounded capacity
// Automatic blocking
// Graceful completion

// In this example:

// Producer
// --------
// The main thread accepts user requests.

// Consumer
// --------
// Multiple worker threads process requests.

// Queue
// -----
// BlockingCollection coordinates both sides.


// The underlying collection.

// BlockingCollection can wrap:

// ConcurrentQueue<T> (FIFO)
// ConcurrentStack<T> (LIFO)
// ConcurrentBag<T>

// Here we use a FIFO queue.
ConcurrentQueue<string?> requestQueue = new();


// Capacity of 3
// Once three items are waiting:

// Add() blocks until space becomes available.

BlockingCollection<string?> collection =
    new(requestQueue, boundedCapacity: 3);


// ============================================================================
// Start multiple consumers.
//
// Each consumer waits for work.

const int workerCount = 3;

Thread[] workers = new Thread[workerCount];

for (int i = 0; i < workerCount; i++)
{
    workers[i] = new Thread(MonitorQueue);

    workers[i].Name = $"Worker {i + 1}";

    workers[i].Start();
}


// ============================================================================
// Producer

Console.WriteLine("=======================================");
Console.WriteLine("BlockingCollection Producer/Consumer");
Console.WriteLine("=======================================");
Console.WriteLine();
Console.WriteLine("Enter requests.");
Console.WriteLine("Type 'exit' to stop.");
Console.WriteLine();

while (true)
{
    Console.Write("Request type character: ");

    string? input = Console.ReadLine();

    if (string.Equals(input, "exit",
        StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine();

        Console.WriteLine("No more requests will be accepted!!!");

        // Tell consumers no more items are coming.
        collection.CompleteAdding();

        break;
    }

    Console.WriteLine("Adding request...");

    // Blocks automatically if capacity has been reached.
    collection.Add(input);

    Console.WriteLine(
        $"Queued '{input}'   Queue Size: {collection.Count}");
}

// Wait for all workers.
foreach (Thread worker in workers)
{
    worker.Join();
}

Console.WriteLine();
Console.WriteLine("Server shutdown complete.");

Console.ReadLine();


// ============================================================================
// Consumer

// GetConsumingEnumerable()

// Automatically:

// Waits while the collection is empty.
// Removes items.
// Ends when CompleteAdding() has been called
// AND every item has been processed.

// No polling.

// No locking.

// No Thread.Sleep().

void MonitorQueue()
{
    Console.WriteLine(
        $"{Thread.CurrentThread.Name} started.");

    foreach (string? request in collection.GetConsumingEnumerable())
    {
        Console.WriteLine(
            $"{Thread.CurrentThread.Name} dequeued '{request}'");

        ProcessInput(request);
    }

    Console.WriteLine(
        $"{Thread.CurrentThread.Name} exiting.");
}


// ============================================================================
// Simulate processing.

// Multiple workers execute this simultaneously.

void ProcessInput(string? request)
{
    Console.WriteLine(
        $"{Thread.CurrentThread.Name} processing '{request}'");

    Thread.Sleep(3000);

    Console.WriteLine(
        $"{Thread.CurrentThread.Name} finished '{request}'");
}
