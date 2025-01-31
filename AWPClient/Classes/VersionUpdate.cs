using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWPClient.Classes
{
    public class VersionUpdate
    {
        public int id { get; set; }
        public string? Version { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
    }
}
