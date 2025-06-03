using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Master
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            List<string> output = new();

            Thread t1 = new Thread(() => ReadPipe("pipeA", output));
            Thread t2 = new Thread(() => ReadPipe("pipeB", output));

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ResultForm(output));
        }

        static void ReadPipe(string pipeName, List<string> output)
        {
            using var server = new NamedPipeServerStream(pipeName, PipeDirection.In);
            server.WaitForConnection();

            using var reader = new StreamReader(server, Encoding.UTF8);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                lock (output)
                {
                    output.Add(line);
                }
            }
        }
    }
}

