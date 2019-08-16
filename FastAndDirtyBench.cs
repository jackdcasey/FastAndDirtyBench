using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace FastAndDirtyBench
{
    /// <summary>
    /// Class used to store and manage the timers, and create summary output
    /// </summary>
    public class Benchmarker
    {
        private Dictionary<string, Stopwatch> StopwatchStore;
        private string Comment;

        /// <summary>
        /// Constructor to initialize a new Benchmarker without comment
        /// </summary>
        public Benchmarker()
        {
            StopwatchStore = new Dictionary<string, Stopwatch>();
        }

        /// <summary>
        /// Constructor to initialize a new Benchmarker with comment
        /// </summary>
        /// <param name="comment"></param>
        public Benchmarker(string comment)
        {
            StopwatchStore = new Dictionary<string, Stopwatch>();
            Comment = comment;
        }

        /// <summary>
        /// Used to start the timer for the specified tag
        /// </summary>
        /// <param name="name">The tag we are starting the timer for</param>
        public void Start(string name)
        {
            Stopwatch newStopwatch = new Stopwatch();
            StopwatchStore.Add(name, newStopwatch);
            StopwatchStore[name].Start();
        }       

        /// <summary>
        /// Used to stop the timer for the specified tag
        /// </summary>
        /// <param name="name">Tag we are stopping the timer for</param>
        public void Stop(string name)
        {
            StopwatchStore[name].Stop();
        }

        /// <summary>
        /// Used to output the result of all timers to file
        /// </summary>
        /// <param name="outputFile">The file used for output</param>
        public void Output(string outputFile)
        {
            if (!File.Exists(outputFile)) File.Create(outputFile).Close();

            int longestName = StopwatchStore.Select(k => k.Key.Length).Max();

            string stringFormat = $"{{0, -{longestName + 5}}}{{1, -20}}{{2, -20}}"; // Looks ugly, but fixes spacing

            using (StreamWriter sw = new StreamWriter(outputFile, true))
            {
                sw.WriteLine(DateTime.Now.ToString("F"));
                if (!String.IsNullOrEmpty(Comment)) sw.WriteLine(Comment);

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
}
