using Avalonia.Input;

namespace AWPClient.Classes
{
    public class HotKeyItem
    {
        public string? ElementName { get; set; }
        public Key HotKey { get; set; }
        public string? Action { get; set; }
    }
}
