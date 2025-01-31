using System;

namespace AWPClient.LogServices
{
    public class LogInfo
    {
        public string? Subject { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public DateTime LogDateTime { get; set; }

        public  LogInfo()
        {
            LogDateTime = DateTime.Now;
        }
    }
}
