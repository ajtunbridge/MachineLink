#region Imports

using System;
using System.ServiceModel;
using System.Threading;
using MachineLink.IO;

#endregion

namespace MachineLink.Service
{
    public class MachineLinkService : IMachineLinkService
    {
        private bool _cancelCurrentAction;
        
        public IAsyncResult BeginDownloadProgram(AsyncCallback callback, object state)
        {
            var result = new GenericAsyncResult<DownloadProgramResponse>();

            var serial = new SerialTest();

            serial.DownloadProgram();

            while (serial.IsDownloading)
            {
                Thread.Sleep(1000);
                if (_cancelCurrentAction)
                {
                    break;
                }
            }

            DownloadProgramResponse response = null;

            if (_cancelCurrentAction)
            {
                _cancelCurrentAction = false;
                response = new DownloadProgramResponse { Error = new Exception("Download cancelled"), Program = null };
            }
            else
            {
                response = new DownloadProgramResponse {Error = null, Program = serial.ReceivedProgram};
            }

            result.Data = response;

            return result;
        }

        public DownloadProgramResponse EndDownloadProgram(IAsyncResult r)
        {
            var response = ((GenericAsyncResult<DownloadProgramResponse>) r).Data;

            return response;
        }

        public IAsyncResult BeginTestMethod(AsyncCallback callback, object state)
        {
            // simulate something interesting
            Thread.Sleep(3000);

            var result = new GenericAsyncResult<string> {Data = "The test method executed successfully!"};

            return result;
        }

        public string EndTestMethod(IAsyncResult r)
        {
            var result = ((GenericAsyncResult<string>) r).Data;

            return result;
        }
    }
}