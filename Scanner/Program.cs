using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Scanner started.");

        string directory = @"C:\TestScanner"; // <--- Replace with your path

        Thread agentA = new Thread(() => RunAgent("pipeA", directory));
        Thread agentB = new Thread(() => RunAgent("pipeB", directory));

        agentA.Start();
        agentB.Start();

        agentA.Join();
        agentB.Join();

        Console.WriteLine("Transfer completed.");
    }

    static void RunAgent(string pipeName, string dir)
    {
        var results = new List<string>();

        foreach (var file in Directory.GetFiles(dir, "*.txt"))
        {
            var text = File.ReadAllText(file);
            var words = text.Split(new[] { ' ', '.', ',', '!', '?', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var grouped = words.GroupBy(w => w.ToLower());

            foreach (var g in grouped)
            {
                results.Add($"{Path.GetFileName(file)};{g.Key};{g.Count()}");
            }
        }

        using var pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
        pipe.Connect();

        using var writer = new StreamWriter(pipe, Encoding.UTF8) { AutoFlush = true };
        foreach (var line in results)
        {
            writer.WriteLine(line);
        }
    }
}

