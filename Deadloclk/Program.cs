using System;
using System.Collections.Generic;
using System.Threading;

namespace Deadloclk
{
    class Program
    {
        static void Main(string[] args)
        {
            object locker1 = new object();
            object locker2 = new object();

            var collectionA = new List<string>();
            var collectionB = new List<string>();

            new Thread(() =>
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} have started");

                lock (locker1)
                {
                    Thread.Sleep(1000); // <-- comment this
                    for (int i = 0; i < 100; i++)
                    {
                        collectionA.Add("item: " + i);
                    }

                    // DeadLock
                    lock (locker2) 
                    {
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is reading collectionB");
                        foreach(var item in collectionB)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }

            }).Start();


            new Thread(() =>
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} have started");

                lock (locker2)
                {
                    Thread.Sleep(1000); // <-- comment this
                    for (int i = 0; i < 100; i++)
                    {
                        collectionB.Add("item: " + i);
                    }

                    // DeadLock
                    lock (locker1)
                    {
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is reading collectionA");
                        foreach (var item in collectionA)
                        {
                            Console.WriteLine(item);
                        }
                    }
                }
            }).Start();
        }
    }
}
