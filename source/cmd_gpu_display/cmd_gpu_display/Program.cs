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


        static void readGPUData(String[] GpuN, String[] GpuT, String[] GpuF, String[] GpuC, String[] GpuM)
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.GPUEnabled = true;
            computer.Accept(updateVisitor);

            int indexGpuN = 0, indexGpuT = 0, indexGpuF = 0, indexGpuC = 0, indexGpuM = 0;
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        try
                        {
                            GpuN[indexGpuN++] = computer.Hardware[i].Name;
                        }
                        catch
                        {
                            //do nothing
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature) && computer.Hardware[i].Sensors[j].Name == "GPU Core")
                        {
                            GpuT[indexGpuT++] = computer.Hardware[i].Sensors[j].Value.ToString();
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Control) && computer.Hardware[i].Sensors[j].Name == "GPU Fan")
                        {
                            GpuF[indexGpuF++] = computer.Hardware[i].Sensors[j].Value.ToString();
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock) && computer.Hardware[i].Sensors[j].Name == "GPU Core")
                        {
                            GpuC[indexGpuC++] = computer.Hardware[i].Sensors[j].Value.ToString();
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock) && computer.Hardware[i].Sensors[j].Name == "GPU Memory")
                        {
                            GpuM[indexGpuM++] = computer.Hardware[i].Sensors[j].Value.ToString();
                        }
                    }
                }
                if (computer.Hardware[i].HardwareType == HardwareType.GpuAti)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        try
                        {
                            GpuN[indexGpuN++] = computer.Hardware[i].Name.ToString();
                        }
                        catch
                        {
                            //do nothing
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature) && computer.Hardware[i].Sensors[j].Name == "GPU Core")
                        {
                            GpuT[indexGpuT++] = ((int)computer.Hardware[i].Sensors[j].Value).ToString();
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Control) && computer.Hardware[i].Sensors[j].Name == "GPU Fan")
                        {
                            GpuF[indexGpuF++] = ((int)computer.Hardware[i].Sensors[j].Value).ToString();
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock) && computer.Hardware[i].Sensors[j].Name == "GPU Core")
                        {
                            GpuC[indexGpuC++] = ((int)computer.Hardware[i].Sensors[j].Value).ToString();
                        }
                        if ((computer.Hardware[i].Sensors[j].SensorType == SensorType.Clock) && computer.Hardware[i].Sensors[j].Name == "GPU Memory")
                        {
                            GpuM[indexGpuM++] = ((int)computer.Hardware[i].Sensors[j].Value).ToString();
                        }
                    }
                }
            }
            computer.Close();
        }


        static void Main(string[] args)
        {
            int GPUCount = 0;
            while (true)
            {
                GPUCount = getGPUCount();

                int[] GPUNumber = new int[GPUCount];
                String[] GPUName = new String[GPUCount];
                String[] GPUTemp = new String[GPUCount];
                String[] GPUFan = new String[GPUCount];
                String[] GPUCoreClk = new String[GPUCount];
                String[] GPUMemClk = new String[GPUCount];

                for (int i = 0; i < GPUCount; i++) GPUNumber[i] = i + 1;

                readGPUData(GPUName, GPUTemp, GPUFan, GPUCoreClk, GPUMemClk);

                Console.Clear();
                for (int j = 0; j < GPUCount; j++)
                {
                    Console.WriteLine(GPUNumber[j] + "  Name:" + GPUName[j] + "  T:" + GPUTemp[j] + "C  F:" + GPUFan[j] + "%  CC:" + GPUCoreClk[j] + "MHz  MC:" + GPUMemClk[j] + "MHz");
                }

                System.Threading.Thread.Sleep(5000);
            }

        }
    }
}

