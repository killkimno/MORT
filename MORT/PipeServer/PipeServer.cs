using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.PipeServer
{

    class PipeServer
    {
        private const string _commandTrans = "DoTrans";

        private static int numThreads = 1;
        private string _command = "";
        private string _sendData = "";

        private string _response= "";
        private bool _initResponse;
        public bool InitResponse => _initResponse;
        private bool _transEnd;
        private Process _pipeClient;


        public bool InitPipe()
        {
            if(_pipeClient == null || _pipeClient.HasExited)
            {
                int i;
                Thread[] servers = new Thread[numThreads];

                Console.WriteLine("Waiting for client connect...\n");


                for (i = 0; i < numThreads; i++)
                {
                    servers[i] = new Thread(ServerThread);
                    servers[i].Start();
                }

                foreach (var process in Process.GetProcessesByName("PipeClient"))
                {
                    process.Kill();
                }

                try
                {
                    _pipeClient = new Process();
                    _pipeClient.StartInfo.WorkingDirectory = ".\\ExternDLL";
                    _pipeClient.StartInfo.FileName = ".\\ExternDLL\\PipeClient.exe";
                    _pipeClient.Exited += OnExited;
                    //pipeClient.StartInfo.Arguments = "Start From MORT";
                    _pipeClient.Start();
                    CheckInit();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }

            return _initResponse;

        }
        public void Close()
        {
            _initResponse = false;
            if (_pipeClient != null)
            {
                _pipeClient.Close();
            }
        }

        private void OnExited(object sender, EventArgs e)
        {
            _initResponse = false;
            var process = (sender as Process);
            Console.WriteLine(process.StartInfo.FileName);
        }

        private bool CheckInit()
        {
            while(!( _response == "success" || _response == "fail")) 
            {
                Thread.Sleep(250);
            }

            if(_response == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string> DoTransAsync(string ocr)
        {
            if(string.IsNullOrEmpty(ocr))
            {
                return "";
            }
            _response = "";
            _command = _commandTrans;
            _sendData = ocr;
            _transEnd = false;
            while (!_transEnd)
            {
                await Task.Delay(50);
            }


            return _response;
        }

        private  void ServerThread(object data)
        {
            _command = "";

            NamedPipeServerStream pipeServer =
                new NamedPipeServerStream("testpipe", PipeDirection.InOut, numThreads);

            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Wait for a client to connect
            pipeServer.WaitForConnection();

            Console.WriteLine("Client connected on thread[{0}].", threadId);
            try
            {
                // Read the request from the client. Once the client has
                // written to the pipe its security token will be available.

                StreamString ss = new StreamString(pipeServer);

                // Verify our identity to the connected client using a
                // string that the client anticipates.
                ss.WriteString("I am the one true server!");
                Console.WriteLine("Start");
                while(true)
                {
                    ss.WriteString("InitCheck");
                    _response = ss.ReadString();

                    Console.WriteLine(_response);
                    if(_response == "success")
                    {
                        _initResponse = true;
                        break;
                    }
                    else if(_response == "fail")
                    {
                        _initResponse = false;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }                
                }    
                

                while(_initResponse&&  _pipeClient != null && !_pipeClient.HasExited)
                {
                    if(string.IsNullOrEmpty(_command))
                    {
                        Thread.Sleep(100);
                    }
                    else if(_command == _commandTrans)
                    {
                        Util.ShowLog($"Command on - trans");
                        _response = "";
                        ss.WriteString($"{_command},{_sendData}");
                        Util.ShowLog($"Command on - send end");
                        _sendData = "";
                        _command = "";
                        Util.ShowLog($"Command on - wait ");
                        _response = ss.ReadString();

                        Console.WriteLine($"Result = {_response}");

                        _transEnd = true;

                    }
                }

                Console.WriteLine("pipe end");

                //pipeServer.RunAsClient(fileReader.Start);
            }
            // Catch the IOException that is raised if the pipe is broken
            // or disconnected.
            catch (IOException e)
            {
                Console.WriteLine("ERROR: {0}", e.Message);
            }

            pipeServer.Close();
        }


        public class StreamString
        {
            private Stream ioStream;
            private UnicodeEncoding streamEncoding;

            public StreamString(Stream ioStream)
            {
                this.ioStream = ioStream;
                streamEncoding = new UnicodeEncoding();
            }

            public string ReadString()
            {
                int len = 0;

                len = ioStream.ReadByte() * 256;
                len += ioStream.ReadByte();
                byte[] inBuffer = new byte[len];
                ioStream.Read(inBuffer, 0, len);

                return streamEncoding.GetString(inBuffer);
            }

            public int WriteString(string outString)
            {
                byte[] outBuffer = streamEncoding.GetBytes(outString);
                int len = outBuffer.Length;
                if (len > UInt16.MaxValue)
                {
                    len = (int)UInt16.MaxValue;
                }
                ioStream.WriteByte((byte)(len / 256));
                ioStream.WriteByte((byte)(len & 255));
                ioStream.Write(outBuffer, 0, len);
                ioStream.Flush();

                return outBuffer.Length + 2;
            }
        }
    }
}
