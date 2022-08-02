using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;

namespace cmd_gpu_display
{
    class Program
    {

        public class UpdateVisitor : IVisitor
        {

            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        static int getGPUCount()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.GPUEnabled = true;
            computer.Accept(updateVisitor);
            int countAMD = 0;
            int countNV = 0;

            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia) countNV++;
                if (computer.Hardware[i].HardwareType == HardwareType.GpuAti) countAMD++;
            }
            computer.Close();

            return countAMD + countNV;
        }

        static void Main(string[] args)
        {
            int GPUCount = 0;

            while (true)
            {
                GPUCount = getGPUCount();

                Console.Clear();
                Console.WriteLine(GPUCount);
                Console.WriteLine("Clean previous data");
                Console.WriteLine("Get data for all gpus");
                Console.WriteLine("Print data");
                Console.WriteLine("Wait few seconds");
                System.Threading.Thread.Sleep(5000);

            }
        }
    }
}
