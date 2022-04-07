using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace NABLA.sim
{
    public class PipeClient
    {
        private NamedPipeClientStream _client;

        private string _pipeName;

        public PipeClient(string PipeName)
        {
            _pipeName = PipeName;
        }

        public string ReadFromPipe()
        {
            _client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            if (_client.IsConnected == false)
            {
                _client.Connect();
            }

            Console.WriteLine("Connected for rx");

            byte[] buffer = new byte[1024];

            _client.Read(buffer, 0, 1024);
            
            return Encoding.UTF8.GetString(buffer);

            _client.Close();

        }

        public void WriteToPipe(string data)
        {
            _client = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            if (_client.IsConnected == false)
            {
                _client.Connect();
            }

            Console.WriteLine("Connected for tx");

            byte[] stringBytes  = Encoding.UTF8.GetBytes(data);

            byte[] buffer = new byte[1024];

            stringBytes.CopyTo(buffer, 0);
            
            _client.Write(buffer, 0, 1024);

            _client.Close();
        }

    }
}
