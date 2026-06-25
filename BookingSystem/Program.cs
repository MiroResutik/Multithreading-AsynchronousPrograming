// Queue declaration
Queue<string?> requestQueue = new Queue<string?>();

// Declare number of available tickets
// This is a shared resource in this example
int availableTickets = 10;

//Lock the seats
object ticketsLock = new object();

// 2. Star the request queue monitoring thread
Thread monitoringThread = new Thread(MonitorQueue);
monitoringThread.Start();

// 1. Enqueue the requests
Console.WriteLine("Server is running!!! \r\n Type 'b' to book a ticket.\r\nType 'c' to cancel." +
    "\r\nType 'exit to exit.\r\n");

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
            Thread processingThread = new Thread(() => ProcessBooking(input));
            processingThread.Start();
        }
        Thread.Sleep(100);
    }
}
// 3. Processing the requests
void ProcessBooking(string? input)
{
    //The lock statement ensures that at most only one thread executes its body at any moment in time.
    // By applying the lock function to this critical section
    // we make sure that only one thread is accessing this section at the time
    /*
    lock (seatLock)
    {

        if (input == "b")
        {
            if (availableTickets > 0)
            {
                availableTickets--;
                Console.WriteLine();
                Console.WriteLine($"Your seat is booked!!! {availableTickets} seats are still available.");

            }
            else
            {
                Console.WriteLine("Tickets are not available");
            }
        }
        else if (input == "c")
        {
            if (availableTickets < 10)
            {

                availableTickets++;
                Console.WriteLine();
                Console.WriteLine($"Your booking is canceled!!! {availableTickets} seats are still available.");

            }
            else
            {
                Console.WriteLine("Error. You cannot cancel a booking at this time.");
            }
        }

    }
    */

    //Monitor - Provides a mechanism that synchronizes access to objects.
    /*
    if (Monitor.TryEnter(ticketsLock, 2000))
    {
        try
        {
            // Simulate processing time
            Thread.Sleep(3000);

            if (input == "b")
            {
                if (availableTickets > 0)
                {
                    availableTickets--;
                    Console.WriteLine();
                    Console.WriteLine($"Your seat is booked. {availableTickets} seats are still available.");
                }
                else
                {
                    Console.WriteLine($"Tickets are not available.");
                }
            }
            else if (input == "c")
            {
                if (availableTickets < 10)
                {
                    availableTickets++;
                    Console.WriteLine();
                    Console.WriteLine($"Your booking is canceled. {availableTickets} seats are available.");
                }
                else
                {
                    Console.WriteLine($"Error. You cannot cancel a booking at this time.");
                }
            }
        }
        finally
        {
            // Relese the lock
            Monitor.Exit(ticketsLock);
        }
    }
    else
    {
        Console.WriteLine("The system is busy. Please wait.");
    }
    */

    // Mutex - A synchronization primitive that can also be used for interprocess synchronization.


}



