using SQLDataMaskingConfigurator.Models;
using System;
using System.Configuration;

namespace SQLDataMaskingConfigurator.Helpers
{
   public class ConfigHelper
    {
        public ConfigModel Model;
        public ConfigHelper()
        {
            Model = new ConfigModel
            {
                IsLoggingEnabled = GetSetting<bool>("IsLoggingEnabled"),
                LoggingFileMaxSizeInMB = GetSetting<int>("LoggingFileMaxSizeInMB"),
                LoggingPath = GetSetting<string>("LoggingPath"),
                RecordProcessingBatchSize = GetSetting<int>("RecordProcessingBatchSize"),
                DbSqlExecutionBatchSize = GetSetting<int>("DbSqlExecutionBatchSize"),
                DbTransactionTimeoutInMinutes = GetSetting<int>("DbTransactionTimeoutInMinutes"),
                RecordProgressPerMilliSecond = GetSetting<int>("RecordProgressPerMilliSecond"),
            };
        }

        private T GetSetting<T>(string _Key, T _DefaultValue = default) where T : IConvertible
        {
            string val = ConfigurationManager.AppSettings[_Key] ?? "";
            T result = _DefaultValue;
            if (!string.IsNullOrEmpty(val))
            {
                T typeDefault = default;
                if (typeof(T) == typeof(String))
                {
                    typeDefault = (T)(object)String.Empty;
                }
                result = (T)Convert.ChangeType(val, typeDefault.GetTypeCode());
            }
            return result;
        }
    }
}
