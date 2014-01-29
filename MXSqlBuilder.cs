using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AMDapper.DAL
{
    /// <summary>
    /// A simple SQL builder, easily extendable. I built it for using with dapper as dapper's 
    /// sqlbuilder looks to complex to use. The primary purpose is to build dynamic where condition to be used in search filters.
    /// My Website: http://sites.google.com/site/amitstefen1956
    /// </summary>
    public class MXSqlBuilder
    {
        StringBuilder sb = new StringBuilder();

        int countWhereClause = 0;

        #region "Public Methods"
        /// <summary>
        /// use specific column names in csv format or default is * for all
        /// </summary>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public MXSqlBuilder Select(string columnNames = "*")
        {
            sb.AppendLine("SELECT " + columnNames);
            return this;
        }

        public MXSqlBuilder From(string tableName)
        {
            sb.AppendLine("FROM " + tableName);
            return this;
        }

        /// <summary>
        /// Adds a where condition. The subsequent calls will result in AND operator.
        /// </summary>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public MXSqlBuilder AddWhere(string whereCondition)
        {
            countWhereClause++;
            sb.Append(string.Format("WHERE {0} ", whereCondition));            
            return this;
        }

        /// <summary>
        /// Puts up OR opeartor in the where condition.
        /// </summary>
        /// <param name="orCondition"></param>
        /// <returns></returns>
        public MXSqlBuilder AddOr(string orCondition)
        {
            countWhereClause++;
            sb.Append(string.Format("OR {0} ", orCondition));
            return this;
        }

        public MXSqlBuilder InnerJoin(string joinTableWithOnCondition)
        {
            sb.AppendLine(string.Format("INNER JOIN {0} ", joinTableWithOnCondition));
            return this;
        }

        public MXSqlBuilder LeftJoin(string joinTableWithOnCondition)
        {
            sb.AppendLine(string.Format("LEFT JOIN {0} ", joinTableWithOnCondition));
            return this;
        }

        public MXSqlBuilder RightJoin(string joinTableWithOnCondition)
        {
            sb.AppendLine(string.Format("RIGHT JOIN {0} ", joinTableWithOnCondition));
            return this;
        }

        /// <summary>
        /// Should be called in the end
        /// </summary>
        public string RawSql
        {
            get
            {
                return RefactorSql();
            }
        }

        #endregion

        #region "Private helpers"

        private string RefactorSql()
        {
            if (countWhereClause > 1)
            {
                string sql = this.sb.ToString().Replace("WHERE", "AND");

                var regex = new Regex("AND");
                sql = regex.Replace(sql, "WHERE", 1);

                return sql;
            }
            else
            {
                return this.sb.ToString();
            }
        }

        #endregion
    }//End of class
}
/*
 Sample usage- 
 * Let's say I've two tables; Productions and ProductionTypes. And I want to get productions with their Types for some conditions.
                var sql = new MXSqlBuilder();
                sql.Select("Productions.*, ProductionTypes.*")
                sql.From("Productions");                
                sql.LeftJoin("ProductionTypes ON Productions.ProdTypeId = ProductionTypes.ProdTypeId");                
                sql.AddWhere("Productions.ProdId > @ProdId");
                sql.AddWhere("Productions.Year > @ProdYear");
                sql.AddOr("Productions.ProdTypeId = @ProdTypeId");
 *
 * then call- sql.RawSql and use it with dapper's Query method.
 
 */
