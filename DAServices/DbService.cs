using SQLDataMaskingConfigurator.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLDataMaskingConfigurator.Helpers.Enums;

namespace SQLDataMaskingConfigurator
{
    internal class DbService
    {
        #region Private Members / Constructor of Class

        private enum SqlQueryType
        {
            GetColumns,
            GetConstraints,
            GetTableReference,
            GetTableDependentUpon,
            EnableConstraints,
            DisableConstraints,
            FnGetMaskedValue
        }
        private readonly Logger Logger;
        private readonly ConfigHelper configHelper;
        private static string connectionString;
        private string databaseName;
        private readonly string sqlDbTypesNotAllowed;
        private SqlConnection sqlConnection;
        private readonly object _locker = new object();

        public DbService(ConfigHelper configHelper, Logger logger)
        {
            this.configHelper = configHelper;
            Logger = logger;
            sqlDbTypesNotAllowed = string.Join(",", Utility.GetDescriptions(typeof(SqlDbTypesNotAllowed)).Select(v => string.Format("'{0}'", v.ToString()).ToLower()));
        }

        #endregion

        #region Public Methods of Class

        public bool ValidateDatabaseConnection(string ConnString, string DbName)
        {
            bool _return = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {

                    connection.Open();
                    connection.Close();
                    connectionString = ConnString;
                    databaseName = DbName;
                    _return = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "ValidateDatabaseConnection");
            }
            return _return;
        }
        public Dictionary<string, string> GetListTables()
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        DataTable dt = connection.GetSchema("Tables")
                            .AsEnumerable()
                            .Where(x => x.Field<string>("TABLE_TYPE").Contains("TABLE")).CopyToDataTable();

                        Dictionary<string, string> keyValuePairs = dt.AsEnumerable()
                            .ToDictionary<DataRow, string, string>(
                            row => string.Format("{0}.{1}", row.Field<string>(1), row.Field<string>(2)),
                            //row => string.Format("{0}.{1}", row.Field<string>(1), row.Field<string>(2)));
                            row => row.Field<string>(2));

