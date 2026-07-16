using System.Diagnostics;


// PLINQ (Parallel LINQ) Demonstration

// PLINQ (Parallel LINQ) is the parallel implementation of LINQ.

// Instead of processing a collection on a single thread,
// PLINQ partitions the collection and processes multiple partitions
// simultaneously using ThreadPool threads.
// PLINQ is particularly useful for:

// • CPU-intensive data processing
// • Large collections
// • Independent operations
// • Mathematical calculations

// PLINQ is NOT ideal for:

// • Small collections
// • I/O-bound operations
// • Operations that must execute in strict order

Console.WriteLine("\nParallel LINQ - PLINQ Demonstration!!!\n");

// Create a collection containing the numbers 1 - 200.
IEnumerable<int> items = Enumerable.Range(1, 200);

// Start timing.
Stopwatch stopwatch = Stopwatch.StartNew();

// Process the collection in parallel.

// AsParallel()
//      Converts the IEnumerable into a ParallelQuery.

// WithDegreeOfParallelism()
//      Limits the maximum number of worker threads.

// WithMergeOptions()
//      Controls when results become available.

// FullyBuffered
//      Wait until every partition has completed before returning results.

// Other options:

// • AutoBuffered
// • NotBuffered

// AsOrdered()
//      Preserves the original ordering.

// Remove AsOrdered() to see the results appear in an arbitrary order.

ParallelQuery<int> evenNumbers =
    items.AsParallel()
         .AsOrdered()
         .WithDegreeOfParallelism(Environment.ProcessorCount)
         .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
         .Where(number =>
         {
             Console.WriteLine(
                 $"Filtering {number} on Thread {Environment.CurrentManagedThreadId}");

             Thread.SpinWait(1_000_000);

             return number % 2 == 0;
         });

// Materialize the query.

// LINQ queries are deferred.

// No work actually begins until:

// • foreach
// • Count()
// • ToList()
// • ToArray()
// • Sum()

List<int> results = evenNumbers.ToList();

stopwatch.Stop();

// Display the results. Notice that this foreach executes on the MAIN thread,
// not the worker threads.

Console.WriteLine("\nEven Numbers");
Console.WriteLine("------------");

foreach (int number in results)
{
    Console.WriteLine(
        $"Result {number,-3} displayed on Thread {Environment.CurrentManagedThreadId}");
}

// PLINQ Aggregation
// Aggregation operations are also performed in parallel.

Console.WriteLine();

Console.WriteLine($"Count   : {results.Count}");
Console.WriteLine($"Sum     : {results.Sum()}");
Console.WriteLine($"Average : {results.Average():F2}");
Console.WriteLine($"Minimum : {results.Min()}");
Console.WriteLine($"Maximum : {results.Max()}");

Console.WriteLine();

Console.WriteLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

// Sequential Comparison
stopwatch.Restart();

var sequentialResults =
    items.Where(number =>
    {
        Thread.SpinWait(1_000_000);
        return number % 2 == 0;
    }).ToList();

stopwatch.Stop();

Console.WriteLine();
Console.WriteLine($"Sequential execution took {stopwatch.ElapsedMilliseconds} ms");


// ============================================================================
// Discussion
// ============================================================================

// AsParallel()
//      Enables PLINQ.

// AsOrdered()
//      Preserves ordering but introduces some overhead.

// AsUnordered()
//      Allows PLINQ to maximize throughput.

// WithDegreeOfParallelism()
//      Controls the maximum number of worker threads.

// WithMergeOptions()

// FullyBuffered
//      Wait for all partitions before returning.

// AutoBuffered
//      Return batches as they become available.

// NotBuffered
//      Return results immediately.

// PLINQ uses ThreadPool threads,
// so no manual thread management is required.

Console.WriteLine("\nFinished.");

Console.ReadLine();