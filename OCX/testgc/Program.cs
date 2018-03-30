using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace testgc
{
    class Program
    {
        static void Main(string[] args)
        {
            long lenth = 1024 * 1024 * 128;

            GetCost("程序启动");

            double[] data = new double[lenth];
            for (int i = 0; i < lenth; i++)
            {
                data[i] = double.MaxValue;
            }
            GetCost("数据制造完成");

            data = null;
            GetCost("data = null");

            System.GC.Collect();
            GetCost("System.GC.Collect()");

            Console.ReadKey();
        }

        /// <summary>
        /// 显示内存使用的状态
        /// </summary>
        /// <param name="state"></param>
        static void GetCost(string state)
        {
            Console.Write("当前状态：" + state + ";  占用内存:");
            using (var p1 = new PerformanceCounter("Process", "Working Set - Private", "testgc"))
            {
                Console.WriteLine((p1.NextValue() / 1024 / 1024).ToString("0.0") + "MB");
            }
        }
    }
}
