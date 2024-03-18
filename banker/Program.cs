using System;

namespace banker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int i, j, p, r;

            Console.WriteLine("Initial system"); // 初始系統狀態
            Console.Write("Number of processes: ");
            p = int.Parse(Console.ReadLine());

            Console.Write("Number of resources: "); // 資源數量
            r = int.Parse(Console.ReadLine());

            int[] available = new int[r];
            int[,] max = new int[p, r];
            int[,] allocation = new int[p, r];

            Console.Write("Available resources: "); // 可用資源
            string[] temp = Console.ReadLine().Split(' ');
            for (i = 0; i < temp.Length; i++)
                available[i] = int.Parse(temp[i]);

            Console.WriteLine("Maximum resources for each process: "); // 每個 process 最大資源需求
            for (i = 0; i < p; i++)
            {
                Console.Write("Process " + i + ": ");
                temp = Console.ReadLine().Split(' ');
                for (j = 0; j < r; j++)
                    max[i, j] = int.Parse(temp[j]);
            }   // end of for

            Console.WriteLine("Allocated resources for each process: "); // 每個 process 已分配資源
            for (i = 0; i < p; i++)
            {
                Console.Write("Process " + i + ": ");
                temp = Console.ReadLine().Split(' ');
                for (j = 0; j < r; j++)
                    allocation[i, j] = int.Parse(temp[j]);
            } // end of for

            // 計算 need
            int[,] need = new int[p, r];
            for (i = 0; i < p; i++)
            {
                for (j = 0; j < r; j++)
                    need[i, j] = max[i, j] - allocation[i, j];
            } // end of for

            // 顯示 need
            Console.WriteLine("Need matrix: ");
            for (i = 0; i < p; i++)
            {
                Console.Write("Process " + i + ": ");
                for (j = 0; j < r; j++)
                    Console.Write(need[i, j] + " ");
                Console.WriteLine();
            } // end of for

            banker.OriginSafe(available, need, allocation);

            Console.WriteLine("--------Request-----------");

            string ans;

            // 假設 p 是請求資源的進程的編號，request 是請求的資源數量
            int[] request = new int[r];
            Console.Write("Enter process number: ");
            int requestP = int.Parse(Console.ReadLine());
            Console.Write("Enter request: ");
            temp = Console.ReadLine().Split(' ');
            for (i = 0; i < temp.Length; i++)
                request[i] = int.Parse(temp[i]);

            bool requestCheck = true;
            banker.RequestCheck(request, need, requestP, requestCheck, available);

            banker.RequestSafe(request, need, requestP, allocation, available);

            do
            {
                Console.Write("Do you want to continue? (y/n) ");
                ans = Console.ReadLine();

                if (ans == "y" || ans == "Y")
                {
                    Console.WriteLine("--------Request-----------");

                    // 假設 p 是請求資源的進程的編號，request 是請求的資源數量
                    Console.Write("Enter process number: ");
                    requestP = int.Parse(Console.ReadLine());
                    Console.Write("Enter request: ");
                    temp = Console.ReadLine().Split(' ');
                    for (i = 0; i < temp.Length; i++)
                        request[i] = int.Parse(temp[i]);

                    requestCheck = true;
                    banker.RequestCheck(request, need, requestP, requestCheck, available);

                    banker.RequestSafe(request, need, requestP, allocation, available);
                } // end of if
            } while (ans == "y" || ans == "Y");


            if (ans == "n" || ans == "")
            {
                Console.WriteLine("Exit the system...");
                Environment.Exit(0);
            }

            Console.Read();
        } // end of Main
    }
}