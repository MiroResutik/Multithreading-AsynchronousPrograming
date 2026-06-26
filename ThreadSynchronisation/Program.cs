int counter = 0; // counter is shared resource in this example

//Lock 
object counterLock = new object();


Thread thread1 = new Thread(IncrementCounter);
thread1.Start();
// If the thread1 is run before the thread2 is created -
// thread1 will finish before thread2
// In this instance the counter will be 200000
//thread1.Join(); 

Thread thread2 = new Thread(IncrementCounter);
thread2.Start();

// Uncomment below thread1 and comment the thread1 above
// this will run both threads at the same time and counter will be less than 200000
thread1.Join();
thread2.Join();

Console.WriteLine($"Final counter value is: {counter}");
void IncrementCounter()

{
    for (int i = 0; i < 100000; i++)
    {
        //counter = counter + 1;

        //Monitor lock
        Monitor.Enter(counterLock);
        try
        {
            counter = counter + 1;
        }
        finally
        {
            Monitor.Exit(counterLock);
        }

        // Exclusive Lock the counter that only one thread can execute at one time
        // whithin the lock there is a try and catch mechanism so it will not crash
        /*
        lock (counterLock)
        {
            counter = counter + 1;
        }
        */
    }
    
}
Console.ReadLine();
