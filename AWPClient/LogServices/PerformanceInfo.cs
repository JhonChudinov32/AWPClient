using System;

namespace AWPClient.LogServices
{
    public class PerformanceInfo
    {
        public string? MethodName { get; set; }
        public string WatchName { get; set; }
        public DateTime datetime { get; set; }
        public long ElapsedMilliseconds { get; set; }

        public PerformanceInfo(string WatchName, DateTime datetime, long ElapsedMilliseconds)
        {
            this.WatchName = WatchName;
            this.datetime = datetime;
            this.ElapsedMilliseconds = ElapsedMilliseconds;
        }
    }
}
