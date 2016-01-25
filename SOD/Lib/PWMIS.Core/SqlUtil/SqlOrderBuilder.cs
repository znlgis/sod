using PWMIS.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Common
{
    /// <summary>
    /// SQL语句解析和排序构造器，为Access,SqlCe和早期的SqlServer等分页提供帮助
    /// </summary>
    public  class SqlOrderBuilder
    {
        //示例："select ID,t.[User Name] As UserName from tab t order   by    t.[User Name]  , id desc option(hash group,fast 10) "
        private string sourceSql;
        /// <summary>
        /// 排序字段信息
        /// </summary>
        public List<OrderField> OrderFields { private set; get; }
        /// <summary>
        /// 执行构造后获得的字段选择信息
        /// </summary>
        public List<SqlField> SelectFields { private set; get; }
        /// <summary>
        /// 以一个原始的SQL语句构造本对象实例
        /// </summary>
        /// <param name="sourceSql">源SQL</param>
        public SqlOrderBuilder(string sourceSql)
        {
            this.sourceSql = sourceSql;
        }

        /// <summary>
        /// 解析SQL语句中的Select 字段信息，并返回 From 谓词在SQL语句中的位置索引
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="fromIndex">From 谓词在SQL语句中的位置索引</param>
        /// <returns>Select 字段信息</returns>
        public List<SqlField> ParseSelect(string sql,out int fromIndex)
        {
            Point ps = TextSearchUtil.SearchWordsIndex(sql, "select");
            if (ps.A == -1)
                throw new ArgumentException("未找到期望的谓词select ,不是合法的SQL:\r\n"+sql);
            Point pf = TextSearchUtil.SearchWordsIndex(sql, "from");
            if (pf.A < ps.A)
                throw new ArgumentException("在select谓词之后未找到期望的from谓词，不支持此种类型的SQL分页：\r\n"+sql);
            fromIndex = pf.A;

            string selectFieldsBlock = sql.Substring(ps.B, pf.A - ps.B).Trim ();
            List<SqlField> sqlFields = new List<SqlField>();
            if (selectFieldsBlock == "*")
            {
                sqlFields.Add(new SqlField() { Field = "*" });
                return sqlFields;
            }
            else
            {
                string[] selectFieldsArr = selectFieldsBlock.Split(new string[] { ",", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string fieldItem in selectFieldsArr)
                {
                    int targetIndex = 0;
                    string field = TextSearchUtil.FindNearWords(fieldItem, 0, 100,out targetIndex);
                    //寻找临近的单词，可能没有，可能是AS，也可能直接就是字段别名
                    string fieldAsName = TextSearchUtil.FindNearWords(fieldItem, targetIndex + field.Length, 50, out targetIndex);
                    if (fieldAsName.ToLower() == "as")
                    {
                        fieldAsName = TextSearchUtil.FindNearWords(fieldItem, targetIndex + 2, 50, out targetIndex);
                    }
                    sqlFields.Add(new SqlField() { Field = field, Alias =fieldAsName  });
                }
            }
            return sqlFields;
        }
        /// <summary>
        /// 解析排序部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public  List<OrderField> ParseOrder(string sql,out Point orderBlockPoint)
        {
            Point p = TextSearchUtil.SearchWordsLastIndex(sql, "order by");
            if (p.A == -1)
                throw new ArgumentException("没有 order by子句，无法分页:\r\n"+sql);
            //ROW_NUMBER() OVER(PARTITION BY PostalCode ORDER BY SalesYTD DESC) AS "Row Number", //这种应该不予处理
            //所以 如果还有PARTITION BY  子句，则认为是复杂的SQL，抛出异常语句
            List<OrderField> OrderFields = new List<OrderField>();
            int orderSep=0;//排序信息分隔符（逗号）的位置
            int startIndex = p.B;
            do
            {
                int fieldIndex;
                string field = TextSearchUtil.FindNearWords(sql, startIndex, 255, '[', ']', out fieldIndex);
                startIndex = fieldIndex + field.Length;
                int orderTypeIndex;
                string orderType = TextSearchUtil.FindNearWords(sql, startIndex, 10, out orderTypeIndex);

                OrderField of = new OrderField();
                of.Field = field;
                //of.Alias 需要判断是否是别名
                if (orderTypeIndex == -1)
                {
                    of.IsAsc = true;
                }
                else
                {
                    of.IsAsc = orderType.ToUpper() == "ASC" ;
                }
                OrderFields.Add(of);

                //寻找可能有多个排序的分隔逗号
                int dIndex = orderTypeIndex == -1 ? startIndex : orderTypeIndex + orderType.Length;
                orderSep = TextSearchUtil.FindPunctuationBeforWord(sql, dIndex, ',');
                
                if (orderSep != -1)
                    startIndex = orderSep + 1;
                else
                    startIndex = dIndex;
            } while (orderSep != -1);

            orderBlockPoint = new Point(p.A, startIndex);
            return OrderFields;
            /*
           
            if (orderSep != -1)
            {
                //寻找第二组排序字段信息
                string word2 = TextSearchUtil.FindNearWords(sql, orderSep + 1, 10, out wordIndex);
                if (wordIndex != -1)
                {
                    string orderType2 = TextSearchUtil.FindNearWords(sql, wordIndex + word2.Length, 10, out orderTypeIndex);
                }
            }
            //搜索排序字段在SELECT子句中的别名
            int selectFieldIndex = sql.IndexOf(word, 0, StringComparison.OrdinalIgnoreCase);
            if (selectFieldIndex > 0)
            {
                //寻找临近的单词，可能是AS，也可能直接就是字段别名
                string fieldAsName = TextSearchUtil.FindNearWords(sql, selectFieldIndex + word.Length, 50, out wordIndex);
                if (fieldAsName.ToLower() == "as")
                {
                    fieldAsName = TextSearchUtil.FindNearWords(sql, wordIndex + 2, 50, out wordIndex);
                }
            }
             */ 
        }

        /// <summary>
        /// 构造可分页的排序SQL语句
        /// </summary>
        /// <param name="topCount">如果指定大于0的值，将生成Top子句</param>
        /// <returns></returns>
        public string Build(int topCount)
        {
            int fromIndex;
            Point orderBlockPoint;
            List<SqlField> sqlFields = ParseSelect(this.sourceSql,out fromIndex );
            this.OrderFields = ParseOrder(this.sourceSql, out orderBlockPoint);
            if (sqlFields[0].Field == "*")
            {
                this.SelectFields = sqlFields;
                return this.sourceSql;
            }
            else
            {
                //检查参与排序的字段是否在SELECT中，如果不在，添加进去
                foreach (OrderField orderItem in this.OrderFields)
                {
                    var target = sqlFields.Find (p => string.Equals(p.Field, orderItem.Field, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(p.Alias , orderItem.Field, StringComparison.OrdinalIgnoreCase));
                    if (target == null)
                    {
                        sqlFields.Add(new SqlField() { Field = orderItem.Field });
                        orderItem.InSelect = false;
                    }
                    else
                    {
                        orderItem.Field = target.Field;
                        orderItem.Alias = target.Alias;
                        orderItem.InSelect = true;
                    }
                }
                this.SelectFields = sqlFields;
            }
            //重新构造Select语句块
            System.Text.StringBuilder sb = new StringBuilder();
            if (topCount > 0)
                sb.Append("SELECT Top "+ topCount +"\r\n");
            else
                sb.Append("SELECT \r\n");
            int count = this.SelectFields.Count;
            int index = 0;
            foreach (SqlField  item in this.SelectFields)
            {
                index++;
                sb.Append('\t');
                sb.Append(item.Field);
                if (!string.IsNullOrEmpty(item.Alias))
                {
                    sb.Append(" AS ");
                    sb.Append(item.Alias);
                }
                if(index !=count )
                    sb.Append(',');
                sb.Append("\r\n");
            }

            sb.Append(this.sourceSql.Substring(fromIndex));
            return sb.ToString();
        }

        /// <summary>
        /// 构造可分页排序的SQL，并可以附带结果过滤条件
        /// </summary>
        /// <param name="topCount">要限制的记录条数</param>
        /// <param name="filterConditions">附带结果过滤条件，注意字段不能带表名字或者表的别名</param>
        /// <returns></returns>
        public string Build(int topCount,string filterConditions )
        {
            int fromIndex;
            Point orderBlockPoint;
            string templateSql = @"
SELECT TOP {0} * FROM (
          {1}
        ) T_SOB
WHERE {2}
ORDER BY {3}
        ";
            List<SqlField> sqlFields = ParseSelect(this.sourceSql, out fromIndex);
            this.OrderFields = ParseOrder(this.sourceSql, out orderBlockPoint);
            if (sqlFields[0].Field == "*")
            {
                //待处理
                this.SelectFields = sqlFields;
                string tempSql = this.sourceSql.Substring(0, orderBlockPoint.A);
                tempSql = tempSql + this.sourceSql.Substring(orderBlockPoint.B); ;
                return string.Format(templateSql, topCount, tempSql, filterConditions);
            }
            else
            {
                //检查参与排序的字段是否在SELECT中，如果不在，添加进去
                foreach (OrderField orderItem in this.OrderFields)
                {
                    var target = sqlFields.Find(p => string.Equals(p.Field, orderItem.Field, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(p.Alias, orderItem.Field, StringComparison.OrdinalIgnoreCase));
                    if (target == null)
                    {
                        sqlFields.Add(new SqlField() { Field = orderItem.Field });
                        orderItem.InSelect = false;
                    }
                    else
                    {
                        orderItem.Field = target.Field;
                        orderItem.Alias = target.Alias;
                        orderItem.InSelect = true;
                    }
                }
                this.SelectFields = sqlFields;
            }
            //重新构造Select语句块
            System.Text.StringBuilder sb = new StringBuilder();
            //if (topCount > 0)
            //    sb.Append("SELECT Top " + topCount + "\r\n");
            //else
                sb.Append("SELECT \r\n");
            int count = this.SelectFields.Count;
            int index = 0;
            foreach (SqlField item in this.SelectFields)
            {
                index++;
                sb.Append('\t');
                sb.Append(item.Field);
                if (!string.IsNullOrEmpty(item.Alias))
                {
                    sb.Append(" AS ");
                    sb.Append(item.Alias);
                }
                if (index != count)
                    sb.Append(',');
                sb.Append("\r\n");
            }
            //剔除Order by 子句，用于构建外层过滤查询
            string beforOrder = this.sourceSql.Substring(fromIndex, orderBlockPoint.A - fromIndex);
            string afterOrder = this.sourceSql.Substring(orderBlockPoint.B);
            sb.Append(beforOrder);
            sb.Append(afterOrder);
            string innerSql= sb.ToString();

            string[] orderExpArr = this.OrderFields.ConvertAll<string>(p => p.ToString()).ToArray();
            string orderString = string.Join(",", orderExpArr);
            return string.Format(templateSql, topCount, innerSql, filterConditions, orderString);
        }
    }
    /// <summary>
    /// SQL字段信息
    /// </summary>
    public class SqlField
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Field;
        /// <summary>
        /// 字段别名
        /// </summary>
        public string Alias;
    }

    /// <summary>
    /// 排序字段信息
    /// </summary>
    public class OrderField : SqlField
    {
        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAsc;
        /// <summary>
        /// 是否存在于Select子句中
        /// </summary>
        public bool InSelect;

        /// <summary>
        /// 获取排序表达式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", string.IsNullOrEmpty(this.Alias) ? this.Field : this.Alias
                , this.IsAsc ?"ASC":"DESC");
        }
    }


}
