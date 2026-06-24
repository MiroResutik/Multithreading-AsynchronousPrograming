namespace DivideAndConquer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Threading: Divide and Conquer!!!\n");

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


            int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;

            var startTime = DateTime.Now;

            int numOfThreads = 4;
            int segmentLenght = array.Length / numOfThreads;

            Thread[] threads = new Thread[numOfThreads];
            threads[0] = new Thread(() => {sum1  = SumSegment(0, segmentLenght);});
            threads[1] = new Thread(() => { sum2 = SumSegment(segmentLenght, 2 * segmentLenght); });
            threads[2] = new Thread(() => { sum3 = SumSegment(segmentLenght * 2, 3 * segmentLenght); });
            threads[3] = new Thread(() => { sum4 = SumSegment(segmentLenght * 3, array.Length); });

            foreach (var thread in threads)
            {
                thread.Start();
            }
            // Blocks the calling thread
            foreach (var thread in threads)
            {
                thread.Join();
            }
            int sum = 0;

            foreach (var item in array)
            {
                Thread.Sleep(100);
                sum += item;
            }

            var endTime = DateTime.Now;

            var timespan = endTime - startTime;

            Console.WriteLine($"The sum is {sum}");
            Console.WriteLine($"The sum is {sum1 + sum2 + sum3 + sum4}");
            Console.WriteLine($"The time it takes in Miliseconds is: {timespan.TotalMilliseconds}");

            Console.ReadLine();
        }
    }
}
