using PetaPocoAzure.DatabaseTypes;
using System.Data.SqlClient;
using System.Linq;

namespace PetaPocoAzure.Internal
{
    /// <summary>
    /// Base class for DatabaseType handlers - provides default/common handling for different database engines
    /// </summary>
    abstract class DatabaseType
    {
        /// <summary>
        /// Returns the prefix used to delimit parameters in SQL query strings.
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public virtual string GetParameterPrefix(string ConnectionString)
        {
            return "@";
        }

        /// <summary>
        /// Converts a supplied C# object value into a value suitable for passing to the database
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <returns>The converted value</returns>
        public virtual object MapParameterValue(object value)
        {
            // Cast bools to integer
            if (value.GetType() == typeof(bool))
            {
                return ((bool)value) ? 1 : 0;
            }
    
            // Leave it
            return value;
        }

        /// <summary>
        /// Builds an SQL query suitable for performing page based queries to the database
        /// </summary>
        /// <param name="skip">The number of rows that should be skipped by the query</param>
        /// <param name="take">The number of rows that should be retruend by the query</param>
        /// <param name="parts">The original SQL query after being parsed into it's component parts</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL query</param>
        /// <returns>The final SQL query that should be executed.</returns>
        public virtual string BuildPageQuery(long skip, long take, PagingHelper.SQLParts parts, ref object[] args)
        {
            var sql = string.Format("{0}\nLIMIT @{1} OFFSET @{2}", parts.sql, args.Length, args.Length + 1);
            args = args.Concat(new object[] { take, skip }).ToArray();
            return sql;
        }

        /// <summary>
        /// Returns an SQL Statement that can check for the existance of a row in the database.
        /// </summary>
        /// <returns></returns>
        public virtual string GetExistsSql()
        {
            return "SELECT COUNT(*) FROM {0} WHERE {1}";
        }

        /// <summary>
        /// Escape a tablename into a suitable format for the associated database provider.
        /// </summary>
        /// <param name="tableName">The name of the table (as specified by the client program, or as attributes on the associated POCO class.</param>
        /// <returns>The escaped table name</returns>
        public virtual string EscapeTableName(string tableName)
        {
            // Assume table names with "dot" are already escaped
            return tableName.IndexOf('.') >= 0 ? tableName : EscapeSqlIdentifier(tableName);
        }

        /// <summary>
        /// Escape and arbitary SQL identifier into a format suitable for the associated database provider
        /// </summary>
        /// <param name="str">The SQL identifier to be escaped</param>
        /// <returns>The escaped identifier</returns>
        public virtual string EscapeSqlIdentifier(string str)
        {
            return string.Format("[{0}]", str);
        }

        /// <summary>
        /// Return an SQL expression that can be used to populate the primary key column of an auto-increment column.
        /// </summary>
        /// <param name="ti">Table info describing the table</param>
        /// <returns>An SQL expressions</returns>
        public virtual string GetAutoIncrementExpression(TableInfo ti)
        {
            return null;
        }

        /// <summary>
        /// Returns an SQL expression that can be used to specify the return value of auto incremented columns.
        /// </summary>
        /// <param name="primaryKeyName">The primary key of the row being inserted.</param>
        /// <returns>An expression describing how to return the new primary key value</returns>
        /// <remarks>See the SQLServer database provider for an example of how this method is used.</remarks>
        public virtual string GetInsertOutputClause(string primaryKeyName)
        {
            return string.Empty;
        }

        /// <summary>
        /// Performs an Insert operation
        /// </summary>
        /// <param name="db">The calling Database object</param>
        /// <param name="cmd">The insert command to be executed</param>
        /// <param name="PrimaryKeyName">The primary key of the table being inserted into</param>
        /// <returns>The ID of the newly inserted record</returns>
        public virtual object ExecuteInsert(Database db, SqlCommand cmd, string PrimaryKeyName)
        {
            cmd.CommandText += ";\nSELECT @@IDENTITY AS NewID;";
            return db.ExecuteScalarHelper(cmd);
        }


        /// <summary>
        /// Look at the type and provider name being used and instantiate a suitable DatabaseType instance.
        /// </summary>
        /// <returns></returns>
        public static DatabaseType Resolve()
        {
            return Singleton<SqlAzureDatabaseType>.Instance;
        }

    }
}
