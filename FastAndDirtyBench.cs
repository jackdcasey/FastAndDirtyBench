using System;
using System.IO;
using System.Collections.Generic;

namespace FastAndDirtyBench
{
    // TODO: Add comments 
    // TODO: Run everything async to minimize performance impact 
    // TODO: Add license
    // This is a work in progress :) 

    public class FastAndDirtyBench
    {
        public bool Enabled;

        private readonly string OutputFile;
        private Dictionary<string, BenchmarkItem> BenchmarkItemStore;

        public FastAndDirtyBench(string outputFile)
        {
            if (!File.Exists(outputFile)) File.Create(outputFile);

            OutputFile = outputFile;
            BenchmarkItemStore = new Dictionary<string, BenchmarkItem>();
        }

        public void Start(string name)
        {
            BenchmarkItemStore[name] = new BenchmarkItem
            {
                Name = name,
                StartTime = DateTime.Now,
            };
        }

        public void Finish(string name)
        {
            BenchmarkItemStore[name].EndTime = DateTime.Now;
            BenchmarkItemStore[name].Completed = true;
        }

        public void Output()
        {
            using (StreamWriter sw = new StreamWriter(OutputFile, true))
            {
                sw.WriteLine();
                sw.WriteLine("Output created at:");
                sw.WriteLine(DateTime.Now.ToString("F"));
                sw.WriteLine();
                sw.WriteLine("~ ~ ~");

                foreach (KeyValuePair<string, BenchmarkItem> kv in BenchmarkItemStore)
                {
                    if (kv.Value.Completed)
                    {
                        TimeSpan interval = kv.Value.EndTime - kv.Value.StartTime;
                        sw.WriteLine($"{kv.Key} completed in: {interval.TotalMilliseconds}ms");
                    }
                    else
                    {
                        sw.WriteLine($"{kv.Key} did not complete");
                    }
                }

                sw.WriteLine("~ ~ ~");
                sw.WriteLine();
            }
        }
    }

    public class BenchmarkItem
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Completed { get; set; } = false;
    }
}
