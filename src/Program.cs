using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoundRobin
{
    public class Program
    {
        private static string ReadInputFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static List<Process> TransformInputIntoList(string inputFile)
        {
            var processList = new List<Process>();
            var processFound = Regex.Matches(inputFile, @"[\d]:.*:[\d]:[\d]");
            if (processFound == null)
                throw new Exception();
            foreach (Match process in processFound.Cast<Match>())
            {
                var splitData = process.ToString().Split(':');
                processList.Add(new Process
                {
                    ProcessName = splitData[1],
                    ArrivalTime = Int32.Parse(splitData[0]),
                    ExecuteTime = Int32.Parse(splitData[3]),
                    Priority = Int32.Parse(splitData[2]),
                    TimeInProcess = 0,
                    RemainingExecuteTime = Int32.Parse(splitData[3])
                });
            }
            return processList;
        }

        private static int GetTimeLineSize(List<Process> processList)
        {
            int timeLineSize = processList.Select(s => s.ExecuteTime).Sum();
            if (timeLineSize == 0)
            {
                throw new Exception();
            }
            return timeLineSize;
        }

        private static string RoundRobin(List<Process> processList, int quantum)
        {
            string outputText = "QUANTUM = " + quantum.ToString() + "\r\nTIME: ID \r\n";
            var FIFOProcesses = new List<Process>();
            Process currentProcess = null;
            Process remainingProcess = null;
            int count = 0, i = 0, countAllProcessExecuteTime = 0;
            int allProcessExecuteTime = GetTimeLineSize(processList);
            do
            {
                FIFOProcesses.AddRange(processList.Where(w => w.ArrivalTime == i).ToArray());
                if (remainingProcess != null)
                {
                    FIFOProcesses.Add(remainingProcess);
                    remainingProcess = null;
                }
                currentProcess = FIFOProcesses.FirstOrDefault();
                if (currentProcess != null)
                {
                    if (count < quantum && currentProcess.RemainingExecuteTime > 0)
                    {
                        count++;
                        countAllProcessExecuteTime++;
                        currentProcess.RemainingExecuteTime--;
                        outputText += (i+1) + ": " + FIFOProcesses.FirstOrDefault().ProcessName + "\r\n";
                    }
                    if (count == quantum || currentProcess.RemainingExecuteTime == 0)
                    {
                        count = 0;
                        if (currentProcess.RemainingExecuteTime > 0)
                        {
                            remainingProcess = currentProcess;
                        }
                        else
                        {
                            currentProcess.TimeInProcess = i + 1;
                        }
                        FIFOProcesses.Remove(FIFOProcesses.FirstOrDefault());
                    }
                }
                ++i;
            } while (countAllProcessExecuteTime < allProcessExecuteTime);
            outputText += "Average Time: " + AverageTime(processList) + "\r\n";
            return outputText;
        }

        private static double AverageTime(List<Process> processList)
        {
            foreach (var process in processList)
            {
                process.AverageTime = process.TimeInProcess - process.ArrivalTime - process.ExecuteTime;
            }
            return (processList.Select(s => s.AverageTime).Sum())/processList.Count();
        }

        static void Main()
        {
            Console.WriteLine("Digite o caminho do arquivo de entrada: (ex.: C:\\nomePasta\\nomeArquivo.txt)");
            var inputPath = Console.ReadLine();
            Console.WriteLine("Digite o caminho onde deseja que o arquivo de saída seja gerado: (ex.: C:\\nomePasta\\nomeArquivoSaida.txt)");
            var outputPath = Console.ReadLine();
            Console.WriteLine("Digite o tamanho do quantum: ");
            var stringQuantum = Console.ReadLine();
            var quantum = Int32.Parse(stringQuantum);
            var inputFile = ReadInputFile(@inputPath);
            var processList = TransformInputIntoList(inputFile);
            var outputText = RoundRobin(processList, quantum);
            File.WriteAllText(@outputPath, outputText);
        }
    }
}
