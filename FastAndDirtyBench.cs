using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FastAndDirtyBench
{
    public class Benchmarker
    {
        private Dictionary<string, BenchmarkItem> BenchmarkItemStore;

        public Benchmarker()
        {
            BenchmarkItemStore = new Dictionary<string, BenchmarkItem>();

            // Initialize dictionary to reduce possible latancy
            BenchmarkItemStore.Add("Initialization", new BenchmarkItem
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Completed = true,
            });

            BenchmarkItemStore.Remove("Initialization");
        }

        public void Start(string name)
        {
            DateTime currentTime = DateTime.Now;

            BenchmarkItemStore.Add(name, new BenchmarkItem
            {
                StartTime = currentTime
            });
        }

        public void Stop(string name)
        {
            DateTime currentTime = DateTime.Now;

            BenchmarkItemStore[name].EndTime = currentTime;
            BenchmarkItemStore[name].Completed = true;
        }

        public void Output(string outputFile)
        {
            if (!File.Exists(outputFile)) File.Create(outputFile);

            int longestName = BenchmarkItemStore.Select(k => k.Key.Length).Max();

            string stringFormat = $"{{0, -{longestName + 5}}}{{1, -20}}{{2, -20}}"; // Looks ugly, but works

            using (StreamWriter sw = new StreamWriter(outputFile, true))
            {
                sw.WriteLine($"Output created at: {DateTime.Now.ToString("F")}");
                sw.WriteLine();
                sw.WriteLine(String.Format(stringFormat, "Name:", "Time:", "Time (ms):"));

                foreach (KeyValuePair<string, BenchmarkItem> kv in BenchmarkItemStore.OrderBy(kv => kv.Key))
                {
                    if (kv.Value.Completed)
                    {
                        TimeSpan interval = kv.Value.EndTime - kv.Value.StartTime;
                        sw.WriteLine(String.Format(stringFormat, kv.Key, interval.ToString(), interval.TotalMilliseconds));
                    }
                    else
                    {
                        sw.WriteLine(String.Format(stringFormat, kv.Key, "Did Not Complete", "---"));
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
