using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MachineLink.Service
{
    public class GenericAsyncResult<T> : IAsyncResult
    {
        public T Data { get; set; }

        public bool IsCompleted => true;
        public WaitHandle AsyncWaitHandle => null;
        public object AsyncState => Data;
        public bool CompletedSynchronously => true;
    }
}
