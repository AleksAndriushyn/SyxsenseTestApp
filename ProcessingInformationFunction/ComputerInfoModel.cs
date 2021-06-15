using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessingInformationFunction
{
    public class ComputerInfoModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime TimeZone { get; set; }
        public DateTime LastConnectedTime { get; set; }
        public DateTime LastDisconnectedTime { get; set; }
        public string ComputerName { get; set; }
        public string OSName { get; set; }
        public float NetVersion { get; set; }
    }
}