                        connection.Close();
                        return keyValuePairs;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetListTables");
                    return null;
                }
            }
            { return null; }
        }

        public DataTable GetTableData(string tableName, int page, int PgSize)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (page == 1)
                {
                    cmd = new SqlCommand("Select TOP " + PgSize + " * from " + tableName + " WITH (NOLOCK)", con);
                }
                else
                {
                    int PreviousPageOffSet = (page - 1) * PgSize;
                    string sqlQuery = "Select TOP " + PgSize + " * from (Select Row_Number() Over (Order By (select 1)) As RowNum, " +
                        "* From " + tableName + " WITH (NOLOCK) ) t2 WHERE RowNum > " + PreviousPageOffSet;

                    cmd = new SqlCommand(sqlQuery, con);
                }
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                    dt.TableName = tableName;
                }
            }
            return dt;
        }
        public DataTable GetTableData(string TableNameWithSchema, bool CountOnly = false, bool IsMaskedCount = false, bool IsMaskedValue = false)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    string SqlQuery = string.Format("SELECT {0} FROM {1} WITH (NOLOCK) {2} ;",
                     CountOnly ? "Count(1)" : "*",
                     TableNameWithSchema,
                     IsMaskedCount ? string.Format("WHERE IS_MASKED={0}", Convert.ToInt32(IsMaskedValue)) : "");

                    DataTable dt = ExecuteSQLAsDataTable(TableNameWithSchema, SqlQuery);
                    dt.TableName = TableNameWithSchema;
                    return dt;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetTableData");
                    return null;
                }
            }
            { return null; }
        }
        public int GetTotalCount(string tableName)
        {
            string strSql = "SELECT Count(*) FROM " + tableName + " WITH (NOLOCK) ";
            int intCount = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandTimeout = 120;
                        cmd.CommandText = strSql;
                        conn.Open();
                        intCount = (int)cmd.ExecuteScalar();
                        conn.Close();
                    }
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
                }
            }

            return intCount;
        }
        public DataTable GetTableColumnDetails(string TableNameWithSchema)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    string SqlQuery = GetSQLQueryByType(SqlQueryType.GetColumns);
                    return ExecuteSQLAsDataTable(TableNameWithSchema, SqlQuery);

                    #region Not In Use
                    //DataTable dtSource2 = connection.GetSchema("Columns", new[] { databaseName, null, TableName });
                    //dtSource2.Columns.Remove("TABLE_CATALOG");
                    //dtSource2.Columns.Remove("TABLE_SCHEMA");
                    //dtSource2.Columns.Remove("TABLE_NAME");
                    ////dtSource2.Columns.Remove("ORDINAL_POSITION");
                    //dtSource2.Columns.Remove("CHARACTER_OCTET_LENGTH");
                    //dtSource2.Columns.Remove("NUMERIC_PRECISION_RADIX");
                    //dtSource2.Columns.Remove("CHARACTER_SET_CATALOG");
                    //dtSource2.Columns.Remove("CHARACTER_SET_SCHEMA");
                    //dtSource2.Columns.Remove("CHARACTER_SET_NAME");
                    //dtSource2.Columns.Remove("COLLATION_CATALOG");
                    //dtSource2.Columns.Remove("IS_SPARSE");
                    //dtSource2.Columns.Remove("IS_COLUMN_SET");
                    //dtSource2.Columns.Remove("IS_FILESTREAM"); 
                    #endregion
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetTableColumnDetails");
                    return null;
                }
            }
            { return null; }
        }
        public DataTable GetTableReferenceDetails(string TableNameWithSchema)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    string SqlQuery = GetSQLQueryByType(SqlQueryType.GetTableReference);
                    return ExecuteSQLAsDataTable(TableNameWithSchema, SqlQuery);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetTableReferenceDetails");
                    return null;
                }
            }
            { return null; }
        }
        public DataTable GetTableConstraintsDetails(string TableNameWithSchema)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    string SqlQuery = GetSQLQueryByType(SqlQueryType.GetConstraints);
                    DataTable dtSource = ExecuteSQLAsDataTable(TableNameWithSchema, SqlQuery);
                    dtSource.Columns.Remove("TABLE_VIEW");
                    dtSource.Columns.Remove("OBJECT_TYPE");
                    //dtSource.Columns.Remove("CONSTRAINT_NAME");
                    return dtSource;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetTableReferenceDetails");
                    return null;
                }
            }
            { return null; }
        }
        public DataTable GetTableDependentWithDetails(string TableNameWithSchema)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                try
                {
                    string SqlQuery = GetSQLQueryByType(SqlQueryType.GetTableDependentUpon);
                    DataTable dtSource = ExecuteSQLAsDataTable(TableNameWithSchema, SqlQuery);
                    return dtSource;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetTableReferenceDetails");
                    return null;
                }
            }
            { return null; }
        }
        public DataSet GetTablesData(List<string> TableListToFetchData, bool IsCountOnly = false)
        {
            if (TableListToFetchData != null)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    DataSet ds;
                    if (IsCountOnly)
                    {
                        sb.AppendLine("SELECT SUM(ISNULL(TBLA.COLA,0)) FROM ( ");
                        for (int i = 0; i < TableListToFetchData.Count; i++)
                        {
                            if (i == 0)
                            {
                                sb.AppendLine(string.Format("SELECT count(*) AS ColA FROM {0} WITH (NOLOCK) ", TableListToFetchData[i]));
                            }
                            else
                            {
                                sb.AppendLine(string.Format("UNION SELECT count(*) AS ColA FROM {0} WITH (NOLOCK) ", TableListToFetchData[i]));
                            }
                        }
                        sb.AppendLine(" ) tblA; ");

                        ds = ExecuteSQLAsDataSet(sb.ToString());
                    }
                    else
                    {
                        foreach (string item in TableListToFetchData)
                        {
                            sb.AppendLine(string.Format("SELECT * FROM {0} WITH (NOLOCK) ;", item));
                        }

                        ds = ExecuteSQLAsDataSet(sb.ToString());
                        for (int i = 0; i < TableListToFetchData.Count; i++)
                        {
                            ds.Tables[i].TableName = TableListToFetchData[i];
                        }
                    }

                    return ds;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "GetTablesData");
                    return null;
                }
            }
            { return null; }
        }
        public bool AddOrResetMaskColumnInTable(List<string> TableListToFetchData, bool WithUpdateSqlCommand = true, bool HasToReset = true, bool HasToPushMaskingFunctionInDb = false)
        {
            try
            {
                dynamic taskToAddFunc = null;
                if (HasToPushMaskingFunctionInDb)
                {
                    taskToAddFunc = Task.Run(() => { return AddSqlDataMaskingFuntionToDb(); });
                }

                string batchSql = string.Empty;
                string _validateColumnExistance = @"IF EXISTS (SELECT OBJECT_ID,[NAME] FROM sys.columns 
                                                    WHERE  object_id = OBJECT_ID('{0}') AND name = 'IS_MASKED')
                                                    BEGIN 
	                                                    SELECT 1 AS IS_EXIST; 
                                                    END
                                                    ELSE
                                                    BEGIN
	                                                    SELECT 0 AS IS_EXIST; 
                                                    END;";

                string _alterTableAddColumn = @"ALTER TABLE {0} ADD 
                                                IS_MASKED Bit NOT NULL,
                                                CONSTRAINT [DF_{1}_IS_MASKED] DEFAULT 0 FOR IS_MASKED;";

                string _UpdateDefaultValuesToColumn = @"UPDATE {0} SET IS_MASKED=0;";

                StringBuilder sb = new StringBuilder();
                foreach (string TableNameWithSchema in TableListToFetchData)
                {
                    string TableNameOnly = TableNameWithSchema.Split('.')[1];
                    _validateColumnExistance = string.Format(_validateColumnExistance, TableNameWithSchema);
                    _alterTableAddColumn = string.Format(_alterTableAddColumn, TableNameWithSchema, TableNameOnly);
                    _UpdateDefaultValuesToColumn = string.Format(_UpdateDefaultValuesToColumn, TableNameWithSchema);

                    using (DataTable dt = ExecuteSQLAsDataTable(TableNameWithSchema, _validateColumnExistance))
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dt.Rows[0]["IS_EXIST"]))
                            {
                                if (HasToReset)
                                {
                                    sb.AppendLine(_UpdateDefaultValuesToColumn);
                                }
                            }
                            else
                            {
                                if (WithUpdateSqlCommand)
                                {
                                    sb.AppendLine(_alterTableAddColumn);
                                }
                            }
                        }
                    }
                }

                batchSql = sb.ToString();
                bool isExecutionSucceed = batchSql.Trim().Length > 0 ? ExecuteDynamicSql(batchSql) : true;

                if (HasToPushMaskingFunctionInDb)
                {
                    taskToAddFunc.Wait();
                    if (isExecutionSucceed) { isExecutionSucceed = taskToAddFunc.Result; }
                }

                return isExecutionSucceed;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddMaskColumnInTable");
                return false;
            }
        }
        public bool UpdateConstraintsOnTable(List<string> TableListToFetchData, bool DisableConstraints = false)
        {
            try
            {
                string batchSql = string.Empty;
                StringBuilder sb = new StringBuilder();
                foreach (string item in TableListToFetchData)
                {
                    if (DisableConstraints)
                    {
                        sb.AppendLine(string.Format(GetSQLQueryByType(SqlQueryType.DisableConstraints), item));
                    }
                    else
                    {
                        sb.AppendLine(string.Format(GetSQLQueryByType(SqlQueryType.EnableConstraints), item));
                    }
                }

                batchSql = sb.ToString();
                return ExecuteDynamicSql(batchSql);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "UpdateConstraintsOnTables");
                return false;
            }
        }
        public bool PushDataIntoDb(string tableName, Dictionary<string, string> columnsToMask, int batchSize = 0)
        {
            if (!string.IsNullOrEmpty(tableName) && columnsToMask != null & columnsToMask.Count > 0)
            {
                try
                {
                    //"UPDATE <<TableName>> SET <<FieldName>> = <<EncryptionFunction>>(<<FieldName>>), IS_MASKED=1 WHERE IS_MASKED=0";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("UPDATE {0} {1} SET ",
                        batchSize > 0 ? "Top(" + batchSize + ")" : "",
                        tableName));
                    for (int counter = 0; counter < columnsToMask.Count; counter++)
                    {
                        string columnName = columnsToMask.ElementAt(counter).Key;
                        string columnDataType = columnsToMask.ElementAt(counter).Value;
                        long maxVal = GetMaxValOfTypes(columnDataType);
                        if (counter > 0) { sb.AppendLine(", "); }
                        sb.Append(string.Format("[{0}] = [dbo].[fnGetMaskedValue]([{1}], {2}, {3})", columnName, columnName, "'" + columnDataType + "'", maxVal));
                    }
                    sb.AppendLine(", IS_MASKED=1 WHERE IS_MASKED=0");
                    return ExecuteDynamicSql(sb.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "PushDataIntoDb");
                    return false;
                }
            }
            { return false; }

        }
        public bool AddSqlDataMaskingFuntionToDb()
        {
            try
            {
                return ExecuteDynamicSql(GetSQLQueryByType(SqlQueryType.FnGetMaskedValue));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddMaskingFuntionToDb");
                return false;
            }
        }
        public bool ExecuteDynamicSql(string SqlCommandString)
        {
            if (!string.IsNullOrEmpty(SqlCommandString))
            {
                try
                {
                    int i = ExecuteSQLAsNonQuery(SqlCommandString);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "ExecuteDynamicSql");
                    return false;
                }
            }
            { return false; }
        }

        #endregion

        #region Private Methods of Class

        private long GetMaxValOfTypes(string columnDataType)
        {
            long maxVal = 0;
            switch (columnDataType.ToString().Trim().ToLower())
            {
                case "tinyint":
                    maxVal = 255;
                    break;
                case "smallint":
                    maxVal = 32768;
                    break;
                case "int":
                    maxVal = 0;
                    break;
                case "bigint":
                    maxVal = 0;
                    break;
                default:
                    maxVal = 0;
                    break;
            }
            return maxVal;
        }
        private SqlConnection GetSqlConnection()
        {
            lock (_locker)
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    if (sqlConnection == null) { sqlConnection = new SqlConnection(connectionString); return sqlConnection; }
                    else { return sqlConnection; }
                }
                else { return sqlConnection; }
            }
        }
        private void DisposeSqlConnection()
        {
            if (sqlConnection != null)
            {
                using (sqlConnection)
                {
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                }
            }
        }
        private int ExecuteSQLAsNonQuery(string SQL)
        {
            int _result = 0;
            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandTimeout = configHelper.Model.DbTransactionTimeoutInMinutes * 60;
                            cmd.CommandText = SQL;
                            conn.Open();
                            _result = cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open) { conn.Close(); conn.Dispose(); }
                    }
                }
            }
            return _result;
        }
        private DataSet ExecuteSQLAsDataSet(string SQL)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = SQL;
                        da.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            da.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
        }
        private DataTable ExecuteSQLAsDataTable(string TableNameWithSchema, string SqlQuery)
        {
            string tableName = TableNameWithSchema.Split('.')[1];
            SqlParameter[] sqlParam = new SqlParameter[] {
                new SqlParameter("@TableNameWithSchema", TableNameWithSchema),
                new SqlParameter("@TableName", tableName)
            };
            return ExecuteSQLAsDataTable(TableNameWithSchema, SqlQuery, sqlParam);
        }
        private DataTable ExecuteSQLAsDataTable(string TableNameWithSchema, string SqlQuery, SqlParameter[] sqlParameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DataTable dtSource = new DataTable();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = SqlQuery;
                    cmd.Parameters.AddRange(sqlParameters);
                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = connection;
                    using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        dtSource.Load(sdr);
                    }
                }
                if (connection != null && connection.State == ConnectionState.Open) { connection.Close(); }
                return dtSource;
            }
        }
        private string GetSQLQueryByType(SqlQueryType sqlQueryType)
        {

            // CHARACTER_MAXIMUM_LENGTH -1 For MAX Length And NULL is for undefined i.e int , float etc. so we have excluded max length and included undefined
            string _tableColumns = @"SELECT DISTINCT TBLA.COLUMN_ID, TBLB.COLUMN_NAME, TBLB.DATA_TYPE,TBLA.MAX_LENGTH, TBLA.PRECISION,
                                    CASE TBLB.NUMERIC_SCALE WHEN 0 THEN 'NO' ELSE 'YES' END AS NUMERIC_SCALE,
                                    TBLB.IS_NULLABLE, TBLB.COLUMN_DEFAULT
                                    --,ISNULL(CONSTRAINTS.IS_CONSTRAINT,0) AS IS_CONSTRAINT
                                    FROM SYS.COLUMNS TBLA 
                                    INNER JOIN (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName) TBLB
                                    ON TBLA.COLUMN_ID=TBLB.ORDINAL_POSITION 
                                    LEFT JOIN (
	                                    SELECT 1 as IS_CONSTRAINT, TABLE_VIEW, CONSTRAINT_TYPE AS [CONSTRAINT], CONSTRAINT_NAME, Column_Name
	                                    FROM (
		                                    SELECT table_view
			                                    ,object_type
			                                    ,constraint_type
			                                    ,constraint_name
			                                    ,details
			                                    ,Column_Name
		                                    FROM (	SELECT '[' + schema_name(t.schema_id) + '].['  + t.[name] + ']' AS table_view
					                                    ,CASE 
						                                    WHEN t.[type] = 'U'
							                                    THEN 'Table'
						                                    WHEN t.[type] = 'V'
							                                    THEN 'View'
						                                    END AS [object_type]
					                                    ,CASE 
						                                    WHEN c.[type] = 'PK'
							                                    THEN 'Primary key'
						                                    WHEN c.[type] = 'UQ'
							                                    THEN 'Unique constraint'
						                                    WHEN i.[type] = 1
							                                    THEN 'Unique clustered index'
						                                    WHEN i.type = 2
							                                    THEN 'Unique index'
						                                    END AS constraint_type
					                                    ,isnull(c.[name], i.[name]) AS constraint_name
					                                    ,substring(column_names, 1, len(column_names) - 1) AS [details]
					                                    ,substring(column_names, 1, len(column_names)-1)  AS Column_Name
				                                    FROM sys.objects t
				                                    LEFT OUTER JOIN sys.indexes i ON t.object_id = i.object_id
				                                    LEFT OUTER JOIN sys.key_constraints c ON i.object_id = c.parent_object_id
					                                    AND i.index_id = c.unique_index_id
				                                    CROSS APPLY (
					                                    SELECT col.[name] + ', '
					                                    FROM sys.index_columns ic
					                                    INNER JOIN sys.columns col ON ic.object_id = col.object_id
						                                    AND ic.column_id = col.column_id
					                                    WHERE ic.object_id = t.object_id
						                                    AND ic.index_id = i.index_id
					                                    ORDER BY col.column_id
					                                    FOR XML path('')
					                                    ) D(column_names)
				                                    WHERE is_unique = 1
					                                    AND t.is_ms_shipped <> 1
			                                    UNION ALL
				                                    SELECT '[' + OBJECT_SCHEMA_NAME(f.parent_object_id) + '].['  + OBJECT_NAME(f.parent_object_id) + ']' AS foreign_table
					                                    ,'Table'
					                                    ,'Foreign key'
						                                    ,f.name AS fk_constraint_name
						                                    ,COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS PKRefdBaseColumn
						                                    ,COL_NAME(fc.parent_object_id, fc.parent_column_id) AS Column_Name
					                                    FROM sys.foreign_keys AS f
					                                    JOIN sys.foreign_key_columns AS fc ON (f.object_id = fc.constraint_object_id)
			                                    UNION ALL
				                                    SELECT '[' + schema_name(t.schema_id) + '].['  + t.[name] + ']'
					                                    ,'Table'
					                                    ,'Check constraint'
					                                    ,con.[name] AS constraint_name
					                                    ,con.[definition]
					                                    ,COL_NAME(col.object_id, col.column_id) AS Column_Name
				                                    FROM sys.check_constraints con
				                                    LEFT OUTER JOIN sys.objects t ON con.parent_object_id = t.object_id
				                                    LEFT OUTER JOIN sys.all_columns col ON con.parent_column_id = col.column_id
					                                    AND con.parent_object_id = col.object_id
			                                    UNION ALL
				                                    SELECT '[' + schema_name(t.schema_id) + '].[' + t.[name] + ']'
					                                    ,'Table'
					                                    ,'Default constraint'
					                                    ,con.[name]
					                                    ,col.[name] + ' = ' + con.[definition]
					                                    ,COL_NAME(col.object_id, col.column_id) AS Column_Name
				                                    FROM sys.default_constraints con
				                                    LEFT OUTER JOIN sys.objects t ON con.parent_object_id = t.object_id
				                                    LEFT OUTER JOIN sys.all_columns col ON con.parent_column_id = col.column_id
					                                    AND con.parent_object_id = col.object_id
			                                    ) a
		                                    ) tblA
                                    )  CONSTRAINTS ON COL_NAME(TBLA.object_id, TBLA.column_id)  = CONSTRAINTS.COLUMN_NAME
									AND TBLA.OBJECT_ID = OBJECT_ID(CONSTRAINTS.table_view)
                                    WHERE TBLA.OBJECT_ID = OBJECT_ID(@TableNameWithSchema)
                                    AND TBLA.IS_IDENTITY = 0 AND TBLA.IS_COMPUTED = 0
                                    AND ISNULL(CONSTRAINTS.IS_CONSTRAINT,0) = 0
                               AND ((CHARACTER_MAXIMUM_LENGTH BETWEEN 0 AND 500) OR CHARACTER_MAXIMUM_LENGTH IS NULL)
                               AND RTRIM(LTRIM((LOWER(DATA_TYPE)))) NOT IN( " + sqlDbTypesNotAllowed + ")";

            string _tableConstrains = @"SELECT TABLE_VIEW, OBJECT_TYPE, CONSTRAINT_TYPE AS [CONSTRAINT], DETAILS AS [COLUMNS], CONSTRAINT_NAME, Column_Name
                                        FROM (SELECT table_view
		                                            ,object_type
		                                            ,constraint_type
		                                            ,constraint_name
		                                            ,details
		                                            ,Column_Name
	                                            FROM (	SELECT '[' + schema_name(t.schema_id) + '].['  + t.[name] + ']' AS table_view
					                                    ,CASE 
						                                    WHEN t.[type] = 'U'
							                                    THEN 'Table'
						                                    WHEN t.[type] = 'V'
							                                    THEN 'View'
						                                    END AS [object_type]
					                                    ,CASE 
						                                    WHEN c.[type] = 'PK'
							                                    THEN 'Primary key'
						                                    WHEN c.[type] = 'UQ'
							                                    THEN 'Unique constraint'
						                                    WHEN i.[type] = 1
							                                    THEN 'Unique clustered index'
						                                    WHEN i.type = 2
							                                    THEN 'Unique index'
						                                    END AS constraint_type
					                                    ,isnull(c.[name], i.[name]) AS constraint_name
					                                    ,substring(column_names, 1, len(column_names) - 1) AS [details]
					                                    ,substring(column_names, 1, len(column_names)-1)  AS Column_Name
				                                    FROM sys.objects t
				                                    LEFT OUTER JOIN sys.indexes i ON t.object_id = i.object_id
				                                    LEFT OUTER JOIN sys.key_constraints c ON i.object_id = c.parent_object_id
					                                    AND i.index_id = c.unique_index_id
				                                    CROSS APPLY (
					                                    SELECT col.[name] + ', '
					                                    FROM sys.index_columns ic
					                                    INNER JOIN sys.columns col ON ic.object_id = col.object_id
						                                    AND ic.column_id = col.column_id
					                                    WHERE ic.object_id = t.object_id
						                                    AND ic.index_id = i.index_id
					                                    ORDER BY col.column_id
					                                    FOR XML path('')
					                                    ) D(column_names)
				                                    WHERE is_unique = 1
					                                    AND t.is_ms_shipped <> 1
			                                    UNION ALL
				                                    SELECT '[' + OBJECT_SCHEMA_NAME(f.parent_object_id) + '].['  + OBJECT_NAME(f.parent_object_id) + ']' AS foreign_table
					                                    ,'Table'
					                                    ,'Foreign key'
						                                    ,f.name AS fk_constraint_name
						                                    ,COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS PKRefdBaseColumn
						                                    ,COL_NAME(fc.parent_object_id, fc.parent_column_id) AS Column_Name
					                                    FROM sys.foreign_keys AS f
					                                    JOIN sys.foreign_key_columns AS fc ON (f.object_id = fc.constraint_object_id)
			                                    UNION ALL
				                                    SELECT '[' + schema_name(t.schema_id) + '].['  + t.[name] + ']'
					                                    ,'Table'
					                                    ,'Check constraint'
					                                    ,con.[name] AS constraint_name
					                                    ,con.[definition]
					                                    ,COL_NAME(col.object_id, col.column_id) AS Column_Name
				                                    FROM sys.check_constraints con
				                                    LEFT OUTER JOIN sys.objects t ON con.parent_object_id = t.object_id
				                                    LEFT OUTER JOIN sys.all_columns col ON con.parent_column_id = col.column_id
					                                    AND con.parent_object_id = col.object_id
			                                    UNION ALL
				                                    SELECT '[' + schema_name(t.schema_id) + '].[' + t.[name] + ']'
					                                    ,'Table'
					                                    ,'Default constraint'
					                                    ,con.[name]
					                                    ,col.[name] + ' = ' + con.[definition]
					                                    ,COL_NAME(col.object_id, col.column_id) AS Column_Name
				                                    FROM sys.default_constraints con
				                                    LEFT OUTER JOIN sys.objects t ON con.parent_object_id = t.object_id
				                                    LEFT OUTER JOIN sys.all_columns col ON con.parent_column_id = col.column_id
					                                    AND con.parent_object_id = col.object_id
			                                    ) a
		                                    ) tblA
                                        WHERE OBJECT_ID(tblA.table_view) =  OBJECT_ID(@TableNameWithSchema)
                                    ORDER BY table_view
	                                    ,constraint_type
	                                    ,constraint_name";

            string _tableReference = @"SELECT (OBJECT_SCHEMA_NAME(f.parent_object_id) + '.'+ OBJECT_NAME(f.parent_object_id)) as [TABLE] , 
                       COL_NAME(fc.parent_object_id, fc.parent_column_id) as [COLUMN] 
                       FROM sys.foreign_keys AS f 
                       INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id 
                       INNER JOIN sys.tables t ON t.OBJECT_ID = fc.referenced_object_id 
                       WHERE OBJECT_NAME(f.referenced_object_id) = @TableName";

            string _tableDependentUpon = @"SELECT  BASE_COLUMN, REF_TABLE, REF_COLUMN FROM (
                                            SELECT BASE_TABLE = FK.TABLE_NAME
	                                            ,BASE_COLUMN = CU.COLUMN_NAME
	                                            ,REF_TABLE = PK.TABLE_SCHEMA + '.' + PK.TABLE_NAME
	                                            ,REF_COLUMN = PT.COLUMN_NAME
	                                            ,CONSTRAINT_NAME = C.CONSTRAINT_NAME
                                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
                                            INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                                            INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                                            INNER JOIN (
	                                            SELECT i1.TABLE_NAME
		                                            ,i2.COLUMN_NAME
	                                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
	                                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
		                                             WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
	                                            ) PT ON PT.TABLE_NAME = PK.TABLE_NAME 
                                                    WHERE FK.TABLE_NAME LIKE '%' + @TableName
	                                            ) TBL ORDER BY 1 ASC";

            string _disableConstrainst = "ALTER TABLE {0} NOCHECK CONSTRAINT ALL;";

            string _enableConstraint = "ALTER TABLE {0} CHECK CONSTRAINT ALL;";

            string _fnGetMaskedValue = @"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetMaskedValue]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fnGetMaskedValue] (@InputText VARCHAR(max), @DataType VARCHAR(100), @NumericMaxLength BIGINT)
