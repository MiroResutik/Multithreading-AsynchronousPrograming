//Automatic and Manual Reset Event

// Manual Reset Event
// ManualResetEventSlim - Represents a thread synchronization event that,
// when signaled, must be reset manually.
// This class is a lightweight alternative to ManualResetEvent.

//Thread are being blocked by setting the ManualResetEventSlim to false
ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);

Console.WriteLine("\nPress enter to release all threads...");

for (int i = 0; i <= 3; i++)
{
    Thread thread = new Thread(Work);
    thread.Name = $"\nThread {i}";
    thread.Start();
}

// Main producer in this example is below WriteLine statement enter key stroke

Console.ReadLine();

manualResetEvent.Set();

Console.ReadLine();

void Work()
{
    Console.WriteLine($"\n{Thread.CurrentThread.Name} is waiting for the signal...");
    manualResetEvent.Wait(); // waiting for signal
    Thread.Sleep(1000); // simulate longer process time
    Console.WriteLine($"\n{Thread.CurrentThread.Name} has been released!!!");
}
manualResetEvent.Reset();


// //Automatic Reset Event
// Single Worker thread
/*
//Represents a thread synchronization event that, when signaled, 
//releases one single waiting thread and then resets automatically. 
//This class cannot be inherited.
using AutoResetEvent autoResetEvent = new AutoResetEvent(false);
string? userInput = null;

Console.WriteLine("\nServer is running!!!\nType 'go' to proceed and  'exit' to stop.");

// Start the worker thread
for (int i = 0; i < 3; i++)
{
    Thread workerThread = new Thread(Worker);
    workerThread.Name = $"Worker {i + 1}";
    workerThread.Start();
}

// Main thread receives user input

while (true)
{
    userInput = Console.ReadLine();

    // Signal the worker thread if input is "go"
    if (userInput.ToLower() == "go")
    {
        autoResetEvent.Set();
    }
}


void Worker()
{
    while (true)
    {
        Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for signal.");
        // Wait for the signal from the main thread
        autoResetEvent.WaitOne();

        Console.WriteLine($"{Thread.CurrentThread.Name} proceeds.");
        // Simulate processing time
        Thread.Sleep(3000);
    }
}
*/
// Multiple Worker threads
/*
using AutoResetEvent autoResetEvent = new AutoResetEvent(false);
string? userInput = null;


// Start the worker thread
Thread workerThread = new Thread(Worker);
workerThread.Start();

// Main thread receives user input
Console.WriteLine("\nServer is running!!!\nType 'go' to proceed and  'exit' to stop.");
while (true)
{
    userInput = Console.ReadLine();

    // Signal the worker thread if input is "go"
    if (userInput.ToLower() == "go")
    {
        autoResetEvent.Set();
    }
}


void Worker()
{
    while (true)
    {
        Console.WriteLine("\nWorker thread is waiting for signal.");
        // Wait for the signal from the main thread
        autoResetEvent.WaitOne();

        Console.WriteLine("\nWorker thread proceeds.");
        // Simulate processing time
        Thread.Sleep(3000);
    }
}
*/

