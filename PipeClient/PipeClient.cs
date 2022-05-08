using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PipeClient
{
    public class PipeClient
    {
        public enum StatusType
        {
            None,
            Initing,
            InitSuccess,
            InitFail

        }
        private PipeEzTransAPI _eztransAPI;
        private NamedPipeClientStream _pipeClient;
        private StreamString _streamString;
        private StatusType _statusType = StatusType.None;

        private void InitEzTrans()
        {

            bool isSuccess = false;
            if (_eztransAPI == null)
            {
                _eztransAPI = new PipeEzTransAPI();
                _eztransAPI.Init();
            }
            else if (!_eztransAPI.IsInit)
            {
                _eztransAPI.Init();
            }

            if (_eztransAPI != null)
            {
                isSuccess = _eztransAPI.IsInit;
            }

            if(isSuccess)
            {
                _statusType = StatusType.InitSuccess;
            }
            else
            {
                _statusType = StatusType.InitFail;
            }
        }

        private void CheckCommand()
        {
            // Validate the server's signature string.
            if (_streamString.ReadString() == "I am the one true server!")
            {
                Console.WriteLine("Lets do this");

                string temp = "";

                do
                {
                    temp = _streamString.ReadString();

                    if (temp == null)
                    {
                        temp = "Null";
                    }

                    Console.WriteLine("Enter : " + temp);

                    string[] datas = temp.Split(',');
                    string commmand = "";

                    if(datas.Length > 0)
                    {
                        commmand = datas[0];
                    }

                    if(commmand == "InitCheck")
                    {
                        switch(_statusType)
                        {
                            case StatusType.InitSuccess:
                                _streamString.WriteString("success");
                                break;

                            case StatusType.InitFail:
                                _streamString.WriteString("fail");
                                break;

                            default:
                                _streamString.WriteString("wait");
                                break;
                        }
                    }
                    else if(commmand == "DoTrans")
                    {
                        if(datas.Length > 1)
                        {
                            string result = _eztransAPI.DoTrsans(datas[1]);
                            _streamString.WriteString(result);
                        }
                        else
                        {
                            _streamString.WriteString("fail");
                        }
                      
                    }
                    else if (commmand == "Entry")
                    {
                        Thread.Sleep(500);
                        _streamString.WriteString("SDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFDSFSDF");
                    }
                    else if (commmand == "Break")
                    {
                        _streamString.WriteString("SDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFSDFDSFSDF");
                        break;
                    }
                    else
                    {
                        _streamString.WriteString("");
                    }
                }
                while (!string.IsNullOrEmpty(temp));
            }
            _pipeClient.Close();
            // Give the client process some time to display results before exiting.
            Thread.Sleep(4000);
        }

        public void Start(NamedPipeClientStream pipeClient)
        {
            _pipeClient = pipeClient;

            _streamString = new StreamString(pipeClient);
            InitEzTrans();
            CheckCommand();
        }
    }
}
