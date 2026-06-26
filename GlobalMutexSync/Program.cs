
using System.Diagnostics.Metrics;

string filePath = "counter.txt";
// Path for created data is ..source\repos\Multithreading-AsynchronousPrograming\GlobalMutexSync\bin\Debug\net10.0
// Delete all data in that folder. It will get recreated once this project is runned for the firs time
// To get to expected result run GlobalMutexSync.exe twice quickly

// Standard loop not using Mutex
/*
for (int i = 0; i < 10000; i++)
{
    
    int counter = ReadCounter(filePath);
    counter++;
    WriteCounter(filePath, counter);

}
*/

// Mutex is protecting the critical section across diffrent processes
using (var mutex = new Mutex(false, $"GlobalFileMutex:{filePath}"))
{
    for (int i = 0; i < 10000; i++)
    {
        //declare Mutex before the critical section
        mutex.WaitOne();
        try
        {
            // This is the critical section
            int counter = ReadCounter(filePath);
            counter++;
            WriteCounter(filePath, counter);
        }
        finally 
        {

            mutex.ReleaseMutex();
        }


    }
}
Console.WriteLine($"\nProcess finished!!!\n\nExpected result is 10000 when runned once." +
    $"\n 20000 when runned 2 times ");
Console.ReadLine();
Console.ReadKey();

int ReadCounter(string filePath)
{
    using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
    using (var reader = new StreamReader(stream))
    {
        string content = reader.ReadToEnd();
        return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
    }
}

void WriteCounter(string filePath, int counter)
{
    using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
    using (var writer = new StreamWriter(stream))
    {
        writer.Write(counter);
    }
}
