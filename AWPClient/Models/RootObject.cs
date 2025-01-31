using System.Collections.Generic;

namespace AWPClient.Models
{
    public class RootObject
    {
        public Bridge Bridge { get; set; }
        public List<Var> Vars { get; set; }
        public Client Client { get; set; }
    }
}
