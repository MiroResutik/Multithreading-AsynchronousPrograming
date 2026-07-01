

Queue<int> queue = new Queue<int>();
// Two way signaling for 2 events (consumeEvent and produceEvent)
// Declare event
ManualResetEventSlim consumeEvent = new ManualResetEventSlim(false);

// set to true - if the producer is waiting it can produce right away
ManualResetEventSlim produceEvent = new ManualResetEventSlim(true);

// Set consume count 
int consumerCount = 0;
object lockconsumerCount = new object();

// Array of consumer threads
Thread[] consumerThreads = new Thread[3];

// loop to produce above threads
for (int i = 0; i < 3; i++)
{
    consumerThreads[i] = new Thread(Consume);
    consumerThreads[i].Name = $"Consumer {i}";
    consumerThreads[i].Start();
}
// Main thread that is blocking
while (true)
{

    produceEvent.Wait();
    produceEvent.Reset();

    Console.WriteLine("\nTo produce, enter 'p'");
    var input = Console.ReadLine() ?? ""; // ?? - if it is null use empty string

    if (input.ToLower() == "p")
    {
        for (int i = 0; i <= 10; i++)
        {
            queue.Enqueue(i);
            Console.WriteLine($"Produced: {i +1}");
        }

        consumeEvent.Set();
    }
}

// Consumer's behavior
void Consume()
{
    while (true)
    {
        consumeEvent.Wait();

        while (queue.TryDequeue(out var item))
        {
            // work on the itmes produced
            Thread.Sleep(500);
            Console.WriteLine($"Consumed: {item} from thread: {Thread.CurrentThread.Name}");
        }

        lock (lockconsumerCount)
        {
            consumerCount++;

            if (consumerCount == 3)
            {
                consumeEvent.Reset(); // turning off the event so no other signal can pass
                produceEvent.Set(); // signal is back for produce event
                consumerCount = 0;

                Console.WriteLine("More please!!!");
            }

        }

        
        
    }
}