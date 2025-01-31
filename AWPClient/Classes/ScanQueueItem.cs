using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWPClient.Classes
{
    public class ScanQueueItem
    {
        public int id { get; set; }
        public string? scan { get; set; }
        public string? Status { get; set; }
        public long UpdTime { get; set; }
        public bool isManual { get; set; }
    }
}
