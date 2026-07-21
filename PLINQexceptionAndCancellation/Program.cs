using System.Diagnostics;

Console.WriteLine("\nPLINQ Exception Handling and Cancellation\n");

// ============================================================================
// PLINQ Exception Handling and Cancellation
// ============================================================================
//
// This example demonstrates:
//
// • Parallel query execution
// • Exception handling
// • AggregateException
// • Cancellation using CancellationToken
// • ForAll()
// • ThreadPool worker threads
//
// PLINQ executes work on multiple threads simultaneously.
//
// Because multiple threads may fail at the same time,
// PLINQ wraps all exceptions inside an AggregateException.
//
// ============================================================================

// Used to cancel the query.
using CancellationTokenSource cts = new();

IEnumerable<int> items = Enumerable.Range(1, 50);

Stopwatch stopwatch = Stopwatch.StartNew();

try
{
    ParallelQuery<int> evenNumbers =
        items.AsParallel()

             // Preserve ordering (optional)
             .AsOrdered()

             // Allow cancellation.
             .WithCancellation(cts.Token)

             // Wait until all partitions complete.
            
             .WithMergeOptions(ParallelMergeOptions.FullyBuffered)

             // Limit worker threads.
             .WithDegreeOfParallelism(Environment.ProcessorCount)

             .Where(number =>
             {
                 Console.WriteLine(
                     $"Filtering {number,-2} on Thread {Environment.CurrentManagedThreadId}");

                 Thread.Sleep(300);

                 // Simulate failures.
                 // Throw exceptions for specific numbers to demonstrate exception handling.
                 // comment out the following lines to see the query complete successfully.
                 if (number == 5)
                     throw new InvalidOperationException(
                         "Number 5 generated an InvalidOperationException.");

                 if (number == 20)
                     throw new ArgumentNullException(
                         "Number 20 generated an ArgumentNullException.");
                 // End of commented out lines for demonstration.
                 return number % 2 == 0;
             });

    Console.WriteLine("\nProcessing Results\n");

    evenNumbers.ForAll(number =>
    {
        Console.WriteLine(
            $"Result {number,-2} processed on Thread {Environment.CurrentManagedThreadId}");

        Thread.Sleep(200);

        // Demonstrate cancellation.
        if (number >= 8)
        {
            Console.WriteLine("\nCancellation requested...\n");
            cts.Cancel();
        }
    });
}
catch (OperationCanceledException)
{
    Console.WriteLine("\nThe PLINQ query was cancelled.");
}
catch (AggregateException ex)
{
    Console.WriteLine("\nOne or more exceptions occurred.\n");

    foreach (Exception exception in ex.InnerExceptions)
    {
        Console.WriteLine($"Exception Type : {exception.GetType().Name}");
        Console.WriteLine($"Message        : {exception.Message}");
        Console.WriteLine();
    }
}
finally
{
    stopwatch.Stop();

    Console.WriteLine("------------------------------------");
    Console.WriteLine($"Elapsed Time : {stopwatch.ElapsedMilliseconds} ms");
    Console.WriteLine("------------------------------------");
}

Console.WriteLine("\nProgram Finished.");

Console.ReadLine();