namespace AWPClient.Classes
{
    public class LogxGridInfo
    {
        public string Name { get; set; }
        public string Sql { get; set; }
        public double[]? ColumnWidth { get; set; }
        public bool IsAutoWidth { get; set; }
        public LogxGridInfo(string Name, string ColumnWidth, string Sql)
        {
            this.Name = Name;
            this.Sql = Sql;

            if (ColumnWidth.Trim() != string.Empty)
            {
                string[] cols = ColumnWidth.Split(',');
                this.ColumnWidth = new double[cols.Length];

                for (int i = 0; i < cols.Length; i++)
                {
                    double d = double.NaN;

                    double.TryParse(cols[i], out d);

                    this.ColumnWidth[i] = d;
                }
            }
            else
            {
                this.IsAutoWidth = true;
            }
        }
    }
}
