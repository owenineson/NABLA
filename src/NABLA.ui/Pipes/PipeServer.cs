using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using NABLA.ui.Pipes;

namespace NABLA.ui
{
    public class PipeServer
    {
        public NamedPipeServerStream server;

        public PipeServer()
        {
            

            
            Console.WriteLine("Connection made");

        }

        public void WriteToPipe(string data)
        {
            NamedPipeServerStream server = new NamedPipeServerStream("NABLApipe", PipeDirection.InOut);
            server.WaitForConnection();

            byte[] bytes = Encoding.UTF8.GetBytes(data);

            server.Write(bytes, 0, bytes.Length);

            server.Close();

        }

        public string ReadFromPipe()
        {
            NamedPipeServerStream server = new NamedPipeServerStream("NABLApipe", PipeDirection.InOut);
            server.WaitForConnection();
            byte[] buffer = new byte[1024];
            server.Read(buffer, 0, 1024);

            return Encoding.UTF8.GetString(buffer);

            server.Close();
        }

        public void ClosePipe()
        {
            server.Close();
        }

    }
}
