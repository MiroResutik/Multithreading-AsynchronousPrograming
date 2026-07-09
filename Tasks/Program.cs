Console.WriteLine("\nThreading: Divide and Conquer using Tasks!!!\n");

int[] array = Enumerable.Range(1, 100).ToArray();

int SumSegment(int start, int end)
{
    int segmetnSum = 0;
    Thread.Sleep(100);
    for (int i = start; i < end; i++)
    {
        segmetnSum += array[i];
    }
    return segmetnSum;
}


var startTime = DateTime.Now;

int numOfThreads = 4;
int segmentLenght = array.Length / numOfThreads;

Task<int>[] tasks = new Task<int>[numOfThreads];
tasks[0] = Task.Run(() => { return SumSegment(0, segmentLenght); });
tasks[1] = Task.Run(() => { return SumSegment(segmentLenght, 2 * segmentLenght); });
tasks[2] = Task.Run(() => { return SumSegment(segmentLenght * 2, 3 * segmentLenght); });
tasks[3] = Task.Run(() => { return SumSegment(segmentLenght * 3, array.Length); });


Console.WriteLine($"The sum is {tasks.Sum(t => t.Result)}");

//Console.WriteLine($"The sum is {tasks[0].Result + tasks[1].Result + tasks[2].Result + tasks[3].Result}");

var endTime = DateTime.Now;

var timespan = endTime - startTime;

Console.WriteLine($"The time it takes in Miliseconds is: {timespan.TotalMilliseconds}");

Console.ReadLine();
