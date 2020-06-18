using System.ComponentModel;

namespace SQLDataMaskingConfigurator.Helpers
{
    public static class Enums
    {
        [System.Flags]
        public enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }

        public enum FormMode
        {
            Add,
            Edit
        }
        public enum SqlDbTypesNotAllowed
        {
            [Description("image")] IMAGE,
            [Description("text")] TEXT,
            [Description("uniqueidentifier")] UNIQUEIDENTIFIER,
            [Description("date")] DATE,
            [Description("time")] TIME,
            [Description("datetime2")] DATETIME2,
            [Description("datetimeoffset")] DATETIMEOFFSET,
            [Description("smalldatetime")] SMALLDATETIME,
            [Description("real")] REAL,
            [Description("smallmoney")] SMALLMONEY,
            [Description("money")] MONEY,
            [Description("datetime")] DATETIME,
            [Description("sql_variant")] SQL_VARIANT,
            [Description("ntext")] NTEXT,
            [Description("bit")] BIT,
            [Description("hierarchyid")] HIERARCHYID,
            [Description("geometry")] GEOMETRY,
            [Description("geography")] GEOGRAPHY,
            [Description("varbinary")] VARBINARY,
            [Description("binary")] BINARY,
            [Description("timestamp")] TIMESTAMP,
            [Description("xml")] XML,
            [Description("sysname")] SYSNAME,

            [Description("float")] FLOAT,
            [Description("decimal")] DECIMAL,
            [Description("numeric")] NUMERIC,

            //[Description("tinyint")] TINYINT,
            //[Description("smallint")] SMALLINT,
            //[Description("int")] INT,
            //[Description("bigint")] BIGINT,
            //[Description("varchar")] VARCHAR,
            //[Description("nvarchar")] NVARCHAR,
            //[Description("char")] CHAR,
            //[Description("nchar")] NCHAR,
        }

    }
}
