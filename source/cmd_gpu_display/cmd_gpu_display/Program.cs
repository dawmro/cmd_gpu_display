using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmd_gpu_display
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Clean previous data");
                Console.WriteLine("Get data for all gpus");
                Console.WriteLine("Print data");
                Console.WriteLine("Wait few seconds");
                System.Threading.Thread.Sleep(5000);

            }
        }
    }
}