RETURNS VARCHAR(max)
AS
BEGIN
	DECLARE @Shit_Pos AS TINYINT = 3
	DECLARE @OutputText AS VARCHAR(max) = ''''
	DECLARE @pc AS VARCHAR(1)
	DECLARE @i AS SMALLINT = 1
	DECLARE @n AS SMALLINT
	DECLARE @chekVal AS VARCHAR(400)=''''
			
    SET @n = len(@InputText)
    SET @InputText = upper(@InputText)
	SET @chekVal= upper(@InputText)

	IF(@DataType = ''TINYINT'')
	BEGIN
		SET @OutputText =  CAST ( (CAST( @InputText as tinyint ) + 1) AS VARCHAR)

		--------- validate to Overflow datatype size --------- 
		IF (@OutputText > @NumericMaxLength AND @NumericMaxLength > 0) 
		BEGIN 
			SET @OutputText = CAST (CAST(@i AS int) AS VARCHAR)
		END 
	END
	ELSE IF(@DataType = ''SMALLINT'')
	BEGIN 
		SET @OutputText =   CAST ( (CAST( @InputText as int ) + 1) AS VARCHAR)
        --------- validate to Overflow datatype size --------- 
		IF (@OutputText > @NumericMaxLength AND @NumericMaxLength > 0) 
		BEGIN 
			SET @OutputText = CAST (CAST(@i AS int) AS VARCHAR)
		END
	END
	ELSE
	BEGIN
		WHILE @i < = @n
		BEGIN
                                                   
            SET @pc = SUBSTRING(@InputText, @i, 1)

				IF ascii(@pc) BETWEEN 65 AND 90
				BEGIN
					IF ascii(@pc) + @Shit_Pos > 90
						SET @pc = CHAR((ascii(@pc) + @Shit_Pos) - 90 + 64)
					ELSE
						SET @pc = CHAR((ascii(@pc) + @Shit_Pos))
				END
				ELSE
				BEGIN
					IF ascii(@pc) BETWEEN 48 AND 57
					BEGIN
						IF ascii(@pc) + @Shit_Pos > 57
							SET @pc = CHAR((ascii(@pc) + @Shit_Pos) - 57 + 47)
						ELSE
							SET @pc = CHAR((ascii(@pc) + @Shit_Pos))
					END
				END

				SET @OutputText = @OutputText + @pc

			SET @i = @i + 1
		END
	END

    IF (@DataType<>''VARCHAR'' AND @DataType<>''CHAR'' AND @DataType<>''NVARCHAR'' AND @DataType<>''NCHAR'')
	BEGIN
	    IF ((SELECT ISNUMERIC(REPLACE(REPLACE(@OutputText,''E'',''X''),''D'',''X''))) = 1)
	    BEGIN
		    IF (CAST(CASE 
								    WHEN Replace(Replace(ISNULL(@OutputText, ''''), ''.'', ''''), '','', '''') = ''''
									    THEN ''0''
								    ELSE Replace(Replace(ISNULL(@OutputText, ''''), ''.'', ''''), '','', '''')
								    END AS DECIMAL(38, 0)) = 0)
		    BEGIN
			    SET @OutputText = CAST((
						    CAST(CASE 
								    WHEN Replace(Replace(ISNULL(@chekVal, ''''), ''.'', ''''), '','', '''') = ''''
									    THEN ''0''
								    ELSE Replace(Replace(ISNULL(@chekVal, ''''), ''.'', ''''), '','', '''')
								    END AS DECIMAL(38, 0)) + 1
						    ) AS VARCHAR)
		    END
	    END
    END

    RETURN @OutputText
END;
' 
END";

            string _return = string.Empty;
            switch (sqlQueryType)
            {
                case SqlQueryType.GetColumns:
                    _return = _tableColumns;
                    break;
                case SqlQueryType.GetConstraints:
                    _return = _tableConstrains;
                    break;
                case SqlQueryType.GetTableReference:
                    _return = _tableReference;
                    break;
                case SqlQueryType.GetTableDependentUpon:
                    _return = _tableDependentUpon;
                    break;
                case SqlQueryType.EnableConstraints:
                    _return = _enableConstraint;
                    break;
                case SqlQueryType.DisableConstraints:
                    _return = _disableConstrainst;
                    break;
                case SqlQueryType.FnGetMaskedValue:
                    _return = _fnGetMaskedValue;
                    break;
                default:
                    _return = string.Empty;
                    break;
            }
            return _return;

        }

        #endregion
    }
}
