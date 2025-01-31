using System.Collections.Generic;
using System.Data;


namespace AWPClient.Connection
{
    public interface IBridgeWorker
    {
        bool SetVariable(string name, string value);
        bool SetGridProc(string sql);
        string ExecProc(string proc, out List<KeyValuePair<string, string>> res);
        DataSet SQLQuery(string sql);
        bool SetError(string proc, string message, string description, string[] data);
    }
}
