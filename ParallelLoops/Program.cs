
// This code demonstrates the use of Parallel.For, Parallel.ForEach,
// and Parallel.Invoke in C# to perform parallel operations on an array of integers.
using System.Diagnostics;

int[] array = Enumerable.Range(1, 50).ToArray();

int sum = 0;
int sum2 = 0;

// An object used for locking to ensure thread safety when updating the sum variables.
object lockSum = new object();

// Parallel.For and Parallel.ForEach are used to perform parallel iterations over a collection.
// In this example,we are calculating the sum of an array of integers using both methods.
Parallel.For(0, array.Length, i =>
{
    lock (lockSum)
    {
        sum += array[i];
        // Display the current task id for each iteration
        // and whether the current thread is a thread pool thread.
        Console.WriteLine($"Current task id: {Task.CurrentId}: Is thread pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
        
    }
});

Parallel.ForEach(array, i =>
{
    lock (lockSum)
    {
        sum2 += i;
        Console.WriteLine($"Current task id: {Task.CurrentId}: Is thread pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
    }
});

Console.WriteLine($"\nThe sum is {sum} using Parallel.For");
Console.WriteLine($"\nThe sum is {sum2} using Parallel.ForEach");



// Parallel.Invoke is used to execute multiple actions in parallel.
//Parallel.Invoke() is synchronous—the main thread waits until all actions complete before continuing.
//Each action is independent and can execute simultaneously on different ThreadPool threads.
//Thread IDs show that multiple worker threads are involved.
//The overall execution time is approximately the duration of the longest action (around 3 seconds),
//not the sum of all action durations, demonstrating the benefit of parallel execution.
//The Task Parallel Library automatically schedules the work, so you don't need to create or manage threads yourself.

// Parallel.Invoke is useful when:
//
// • Multiple independent pieces of work need to be completed.
// • One action does not depend on another.
// • The results do not need to be returned.

// If values need to be returned, Task<T> is usually a better choice.

Console.WriteLine("\n\nParallel.Invoke Demonstration!!!\n");

Stopwatch stopwatch = Stopwatch.StartNew();

Parallel.Invoke(

    () =>
    {
        Console.WriteLine($"Action One started on Thread {Environment.CurrentManagedThreadId}");

        Thread.Sleep(2000);

        Console.WriteLine($"Action One finished on Thread {Environment.CurrentManagedThreadId}");
    },

    () =>
    {
        Console.WriteLine($"Action Two started on Thread {Environment.CurrentManagedThreadId}");

        Thread.Sleep(3000);

        Console.WriteLine($"Action Two finished on Thread {Environment.CurrentManagedThreadId}");
    },

    () =>
    {
        Console.WriteLine($"Action Three started on Thread {Environment.CurrentManagedThreadId}");

        Thread.Sleep(1500);

        Console.WriteLine($"Action Three finished on Thread {Environment.CurrentManagedThreadId}");
    },

    () =>
    {
        Console.WriteLine($"Action Four started on Thread {Environment.CurrentManagedThreadId}");

        // Simulate CPU-intensive work
        long total = 0;

        for (int i = 0; i < 50_000_000; i++)
        {
            total += i;
        }

        Console.WriteLine($"Action Four calculated {total:N0}");

        Console.WriteLine($"Action Four finished on Thread {Environment.CurrentManagedThreadId}");
    }

);

// Parallel.Invoke() blocks until ALL actions complete.
stopwatch.Stop();

Console.WriteLine("\nAll actions have completed.");

Console.WriteLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds} ms");

Console.WriteLine("\nMain Thread Information");
Console.WriteLine("-----------------------");
Console.WriteLine($"Managed Thread ID : {Environment.CurrentManagedThreadId}");
Console.WriteLine($"ThreadPool Thread : {Thread.CurrentThread.IsThreadPoolThread}");
Console.WriteLine($"Background Thread : {Thread.CurrentThread.IsBackground}");

Console.WriteLine("\nPress Enter to exit...");
Console.ReadLine();


