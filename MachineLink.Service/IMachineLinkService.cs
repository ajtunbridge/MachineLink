using System;
using System.ServiceModel;

namespace MachineLink.Service
{
    [ServiceContract]
    public interface IMachineLinkService
    {
        [OperationContract(AsyncPattern=true)]
        IAsyncResult BeginDownloadProgram(AsyncCallback callback, object state);

        DownloadProgramResponse EndDownloadProgram(IAsyncResult r);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginTestMethod(AsyncCallback callback, object state);

        string EndTestMethod(IAsyncResult r);
    }
}