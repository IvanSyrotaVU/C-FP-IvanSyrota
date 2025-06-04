using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.WriteLine("Scanner started.");

        string directory = @"C:\TestScanner"; 

        var allFiles = Directory.GetFiles(directory, "*.txt");
        var queueA = new ConcurrentQueue<string>();
        var queueB = new ConcurrentQueue<string>();

        var threads = new List<Thread>();

        var filesA = allFiles.Take(allFiles.Length / 2).ToArray();
        var filesB = allFiles.Skip(allFiles.Length / 2).ToArray();

        foreach (var file in filesA)
        {
            var t = new Thread(() => ProcessFile(file, queueA));
            threads.Add(t);
            t.Start();
        }

        foreach (var file in filesB)
        {
            var t = new Thread(() => ProcessFile(file, queueB));
            threads.Add(t);
            t.Start();
        }

        
        foreach (var t in threads)
            t.Join();

        SendToPipe("pipeA", queueA);
        SendToPipe("pipeB", queueB);

        Console.WriteLine("Transfer completed.");
    }

    static void ProcessFile(string filePath, ConcurrentQueue<string> queue)
    {
        var text = File.ReadAllText(filePath);
        var words = text.Split(new[] { ' ', '.', ',', '!', '?', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        var grouped = words.GroupBy(w => w.ToLower());

        foreach (var g in grouped)
        {
            queue.Enqueue($"{Path.GetFileName(filePath)};{g.Key};{g.Count()}");
        }

        Console.WriteLine($"Processed: {Path.GetFileName(filePath)}");
    }

    static void SendToPipe(string pipeName, ConcurrentQueue<string> queue)
    {
        using var pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.Out);
        pipe.Connect();

        using var writer = new StreamWriter(pipe, Encoding.UTF8) { AutoFlush = true };

        while (queue.TryDequeue(out var line))
        {
            writer.WriteLine(line);
        }

        Console.WriteLine($"Sent to pipe: {pipeName}");
    }
}


