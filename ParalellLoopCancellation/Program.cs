using System.Diagnostics;

Console.WriteLine("\nParallel.For Cancellation Demonstration!!!\n");

// Create a CancellationTokenSource.
// The source can signal cancellation to one or more Tasks or Parallel loops.

using CancellationTokenSource cts = new();

CancellationToken token = cts.Token;


// Start the work on a ThreadPool thread.
// Passing the CancellationToken allows the Task itself to be cancelled before
// it starts and enables cooperative cancellation while it is running.

Task task = Task.Run(DoWork, token);

Console.WriteLine("Press 'c' and Enter at any time to cancel...\n");

string? input = Console.ReadLine();

if (input?.Equals("c", StringComparison.OrdinalIgnoreCase) == true)
{
    Console.WriteLine("\nCancellation requested...\n");
    cts.Cancel();
}


// Wait for the Task to complete.
// Wait() will throw if the Task was cancelled, so we catch the exception.

try
{
    task.Wait();
}
catch (AggregateException ex)
{
    foreach (Exception exception in ex.InnerExceptions)
    {
        Console.WriteLine($"Task Exception: {exception.GetType().Name}");
        Console.WriteLine(exception.Message);
    }
}

Console.WriteLine($"\nFinal Task Status: {task.Status}");

Console.WriteLine("\nPress Enter to exit...");
Console.ReadLine();


// Performs parallel work.
// Parallel.For automatically distributes iterations across multiple ThreadPool threads.
// Each iteration periodically checks whether cancellation has been requested.

void DoWork()
{
    Console.WriteLine("Starting Parallel.For...\n");

    Stopwatch stopwatch = Stopwatch.StartNew();

    ParallelOptions options = new()
    {
        CancellationToken = token,

        // Limit the number of worker threads.
        // Try changing this value.
        MaxDegreeOfParallelism = Environment.ProcessorCount
    };

    try
    {
        Parallel.For(0, 1000, options, i =>
        {
            // Throws OperationCanceledException when cancellation is requested.
            options.CancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine(
                $"Iteration {i,-4} | Thread {Environment.CurrentManagedThreadId}");

            // Simulate CPU-intensive work.
            Thread.SpinWait(30_000_000);

            // Report progress every 100 iterations.
            if (i % 100 == 0)
            {
                Console.WriteLine($"Completed {i} iterations...");
            }
        });

        Console.WriteLine("\nParallel loop completed successfully.");
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("\nParallel loop was cancelled!!!");
    }
    finally
    {
        stopwatch.Stop();

        Console.WriteLine($"\nElapsed Time: {stopwatch.ElapsedMilliseconds:N0} ms");
    }

    Console.WriteLine("\nWorker method finished!!!");
}
