using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MORT.PipeServer
{

    class PipeServer
    {
        private static int numThreads = 1;
        private string _command = "";
        private string _response= "";
        private bool _initResponse;
       
        public void InitPipe()
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
                Process pipeClient = new Process();
                pipeClient.StartInfo.WorkingDirectory = ".\\ExternDLL";
                pipeClient.StartInfo.FileName = "PipeClient.exe";
                //pipeClient.StartInfo.Arguments = "Start From MORT";
                pipeClient.Start();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool CheckInit()
        {
            while(_response == "success" || _response == "fail") 
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

        private  void ServerThread(object data)
        {
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
                    if(_response == "success" || _response == "fail" )
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(250);
                    }
                
                }
             

              
                string temp = "";
                while ((temp = Console.ReadLine()) != "Break")
                {
                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = "Empty";
                    }
                    ss.WriteString(temp);

                    string result = ss.ReadString();

                    Console.WriteLine(result);
                }


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
