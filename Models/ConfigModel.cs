namespace SQLDataMaskingConfigurator.Models
{
   public class ConfigModel
    {
        //Logger Congiguration
        public bool IsLoggingEnabled { get; set; }
        public int LoggingFileMaxSizeInMB { get; set; }
        public string LoggingPath { get; set; }
        public int DbSqlExecutionBatchSize { get; set; }
        public int RecordProcessingBatchSize { get; set; }
        public int DbTransactionTimeoutInMinutes { get; set; }
       public int RecordProgressPerMilliSecond { get; set; }
    }
}
