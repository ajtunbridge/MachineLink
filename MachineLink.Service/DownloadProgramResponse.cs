using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MachineLink.Service
{
    [DataContract]
    public class DownloadProgramResponse
    {
        [DataMember]
        public string Program { get; set; }

        [DataMember]
        public Exception Error { get; set; }
    }
}
