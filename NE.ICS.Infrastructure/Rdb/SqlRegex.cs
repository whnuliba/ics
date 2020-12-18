using System.Text.RegularExpressions;

namespace NE.ICS.Infrastructure
{
    public class SqlRegex
    {
        // TODO
        // 考虑到效率，只判断以 SELECT 开始的语句就认为是只读的
        private static readonly Regex DefaultSqlReadOnlyRegex = new Regex(@"^\s*select\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex DefaultSqlInjectionRegex = new Regex(@"\s?or\s*|\s?;\s?|\s?drop\s|\s?grant\s|^'|\s?--|\s?union\s|\s?delete\s|\s?truncate\s|\s?sysobjects\s?|\s?xp_.*?|\s?syslogins\s?|\s?sysremote\s?|\s?sysusers\s?|\s?sysxlogins\s?|\s?sysdatabases\s?|\s?aspnet_.*?|\s?exec\s?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex DefaultColumnNameRegex = new Regex(@"^[a-zA-Z_]([a-zA-Z-_]){,}$", RegexOptions.Compiled);

        private static readonly SqlRegex _sqlRegex = new SqlRegex
        {
            SqlReadOnlyRegex = DefaultSqlReadOnlyRegex,
            SqlInjectionRegex = DefaultSqlInjectionRegex,
            ColumnNameRegex = DefaultColumnNameRegex

        };

        public Regex SqlReadOnlyRegex { get; set; }

        public Regex SqlInjectionRegex { get; set; }

        public Regex ColumnNameRegex { get; set; }

        public static SqlRegex Default => _sqlRegex;



        public bool IsRightColumnName(string name)
        {
            return DefaultColumnNameRegex.IsMatch(name);
        }

        public bool IsReadOnlyQuery(string query)
        {
            return SqlReadOnlyRegex.IsMatch(query);
        }


        ///// <summary>
        ///// A helper method to attempt to discover [known] SqlInjection attacks.  
        ///// </summary>
        ///// <param name="query">sql clause</param>
        ///// <returns>true if found, false if not found</returns>
        //public static bool DetectSqlInjection(string query)
        //{
        //    return SqlInjectionRegex.IsMatch(query);
        //}


        /// <summary>
        /// A helper method to attempt to discover [known] SqlInjection attacks.  
        /// </summary>
        /// <param name="whereClause">string of the whereClause to check</param>
        /// <returns>true if found, false if not found</returns>
        public bool DetectSqlInjection(string whereClause)
        {
            return DetectSqlInjection(whereClause, null);
        }

        /// <summary>
        /// A helper method to attempt to discover [known] SqlInjection attacks.  
        /// </summary>
        /// <param name="whereClause">string of the whereClause to check</param>
        /// <param name="orderBy">string of the orderBy clause to check</param>
        /// <returns>true if found, false if not found</returns>
        public bool DetectSqlInjection(string whereClause, string orderBy)
        {
            if (!string.IsNullOrEmpty(whereClause) && SqlInjectionRegex.IsMatch(whereClause))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(orderBy) && SqlInjectionRegex.IsMatch(orderBy))
            {
                return true;
            }
            return false;
        }
    }
}
