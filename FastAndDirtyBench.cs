using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace FastAndDirtyBench
{
    public class Benchmarker
    {
        private Dictionary<string, Stopwatch> StopwatchStore;

        public Benchmarker()
        {
            StopwatchStore = new Dictionary<string, Stopwatch>();
        }

        public void Start(string name)
        {
            Stopwatch newStopwatch = new Stopwatch();
            StopwatchStore.Add(name, newStopwatch);
            StopwatchStore[name].Start();
        }       

        public void Stop(string name)
        {
            StopwatchStore[name].Stop();
        }

        public void Output(string outputFile)
        {
            if (!File.Exists(outputFile)) File.Create(outputFile);

            int longestName = StopwatchStore.Select(k => k.Key.Length).Max();

            string stringFormat = $"{{0, -{longestName + 5}}}{{1, -20}}{{2, -20}}"; // Looks ugly, but works

            using (StreamWriter sw = new StreamWriter(outputFile, true))
            {
                sw.WriteLine($"Output created at: {DateTime.Now.ToString("F")}");
                sw.WriteLine();
                sw.WriteLine(String.Format(stringFormat, "Name:", "Time:", "Time (ms):"));

                foreach (KeyValuePair<string, Stopwatch> kv in StopwatchStore.OrderBy(kv => kv.Key))
                {
                    if (kv.Value.IsRunning)
                    {
                        sw.WriteLine(String.Format(stringFormat, kv.Key, "Did Not Complete", "---"));
                    }
                    else
                    {
                        sw.WriteLine(String.Format(stringFormat, kv.Key, kv.Value.Elapsed.ToString(), kv.Value.ElapsedMilliseconds));
                    }
                }

                sw.WriteLine();
                sw.WriteLine();
            }
        }
    }

    public class BenchmarkItem
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Completed { get; set; } = false;
    }
}
