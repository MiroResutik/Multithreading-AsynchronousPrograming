bool cancelThread = false;

Thread thread = new Thread(Work);
thread.Start();

Console.WriteLine("\nTo cancel, enter 'c'");
var input = Console.ReadLine();
if (input == "c")
{
    cancelThread = true;
}

thread.Join();
Console.ReadLine();

void Work()
{
    Console.WriteLine("\nStarted to open 100 000 threads!!!");

    for (int i = 0; i < 100000; i++)
    {
        if (cancelThread)
        {
            Console.WriteLine($"\nUser requested cancellation at iteration: {i}");
            break;
        }

        Thread.SpinWait(300000);
    }

    Console.WriteLine("\nWork is done!!!");

}
