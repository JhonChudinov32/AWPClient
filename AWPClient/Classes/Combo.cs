using System.Collections.Generic;

namespace AWPClient.Classes
{
    public class Combo
    {
        public string? combobox { get; set; }
        public string? key { get; set; }
        public string? var { get; set; }
        public List<string> btns { get; set; }
        public string? sql { get; set; }

        public Combo()
        {
            btns = new List<string>();
        }
    }
}
