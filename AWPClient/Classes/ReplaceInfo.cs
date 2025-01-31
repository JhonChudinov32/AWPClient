namespace AWPClient.Classes
{
    public class ReplaceInfo
    {
        public string ElementName { get; set; }
        public string ElementProperty { get; set; }
        public string ElementValue { get; set; }
        public ReplaceInfo(string ElementName, string ElementProperty, string ElementValue)
        {
            this.ElementName = ElementName;
            this.ElementProperty = ElementProperty;
            this.ElementValue = ElementValue;
        }
    }
}
