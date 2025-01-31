using System.Threading;

namespace AWPClient.Classes
{
    public class TextElementTimer
    {
        public string ElementName { get; set; }
        public string ElementProperty { get; set; }
        public string Sql { get; set; }
        public int RefreshTimeout { get; set; }
        public Timer? Timer { get; set; }

        public TextElementTimer(string ElementName, string ElementProperty, string Sql, int RefreshTimeout)
        {
            this.ElementName = ElementName;
            this.ElementProperty = ElementProperty;
            this.Sql = Sql;
            this.RefreshTimeout = RefreshTimeout;
        }
    }
}
