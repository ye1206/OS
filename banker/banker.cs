using System;
using System.Collections.Generic;
using System.Text;

namespace banker
{
    public class banker
    {
        public static int[] work; // available
        public static int[] requestAvail;
        public static int[,] requestNeed;
        public static int[,] requestAllo;
        public static bool done = false;
        public static bool[] finish;
        public static List<int> sequence = new List<int>();

        public static void OriginSafe(int[] available, int[,] need, int[,] allocation)
        {
            int i, j;
            int r = available.Length;  // resource 數量
            int p = allocation.Length / r;
            finish = new bool[p];
            work = (int[])available.Clone(); // copy available
            done = false;
            while (!done)
            {
                done = true;
                for (i = 0; i < p; i++)
                {
                    if (finish[i] == false)
                    {
                        bool check = true;
                        for (j = 0; j < r; j++)
                            if (need[i, j] > work[j]) // if need > available
                                check = false;

                        if (check)
                        {
                            for (j = 0; j < r; j++)
                                work[j] += allocation[i, j]; // release resources
                            finish[i] = true;
                            sequence.Add(i); // add to safe sequence
                            done = false; // 有 process 完成，所以我們在下一次再次檢查所有 process
                        } // end of inner-if
                    } // end of if 
                } // end of for
            } // end of while

            // Check if all processes could finish
            if (Array.TrueForAll(finish, x => x))
            {
                Console.WriteLine("System is in safe state.");
                Console.WriteLine("Safe sequence is: " + string.Join(", ", sequence));
                Console.WriteLine();
            } // end of if
            else
                Console.WriteLine("System is not in safe state");
        } // end of OriginSafe

        public static void RequestSafe(int[] request, int[,] need, int requestP, int[,] allocation, int[] available)
        {
            int i, j;
            int r = request.Length;
            int p = allocation.Length / r;
            requestAvail = new int[r];
            requestNeed = new int[p, r];
            requestAllo = new int[p, r];

            work = (int[])available.Clone();
            requestNeed = (int[,])need.Clone();
            requestAllo = (int[,])allocation.Clone();
            done = false;
            finish = new bool[p]; //initialize finish array

            // Assume the request is granted
            for (i = 0; i < r; i++)
            {
                work[i] -= request[i]; // available - request
                requestAvail[i] = work[i];
                requestNeed[requestP, i] -= request[i]; // need - request
                requestAllo[requestP, i] += request[i]; // allocation + request
            } // end of for

            if (work == null)
            {
                Console.WriteLine("work array is not initialized");
                return;
            } // end of if

            Console.Write("Available: ");
            foreach (int item in work)
                Console.Write(item + " ");
            Console.WriteLine();

            Console.WriteLine("New allocation matrix: ");
            for (i = 0; i < allocation.GetLength(0); i++)
            {
                Console.Write("Process " + i + ": ");
                for (j = 0; j < allocation.GetLength(1); j++)
                    Console.Write(requestAllo[i, j] + " ");
                Console.WriteLine();
            } // end of for

            Console.WriteLine("New need matrix: ");
            for (i = 0; i < need.GetLength(0); i++)
            {
                Console.Write("Process " + i + ": ");
                for (j = 0; j < need.GetLength(1); j++)
                    Console.Write(requestNeed[i, j] + " ");
                Console.WriteLine();
            } // end of for

            Console.WriteLine();

            done = false;
            sequence.Clear();

            while (!done)
            {
                done = true;
                for (i = 0; i < p; i++)
                {
                    if (finish[i] == false)
                    {
                        bool check = true;
                        for (j = 0; j < r; j++)
                        {
                            if (work[j] < requestNeed[i, j]) // if need > available
                            {
                                check = false;
                                break;
                            }
                        }

                        if (check) // 如果 need 小於 available
                        {
                            for (j = 0; j < r; j++)
                                work[j] += requestAllo[i, j]; // release resources
                            finish[i] = true; // process i 完成
                            sequence.Add(i); // add to safe sequence
                            done = false;
                        } // end of inner-if
                    } // end of if
                } // end of for
            } // end of while

            // Check if all processes could finish
            if (Array.TrueForAll(finish, x => x))
            {
                Console.WriteLine("System is in safe state after the request");
                Console.WriteLine("Safe sequence is: " + string.Join(", ", sequence));
                available = requestAvail;
                allocation = requestAllo;
                need = requestNeed;

            }
            else
            {
                Console.WriteLine("System is not in safe state after the request");
            }
        } // end of RequestSafe

        public static void RequestCheck(int[] request, int[,] need, int requestP, bool requestCheck, int[] available)
        {
            int r = request.Length;
            int i;
            work = (int[])available.Clone();

            for (i = 0; i < r; i++) // 檢查 request 是否超過 need 
            {
                if (request[i] > need[requestP, i])
                {
                    Console.WriteLine("Error: Request exceeds need.");
                    Console.WriteLine("Exit...");
                    requestCheck = false;
                    Environment.Exit(0);
                } // end of if

                else if (request[i] > work[i]) // 檢查 request 是否超過 available
                {
                    Console.WriteLine("Available: ");
                    foreach (int item in requestAvail)
                        Console.Write(item + " ");
                    Console.WriteLine("Error: Request exceeds available resources.");
                    Console.WriteLine("Exit...");
                    requestCheck = false;
                    Environment.Exit(0);
                } // end of if
                else
                    requestCheck = true;
            }  // end of for
        } // end of RequestCheck
    }
}
