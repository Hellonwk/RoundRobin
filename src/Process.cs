using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoundRobin
{
    public class Process
    {
        public string ProcessName { get; set; }
        public int ArrivalTime { get; set; }
        public int ExecuteTime { get; set; }
        public int RemainingExecuteTime { get; set; }
        public int Priority { get; set; }
        public int TimeInProcess { get; set; }
        public double AverageTime { get; set; }
    }
}
