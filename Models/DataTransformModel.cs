using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SQLDataMaskingConfigurator.Models
{
    public class DataTransformModel
    {
        public DataTransformModel(int Start, int End, DataTable dataTable, List<string> ColumnsToMask, string TableName)
        {
            this.Start = Start;
            this.End = End;
            this.DataTable = dataTable;
            this.ColumnsToMask = ColumnsToMask;
            this.TableName = TableName;
        }
        public int Start { get; set; }
        public int End { get; set; }
        public DataTable DataTable { get; set; }
        public List<string> ColumnsToMask { get; set; }
        public string TableName { get; set; }
        public int ResultProcessedCount { get; set; }
        public string ResultSqlUpdateCommandsForDataMasking { get; set; }
    }
}
