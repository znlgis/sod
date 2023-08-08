using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.Core;

namespace PWMIS.Common
{
    /// <summary>
    ///     SQL语句解析和排序构造器，为Access,SqlCe和早期的SqlServer等分页提供帮助
    /// </summary>
    public class SqlOrderBuilder
    {
        //示例："select ID,t.[User Name] As UserName from tab t order   by    t.[User Name]  , id desc option(hash group,fast 10) "
        private readonly string sourceSql;

        /// <summary>
        ///     以一个原始的SQL语句构造本对象实例
        /// </summary>
        /// <param name="sourceSql">源SQL</param>
        public SqlOrderBuilder(string sourceSql)
        {
            this.sourceSql = sourceSql;
        }

        /// <summary>
        ///     排序字段信息
        /// </summary>
        public List<OrderField> OrderFields { private set; get; }

        /// <summary>
        ///     执行构造后获得的字段选择信息
        /// </summary>
        public List<SqlField> SelectFields { private set; get; }

        /// <summary>
        ///     解析SQL语句中的Select 字段信息，并返回 From 谓词在SQL语句中的位置索引
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="fromIndex">From 谓词在SQL语句中的位置索引</param>
        /// <returns>Select 字段信息</returns>
        public List<SqlField> ParseSelect(string sql, out int fromIndex)
        {
            var ps = TextSearchUtil.SearchWordsIndex(sql, "select");
            if (ps.A == -1)
                throw new ArgumentException("未找到期望的谓词select ,不是合法的SQL:\r\n" + sql);
            var pd = TextSearchUtil.SearchWordsIndex(sql, "DISTINCT");
            if (pd.A != -1)
                ps.B = pd.B;

            var pf = TextSearchUtil.SearchWordsIndex(sql, "from");
            if (pf.A < ps.A)
                throw new ArgumentException("在select谓词之后未找到期望的from谓词，不支持此种类型的SQL分页：\r\n" + sql);
            fromIndex = pf.A;

            var selectFieldsBlock = sql.Substring(ps.B, pf.A - ps.B).Trim();
            var sqlFields = new List<SqlField>();
            if (selectFieldsBlock == "*")
            {
                sqlFields.Add(new SqlField { Field = "*" });
                return sqlFields;
            }

            var selectFieldsArr = selectFieldsBlock.Split(new[] { ",", Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            foreach (var fieldItem in selectFieldsArr)
            {
                var targetIndex = 0;
                var field = TextSearchUtil.FindNearWords(fieldItem, 0, 100, out targetIndex);
                if (targetIndex >= 0)
                {
                    //select 字段可能是函数调用，感谢网友 芜湖－大枕头 发现此Bug 2017.4.14
                    var fieldUp = field.ToUpper();
                    if (fieldUp == "SUM" || fieldUp == "AVG" || fieldUp == "MIN" || fieldUp == "MAX" ||
                        fieldUp == "COUNT" || fieldUp.IndexOf('(') > 0)
                    {
                        var atas = fieldItem.ToUpper().IndexOf(" AS ");
                        var fieldAsName = fieldItem.Substring(atas + 3);
                        sqlFields.Add(new SqlField
                        {
                            Field = fieldItem.Substring(0, atas),
                            Alias = fieldAsName
                        });
                    }
                    else
                    {
                        //寻找临近的单词，可能没有，可能是AS，也可能直接就是字段别名
                        var fieldAsName = TextSearchUtil.FindNearWords(fieldItem, targetIndex + field.Length, 50,
                            out targetIndex);
                        if (fieldAsName.ToLower() == "as")
                            fieldAsName = TextSearchUtil.FindNearWords(fieldItem, targetIndex + 2, 50, out targetIndex);
                        sqlFields.Add(new SqlField { Field = field, Alias = fieldAsName });
                    }
                }
            }

            return sqlFields;
        }

        /// <summary>
        ///     解析排序部分
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="orderBlockPoint">Order语句块位置信息</param>
        /// <returns></returns>
        public List<OrderField> ParseOrder(string sql, out Point orderBlockPoint)
        {
            var p = TextSearchUtil.SearchWordsLastIndex(sql, "order by");
            if (p.A == -1)
            {
                //没有 order by子句，无法分页，但是可能是取前面N条记录
                orderBlockPoint = p;
                return null;
            }

            //ROW_NUMBER() OVER(PARTITION BY PostalCode ORDER BY SalesYTD DESC) AS "Row Number", //这种应该不予处理
            //所以 如果还有PARTITION BY  子句，则认为是复杂的SQL，抛出异常语句
            var OrderFields = new List<OrderField>();
            var orderSep = 0; //排序信息分隔符（逗号）的位置
            var startIndex = p.B;
            do
            {
                int fieldIndex;
                var field = TextSearchUtil.FindNearWords(sql, startIndex, 255, '[', ']', out fieldIndex);
                startIndex = fieldIndex + field.Length;
                int orderTypeIndex;
                var orderType = TextSearchUtil.FindNearWords(sql, startIndex, 10, out orderTypeIndex);

                var of = new OrderField();
                of.Field = field;
                //of.Alias 需要判断是否是别名
                if (orderTypeIndex == -1)
                    of.IsAsc = true;
                else
                    of.IsAsc = orderType.ToUpper() == "ASC";
                OrderFields.Add(of);

                //寻找可能有多个排序的分隔逗号
                var dIndex = orderTypeIndex == -1 ? startIndex : orderTypeIndex + orderType.Length;
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
        ///     构造可分页的排序SQL语句，如果topCount小于0，源SQL必须带排序语句
        /// </summary>
        /// <param name="topCount">如果指定大于0的值，将生成Top子句，如果小于0，则是限制逆向排序的记录数</param>
        /// <returns></returns>
        public string Build(int topCount)
        {
            int fromIndex;
            Point orderBlockPoint;
            var isSelectStart = false;
            var sqlFields = ParseSelect(sourceSql, out fromIndex);
            OrderFields = ParseOrder(sourceSql, out orderBlockPoint);
            if (sqlFields[0].Field == "*")
                isSelectStart = true;
            else if (OrderFields != null)
                //检查参与排序的字段是否在SELECT中，如果不在，添加进去
                foreach (var orderItem in OrderFields)
                {
                    var target = sqlFields.Find(p =>
                        string.Equals(p.Field, orderItem.Field, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(p.Alias, orderItem.Field, StringComparison.OrdinalIgnoreCase));
                    if (target == null)
                    {
                        orderItem.CreateAlias("");
                        var tempField = new SqlField { Field = orderItem.Field, Alias = orderItem.Alias };
                        sqlFields.Add(tempField);
                        orderItem.InSelect = false;
                    }
                    else
                    {
                        orderItem.Field = target.Field;
                        orderItem.Alias = target.Alias;

                        orderItem.InSelect = true;
                    }
                }

            SelectFields = sqlFields;
            //重新构造Select语句块
            var sb = new StringBuilder();
            //DISTINCT 查询问题，感谢网友 @深圳-光头佬 发现此问题 2017.6.21
            var psd = TextSearchUtil.SearchWordsIndex(sourceSql, "select DISTINCT");
            if (psd.A != -1)
                sb.Append("SELECT DISTINCT ");
            else
                sb.Append("SELECT ");

            if (topCount > 0)
                sb.Append("Top " + topCount + "\r\n");
            else if (topCount < 0)
                sb.Append("Top " + -topCount + "\r\n");
            else
                sb.Append(" \r\n");
            if (isSelectStart)
            {
                sb.Append(" * \r\n");
            }
            else
            {
                var count = SelectFields.Count;
                var index = 0;
                foreach (var item in SelectFields)
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
            }

            if (topCount < 0)
            {
                //逆排序，用于获取最后几页的数据，包括最后一页
                if (orderBlockPoint.A == -1)
                    throw new Exception("当参数 topCount小于0（逆排序），必须指定排序信息");
                //改变Order by 子句，生成逆向排序查询
                var beforOrder = sourceSql.Substring(fromIndex, orderBlockPoint.A - fromIndex);
                var afterOrder = sourceSql.Substring(orderBlockPoint.B);
                var InverseOrderString = "\r\n ORDER BY " + GetLastInverseOrderExpString() + "\r\n";
                sb.Append(beforOrder);
                sb.Append(InverseOrderString);
                sb.Append(afterOrder);
            }
            else
            {
                sb.Append(sourceSql.Substring(fromIndex));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     构造可分页排序的SQL，并可以附带结果过滤条件
        /// </summary>
        /// <param name="topCount">要限制的记录条数，如果小于0，则是限制逆向排序的记录数</param>
        /// <param name="filterConditions">附带结果过滤条件，注意字段不能带表名字或者表的别名</param>
        /// <returns></returns>
        public string Build(int topCount, string filterConditions)
        {
            if (topCount == 0)
                throw new ArgumentOutOfRangeException("参数 topCount 不能为0，必须是大于或者小于0的一个整数");
            int fromIndex;
            Point orderBlockPoint;
            var templateSql = @"
SELECT TOP {0} * FROM (
          {1}
        ) T_SOB
WHERE {2}
ORDER BY {3}
        ";
            var sqlFields = ParseSelect(sourceSql, out fromIndex);
            OrderFields = ParseOrder(sourceSql, out orderBlockPoint);
            if (sqlFields[0].Field == "*")
            {
                //待处理
                SelectFields = sqlFields;
                var tempSql = sourceSql.Substring(0, orderBlockPoint.A);
                tempSql = tempSql + sourceSql.Substring(orderBlockPoint.B);
                ;
                //return string.Format(templateSql, topCount, tempSql, filterConditions);
                throw new ArgumentException("暂不支持 * 查询的带额外过滤条件的查询。");
            }

            //检查参与排序的字段是否在SELECT中，如果不在，添加进去
            foreach (var orderItem in OrderFields)
            {
                var target = sqlFields.Find(p =>
                    string.Equals(p.Field, orderItem.Field, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(p.Alias, orderItem.Field, StringComparison.OrdinalIgnoreCase));
                if (target == null)
                {
                    orderItem.CreateAlias("");
                    var tempField = new SqlField { Field = orderItem.Field, Alias = orderItem.Alias };
                    sqlFields.Add(tempField);
                    orderItem.InSelect = false;
                }
                else
                {
                    orderItem.Field = target.Field;
                    orderItem.Alias = target.Alias;

                    orderItem.InSelect = true;
                }
            }

            SelectFields = sqlFields;
            //重新构造Select语句块
            var sb = new StringBuilder();
            sb.Append("SELECT \r\n");
            var count = SelectFields.Count;
            var index = 0;
            foreach (var item in SelectFields)
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
            var beforOrder = sourceSql.Substring(fromIndex, orderBlockPoint.A - fromIndex);
            var afterOrder = sourceSql.Substring(orderBlockPoint.B);
            sb.Append(beforOrder);
            sb.Append(afterOrder);
            var innerSql = sb.ToString();

            if (topCount > 0)
                return string.Format(templateSql, topCount, innerSql, filterConditions, GetOrderExpString());
            return string.Format(templateSql, -topCount, innerSql, filterConditions, GetInverseOrderExpString());
        }

        /// <summary>
        ///     构造统计记录数的SQL查询
        /// </summary>
        /// <param name="filterConditions"></param>
        /// <returns></returns>
        public string BuildCountRecordSql(string filterConditions)
        {
            Point orderBlockPoint;
            OrderFields = ParseOrder(sourceSql, out orderBlockPoint);
            //剔除Order by 子句，用于构建外层过滤查询
            var beforOrder = sourceSql.Substring(0, orderBlockPoint.A);
            var afterOrder = sourceSql.Substring(orderBlockPoint.B);
            var countSql = "Select COUNT(*) FROM ( " + beforOrder + "\r\n" + afterOrder + " ) P_Count ";
            if (!string.IsNullOrEmpty(filterConditions))
                countSql += "\r\n WHERE " + filterConditions;
            return countSql;
        }

        /// <summary>
        ///     获取排序表达式字符串
        /// </summary>
        /// <returns></returns>
        public string GetOrderExpString()
        {
            if (OrderFields == null || OrderFields.Count == 0)
                throw new Exception("排序字段为空，可能源SQL语句没有指定，或者没有调用过Build方法");
            var orderExpArr = OrderFields.ConvertAll(p => p.ToOutString()).ToArray();
            var orderString = string.Join(",", orderExpArr);
            return orderString;
        }

        /// <summary>
        ///     获取逆排序表达式字符串
        /// </summary>
        /// <returns></returns>
        public string GetInverseOrderExpString()
        {
            if (OrderFields == null || OrderFields.Count == 0)
                throw new Exception("排序字段为空，可能源SQL语句没有指定，或者没有调用过Build方法");
            var orderExpArr = OrderFields.ConvertAll(p => p.ToOutInverseString()).ToArray();
            var orderString = string.Join(",", orderExpArr);
            return orderString;
        }

        /// <summary>
        ///     获取分页去最后一页逆排序表达式字符串
        /// </summary>
        /// <returns></returns>
        public string GetLastInverseOrderExpString()
        {
            if (OrderFields == null || OrderFields.Count == 0)
                throw new Exception("排序字段为空，可能源SQL语句没有指定，或者没有调用过Build方法");
            var orderExpArr = OrderFields.ConvertAll(p => p.ToInverseString()).ToArray();
            var orderString = string.Join(",", orderExpArr);
            return orderString;
        }
    }

    /// <summary>
    ///     SQL字段信息
    /// </summary>
    public class SqlField
    {
        /// <summary>
        ///     字段别名
        /// </summary>
        public string Alias;

        /// <summary>
        ///     字段名
        /// </summary>
        public string Field;

        /// <summary>
        ///     如果没有别名，根据字段名，构造别名
        ///     <param name="otherText">构造时候要附加的文本</param>
        /// </summary>
        public void CreateAlias(string otherText)
        {
            if (!string.IsNullOrEmpty(Alias))
                return;
            if (string.IsNullOrEmpty(Field))
                return;
            var arr = Field.Split('.');
            var result = string.Empty;
            if (arr.Length > 1)
            {
                if (!string.IsNullOrEmpty(otherText))
                    result = arr[0].TrimEnd(']') + "_" + otherText + arr[1].TrimStart('[');
                else
                    result = arr[0].TrimEnd(']') + "_" + arr[1].TrimStart('[');
            }
            else
            {
                result = arr[0].TrimStart('[').TrimEnd(']');
                if (!string.IsNullOrEmpty(otherText))
                    result = result + "_" + otherText;
                else
                    result = "A" + DateTime.Now.Day + "_" + result;
            }

            if (result[0] != '[')
                result = result.Insert(0, "[");
            if (result[result.Length - 1] != ']')
                result = result + "]";
            Alias = result;
        }
    }

    /// <summary>
    ///     排序字段信息
    /// </summary>
    public class OrderField : SqlField
    {
        /// <summary>
        ///     是否存在于Select子句中
        /// </summary>
        public bool InSelect;

        /// <summary>
        ///     是否升序
        /// </summary>
        public bool IsAsc;

        /// <summary>
        ///     获取排序表达式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} {1}", string.IsNullOrEmpty(Alias) ? Field : Alias
                , IsAsc ? "ASC" : "DESC");
        }

        public string ToOutString()
        {
            return string.Format("{0} {1}", string.IsNullOrEmpty(Alias) ? getShortFieldName(Field) : Alias
                , IsAsc ? "ASC" : "DESC");
        }

        /// <summary>
        ///     获取反向的排序表达式
        /// </summary>
        /// <returns></returns>
        public string ToInverseString()
        {
            return string.Format("{0} {1}", string.IsNullOrEmpty(Alias) ? Field : Alias
                , IsAsc ? "DESC" : "ASC");
        }

        public string ToOutInverseString()
        {
            return string.Format("{0} {1}", string.IsNullOrEmpty(Alias) ? getShortFieldName(Field) : Alias
                , IsAsc ? "DESC" : "ASC");
        }

        private string getShortFieldName(string field)
        {
            var arr = field.Split('.');
            if (arr.Length == 2)
                return arr[1];
            return arr[0];
        }
    }
}