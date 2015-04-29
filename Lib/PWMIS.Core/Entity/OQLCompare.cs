/*
 * ========================================================================
 * Copyright(c) 2006-2010 PWMIS, All Rights Reserved.
 * Welcom use the PDF.NET (PWMIS Data Process Framework).
 * See more information,Please goto http://www.pwmis.com/sqlmap 
 * ========================================================================
 * OQL 条件比较对象，用于构建复杂的查询条件。
 * 
 * V5.0版本是基于V4.X版本提供的功能基础之上的完全重构版本，有些构造函数已经过时，详细请看源码
 * 当前版本要求必须有一个OQL对象相关联，用于处理条件对象上面的实体类属性选取问题，
 * 因此大幅优化了原有版本的代码。
 * 新增了比较条件的泛型支持，使得条件比较代码跟简洁。
 * 
 * 作者：邓太华     时间：2013-7-1-12
 * 版本：V5.0
 * 
 * 修改者：         时间：2013-10-25                
 * 修改说明：修正OQL 高级子查询中，参数过多引起的子查询参数名错误的问题，感谢网友null 发现问题。
 *               
 * 修改者：         时间：2014-3-7                
 * 修改说明：修正 在OQLCompare 方法中 跟GUID比较的问题，感谢网友 网友Super Show 发现问题。
 * 
 * 修改者：         时间：2015-3-7   
 * 处理条件累加问题某一侧对象可能为空的情况 ，感谢网友 广州-四糸奈 发现此问题
 * 
 * 修改者：         时间：2015-4-28   
 * 增加 IsEmptyCompare 比较空条件的方法
 * 增加 NewCompare 方法用于动态构造条件方法的时候，调用了实体类属性进行条件比较的情况
 * 
 * ========================================================================
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.DataMap.Entity
{

    public delegate OQLCompare OQLCompareFunc(OQLCompare cmp);
    public delegate OQLCompare OQLCompareFunc<T>(OQLCompare cmp, T p);
    public delegate OQLCompare OQLCompareFunc<T1,T2>(OQLCompare cmp,T1 p1,T2 p2);
    public delegate OQLCompare OQLCompareFunc<T1, T2,T3>(OQLCompare cmp, T1 p1, T2 p2,T3 p3);

    /// <summary>
    /// 实体对象条件比较类，用于复杂条件比较表达式
    /// </summary>
    public class OQLCompare //: IDisposable
    {
        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OQLCompare()
        {
           
        }

        public OQLCompare(OQL oql)
        {
            if (oql == null)
                throw new ArgumentException("OQLCompare 关联的OQL对象为空！");
            this.LinkedOQL = oql;
        }

        /// <summary>
        /// 使用一个实体对象初始化本类。该方法已经过时，请使用其它构造函数
        /// </summary>
        /// <param name="e"></param>
        [Obsolete("该方法已经过时，请使用 OQLCompare(OQL oql) 构造函数")]
        public OQLCompare(EntityBase e)
        {
           
        }

        /// <summary>
        /// 使用多个实体类进行连接查询的条件。该方法已经过时，请使用其它构造函数
        /// </summary>
        /// <param name="e"></param>
        /// <param name="joinedEntitys"></param>
        [Obsolete("该方法已经过时，请使用 OQLCompare(OQL oql) 构造函数")]
        public OQLCompare(EntityBase e, params EntityBase[] joinedEntitys)
        {
            
        }

        /// <summary>
        /// 采用两个实体比较对象按照某种比较逻辑进行处理，构造一个新的实体比较对象
        /// </summary>
        /// <seealso cref="http://www.cnblogs.com/bluedoctor/archive/2010/11/28/1870095.html"/>
        /// <param name="compare1">比较逻辑符号左边的实体比较对象</param>
        /// <param name="logic">比较逻辑</param>
        /// <param name="compare2">比较逻辑符号左边的实体比较对象</param>
        public OQLCompare(OQLCompare compare1, CompareLogic logic, OQLCompare compare2)
        {
            if (object.Equals(compare1, null))
            {
                throw new ArgumentNullException("参数compare1 不能都为空！");
            }
                
            if (object.Equals(compare2, null) && logic!= CompareLogic.NOT)
                throw new ArgumentNullException("参数compare2 为空的时候，只能是NOT操作！");
            this.LinkedOQL = compare1.LinkedOQL;
            this.LeftNode = compare1;
            this.RightNode = compare2;
            this.Logic = logic;
        }

        #endregion

        /// <summary>
        /// 对条件表达式取反
        /// </summary>
        /// <param name="cmp"></param>
        /// <returns></returns>
        public static OQLCompare Not(OQLCompare cmp)
        {
            return new OQLCompare(cmp, CompareLogic.NOT, null);
        }

       

        #region 其它方法
        /// <summary>
        /// 比较类别
        /// </summary>
        public enum CompareType
        {
            /// <summary>
            /// 大于
            /// </summary>
            Greater,
            /// <summary>
            /// 不大于（小于或等于）
            /// </summary>
            LessThanOrEqual,
            /// <summary>
            /// 小于
            /// </summary>
            LessThan,
            /// <summary>
            /// 不小于（大于或等于）
            /// </summary>
            GreaterThanOrEqual,
            /// <summary>
            /// 相等
            /// </summary>
            Equal,
            /// <summary>
            /// 不等于
            /// </summary>
            NotEqual,
            /// <summary>
            /// 类似于
            /// </summary>
            Like,
            /// <summary>
            /// IS NULL / IS NOT NULL
            /// </summary>
            IS,
            /// <summary>
            /// IN 查询
            /// </summary>
            IN,
            /// <summary>
            /// Not In 查询
            /// </summary>
            NotIn,
            /// <summary>
            /// IS NOT NULL
            /// </summary>
            IsNot,
            /// <summary>
            /// BETWEEN 在某两个值之间
            /// </summary>
            Between

        }

        /// <summary>
        /// 条件表达式逻辑符号
        /// </summary>
        public enum CompareLogic
        {
            /// <summary>
            /// 逻辑 与
            /// </summary>
            AND,
            /// <summary>
            /// 逻辑 或
            /// </summary>
            OR,
            /// <summary>
            /// 逻辑 非
            /// </summary>
            NOT
        }

        /// <summary>
        /// （条件表达式）比较的参数信息表
        /// </summary>
        [Obsolete("方法已经过时，不再返回有意义的值")]
        public Dictionary<string, object> ComparedParameters
        {
            get { return null; }
        }

        /// <summary>
        /// 获取一个新的参数名称。方法已经过时
        /// </summary>
        /// <returns></returns>
        [Obsolete("方法已经过时,不再返回有意义的值")]
        public string GetNewParameterName()
        {
            return "";
        }

        /// <summary>
        /// 获取比较表达式的字符串形式
        /// </summary>
        public string CompareString
        {
            get
            {
                string result = string.Empty;
                if (this.IsLeaf)
                {
                    //假设左边是字段名，右边是值或者其它字段名
                    result = string.Format("{0} {1} {2}"
                        , getCompareFieldString(this.SqlFunctionFormat, this.ComparedFieldName)
                        , this.GetComparedTypeString()
                        , this.ComparedParameterName);
                }
                else if (this.Logic == CompareLogic.NOT)
                {
                    result = string.Format(" NOT ({0}) ", this.LeftNode.CompareString);
                }
                else
                {
                    string format = string.Empty;
                    if (this.LeftNode.IsLeaf && this.RightNode.IsLeaf)
                    {
                        format = " {0} {1} {2} ";
                    }
                    else if (this.LeftNode.IsLeaf && !this.RightNode.IsLeaf)
                    {
                        if (this.RightNode.Logic == this.Logic)
                            format = " {0} {1} {2}\r\n ";
                        else
                            format = " {0} \r\n\t{1}\r\n\t  (\r\n\t  {2}\r\n\t  )\r\n ";
                    }
                    else if (!this.LeftNode.IsLeaf && this.RightNode.IsLeaf)
                    {
                        if (this.LeftNode.Logic == this.Logic)
                            format = " {0} {1} {2} ";
                        else
                            format = "\r\n\t  (\r\n\t  {0}\r\n\t  ) \r\n\t{1} \r\n\t{2} ";
                    }
                    else
                    {
                        //左右子节点，都不是叶子结点
                        bool left_flag = CheckChildLogicEquals(this.LeftNode, this.Logic);
                        bool right_flag = CheckChildLogicEquals(this.RightNode, this.Logic);

                        if (left_flag && right_flag)
                            format = " {0} {1} {2} ";
                        else if (!left_flag && right_flag)
                            format = "\r\n\t({0}) {1} {2} ";
                        else if (left_flag && !right_flag)
                            format = " {0} {1} ({2})\r\n ";
                        else
                            format = "\r\n\t({0})\r\n {1} \r\n\t({2})\r\n ";

                    }

                    string logicString = this.Logic == CompareLogic.AND ? "AND" : (
                        this.Logic == CompareLogic.OR ? "OR" : "NOT"
                        );
                    result = string.Format(format, this.LeftNode.CompareString, logicString, this.RightNode.CompareString);
                }
                //
                return result;
            }
        }

        private string getCompareFieldString(string sqlFunctionFormat, string currFieldName)
        {
            string compareFieldString = string.Empty;
            if (!string.IsNullOrEmpty(sqlFunctionFormat))
            {
                if (sqlFunctionFormat.Contains("--"))
                    throw new Exception("SQL 函数格式串中有危险的内容");
                if (!sqlFunctionFormat.Contains("{0}"))
                    throw new Exception("SQL 函数格式串未指定替换位置{0}");
                compareFieldString = string.Format(sqlFunctionFormat, currFieldName);
            }
            else
            {
                compareFieldString = currFieldName;
            }
            return compareFieldString;
        }

        /// <summary>
        /// 判断两个OQLCompare 是否是同一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        
        #endregion

        #region 原有私有方法

        private string GetDbCompareTypeStr(CompareType type)
        {
            string typeStr = "";
            switch (type)
            {
                case CompareType.Equal: typeStr = "="; break;
                case CompareType.Greater: typeStr = ">"; break;
                case CompareType.Like: typeStr = " LIKE "; break;
                case CompareType.LessThanOrEqual: typeStr = "<="; break;
                case CompareType.GreaterThanOrEqual: typeStr = ">="; break;
                case CompareType.NotEqual: typeStr = "<>"; break;
                case CompareType.LessThan: typeStr = "<"; break;
                case CompareType.IN: typeStr = " IN "; break;
                case CompareType.IS: typeStr = " IS "; break;
                case CompareType.NotIn: typeStr = " NOT IN "; break;
                case CompareType.IsNot: typeStr = " IS NOT "; break;
                case CompareType.Between: typeStr = " BETWEEN "; break;
                default: typeStr = "="; break;
            }
            return typeStr;
        }

        private static OQLCompare BuildOperator(OQLCompare compare, object Value, CompareType cmpType)
        {
            //如果Value是字段属性，将出错
            if (compare.LinkedOQL == null)
                throw new ArgumentException("OQLCompare 关联的OQL对象为空！参数无效，参数名：compare");
            var tnf = compare.LinkedOQL.TakeOneStackFields();
            compare.ComparedFieldName = tnf.SqlFieldName;
            compare.ComparedParameterName = compare.LinkedOQL.CreateParameter(tnf,Value);
            compare.ComparedType = cmpType;

            compare.LinkedOQL.fieldStack.Clear();
            return compare;
        }
        #endregion

        #region 比较的方法

        /// <summary>
        /// 对一组OQLCompare 对象，执行CompareLogic 类型的比较，通常用于构造复杂的带括号的条件查询
        /// </summary>
        /// <seealso cref="http://www.cnblogs.com/bluedoctor/archive/2011/02/24/1963606.html"/>
        /// <param name="compares">OQL比较对象列表</param>
        /// <param name="logic">各组比较条件的组合方式，And，Or，Not</param>
        /// <returns>新的条件比较对象</returns>
        public OQLCompare Comparer(List<OQLCompare> compares, CompareLogic logic)
        {
            if (compares == null || compares.Count == 0)
                throw new Exception("OQL 条件比较对象集合不能为空或者空引用！");
            if (compares.Count == 1)
                return compares[0];
            OQLCompare cmp = new OQLCompare(this.LinkedOQL);
            //string typeString = logic == CompareLogic.AND ? " And " : logic == CompareLogic.OR ? " Or " : " Not ";
            //foreach (OQLCompare item in compares)
            //{
            //    cmp.CompareString += item.CompareString + typeString;
            //    if (item.ComparedParameters != null)
            //        foreach (string key in item.ComparedParameters.Keys)
            //        {
            //            cmp.ComparedParameters.Add(key, item.ComparedParameters[key]);
            //        }

            //}
            //cmp.CompareString = cmp.CompareString.Substring(0, cmp.CompareString.Length - typeString.Length);
            //cmp.CompareString = " ( " + cmp.CompareString + " ) ";
            //return cmp;
            //
            //将列表转换成树
            foreach (OQLCompare item in compares)
            {
                if (object.Equals(cmp.LeftNode,null))
                {
                    cmp.LeftNode = item;
                    cmp.Logic = logic;
                }
                else if (object.Equals(cmp.RightNode, null))
                {
                    cmp.RightNode = item;
                }
                else
                {
                    var newCmp = new OQLCompare(this.LinkedOQL);
                    newCmp.LeftNode = cmp;
                    newCmp.Logic = logic;
                    newCmp.RightNode = item;

                    cmp = newCmp;
                }
            }
            return cmp;
        }


        /// <summary>
        /// 将当前实体属性的值和要比较的值进行比较，得到一个新的实体比较对象
        /// </summary>
        /// <param name="field">实体对象属性</param>
        /// <param name="type">比较类型枚举</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>比较表达式</returns>
        public OQLCompare Comparer<T>(T field, CompareType type, T Value)
        {
            return Comparer<T>(field, type, Value, null);
        }

        /// <summary>
        /// 清除字段堆栈,返回当前对象,如果在调用Comparer方法之前调用了关联的实体类属性进行条件判断,动态构造比较条件,此时请调用此方法
        /// </summary>
        /// <returns></returns>
        public OQLCompare NewCompare()
        {
            this.LinkedOQL.fieldStack.Clear();
            return this;
        }

        private OQLCompare ComparerInner<T>(T field, CompareType type, object oValue, string sqlFunctionFormat)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            TableNameField leftField;
            TableNameField rightField;

            compare.LinkedOQL.TakeTwoStackFields(field, oValue,out leftField, out rightField);
            if (leftField != null)
                compare.ComparedFieldName = leftField.SqlFieldName;
            else if (rightField != null)
                compare.ComparedFieldName = rightField.SqlFieldName;
            else
                throw new Exception("前或者后的比较字段对像均为空！");

            compare.ComparedType = type;
            if (type == CompareType.IS || type == CompareType.IsNot)
            {
                if (oValue != null)
                {
                    string strValue = oValue.ToString().ToUpper().Trim();
                    if (strValue == "NULL" || strValue == "NOT NULL")
                        compare.ComparedParameterName = strValue;
                    else
                        throw new ArgumentOutOfRangeException("IS 操作符的对象只能是NULL 或者 NOT NULL");
                }
                else
                {
                    compare.ComparedParameterName = "NULL";
                }
            }
            else if (type == CompareType.IN || type == CompareType.NotIn)
            {
                throw new ArgumentOutOfRangeException("IN,NOT IN 操作符请使用Comparer方法中带数组参数的重载方法");
            }
            else
            {
                if (leftField!=null && rightField!=null)
                {
                    //可能直接用相同的字段来比较，感谢网友Sharp_C 发现此问题
                    if (leftField.SqlFieldName == rightField.SqlFieldName)
                        compare.ComparedParameterName = compare.LinkedOQL.CreateParameter(leftField,oValue);
                    else
                        compare.ComparedParameterName = rightField.SqlFieldName;
                }
                else if (leftField!=null && rightField==null)
                {
                    compare.ComparedParameterName = compare.LinkedOQL.CreateParameter(leftField,oValue);
                }
                else if (leftField==null && rightField!=null)
                {
                    compare.ComparedFieldName = compare.LinkedOQL.CreateParameter(rightField,field);
                    compare.ComparedParameterName = rightField.SqlFieldName;
                }
                else
                {
                    throw new InvalidOperationException("当前OQLCompare 内部操作状态无效，条件比较未使用实体类的属性。");
                }
            }
            compare.SqlFunctionFormat = sqlFunctionFormat;
            //compare.LinkedOQL.fieldStack.Clear();//TakeTwoStackFields 方法已经清理过
            return compare;
        }

        /// <summary>
        /// 将当前实体属性的值和要比较的值进行比较，得到一个新的实体比较对象
        /// </summary>
        /// <param name="field">实体对象属性</param>
        /// <param name="type">比较类型枚举</param>
        /// <param name="Value">要比较的值</param>
        /// <param name="sqlFunctionFormat">SQL 函数格式串，例如 "DATEPART(hh, {0})"</param>
        /// <returns>比较表达式</returns>
        public OQLCompare Comparer<T>(T field, CompareType type, T Value, string sqlFunctionFormat)
        {
            return ComparerInner<T>(field, type, Value, sqlFunctionFormat);
        }

        /// <summary>
        /// 将当前实体类的属性值应用SQL函数以后，与一个值进行比较。
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   //查询15点后登录的用户
        ///   Users user = new Users();
        ///
        ///   OQL q = OQL.From(user)
        ///    .Select()
        ///    .Where(cmp => cmp.ComparerSqlFunction(user.LastLoginTime, OQLCompare.CompareType.Greater, 15, "DATEPART(hh, {0})"))
        ///    .END;
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="field">属性字段</param>
        /// <param name="type">比较类型枚举</param>
        /// <param name="Value">应用函数后要比较的值</param>
        /// <param name="sqlFunctionFormat">SQL 函数格式串，例如 "DATEPART(hh, {0})"</param>
        /// <returns>比较表达式</returns>
        public OQLCompare ComparerSqlFunction<T>(T field, CompareType type, object Value, string sqlFunctionFormat)
        {
            return ComparerInner<T>(field, type, Value, sqlFunctionFormat);
        }

        /// <summary>
        /// 将当前实体类的属性值应用SQL函数以后，与一个值进行比较。
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   //查询15点后登录的用户
        ///   Users user = new Users();
        ///
        ///   OQL q = OQL.From(user)
        ///    .Select()
        ///    .Where(cmp => cmp.ComparerSqlFunction(user.LastLoginTime, ">", 15, "DATEPART(hh, {0})"))
        ///    .END;
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">属性的类型</typeparam>
        /// <param name="field">属性字段</param>
        /// <param name="typeString">比较类型字符串</param>
        /// <param name="Value">应用函数后要比较的值</param>
        /// <param name="sqlFunctionFormat">SQL 函数格式串，例如 "DATEPART(hh, {0})"</param>
        /// <returns>比较表达式</returns>
        public OQLCompare ComparerSqlFunction<T>(T field, string typeString, object Value, string sqlFunctionFormat)
        {
            return ComparerInner<T>(field, CompareString2Type(typeString), Value, sqlFunctionFormat);
        }

        #region 聚合函数
        public OQLCompare Count(CompareType type ,object Value)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
           
            if (type == CompareType.IS || type == CompareType.IN || type == CompareType.NotIn)
                throw new ArgumentOutOfRangeException("IS,IN,NOT IN 操作符请使用Count 方法不受支持！");

            compare.ComparedFieldName = "*";
            compare.ComparedType = type;
            compare.ComparedParameterName = compare.LinkedOQL.CreateParameter(null, Value);
            compare.SqlFunctionFormat = "COUNT({0})";
            return compare;
        }

        public OQLCompare Count<T>(T field, CompareType type, object oValue)
        {
            return ComparerInner<T>(field, type, oValue, "COUNT({0})");
        }

        public OQLCompare AVG<T>(T field, CompareType type, object oValue)
        {
            return ComparerInner<T>(field, type, oValue, "AVG({0})");
        }

        public OQLCompare MAX<T>(T field, CompareType type, T Value)
        {
            return Comparer<T>(field, type, Value, "MAX({0})");
        }

        public OQLCompare MIN<T>(T field, CompareType type, T Value)
        {
            return Comparer<T>(field, type, Value, "MIN({0})");
        }

        public OQLCompare SUM<T>(T field, CompareType type, T Value)
        {
            return Comparer<T>(field, type, Value, "SUM({0})");
        }

        public OQLCompare Count<T>(T field, string typeString, object oValue)
        {
            return ComparerInner<T>(field, CompareString2Type(typeString), oValue, "COUNT({0})");
        }

        public OQLCompare AVG<T>(T field, string typeString, object oValue)
        {
            return ComparerInner<T>(field, CompareString2Type(typeString), oValue, "AVG({0})");
        }

        public OQLCompare MAX<T>(T field, string typeString, T Value)
        {
            return Comparer<T>(field, CompareString2Type(typeString), Value, "MAX({0})");
        }

        public OQLCompare MIN<T>(T field, string typeString, T Value)
        {
            return Comparer<T>(field, CompareString2Type(typeString), Value, "MIN({0})");
        }

        public OQLCompare SUM<T>(T field, string typeString, T Value)
        {
            return Comparer<T>(field, CompareString2Type(typeString), Value, "SUM({0})");
        }
        #endregion

        #region 对ＩＮ的特殊处理

        public OQLCompare Comparer<T>(T field, CompareType type, T[] Value)
        {
            return Comparer<T>(field, type, Value, null);
        }

        public OQLCompare Comparer<T>(T field, string cmpTypeString, T[] Value)
        {
            return Comparer<T>(field, CompareString2Type(cmpTypeString), Value, null);
        }

        public OQLCompare Comparer<T>(T field, CompareType type, T[] Value, string sqlFunctionFormat)
        {
            if (Value == null && (type == CompareType.IN || type == CompareType.NotIn))
                throw new ArgumentNullException("IN 条件的参数不能为空！");

            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            var tnf=compare.LinkedOQL.TakeOneStackFields();
            compare.ComparedFieldName = tnf.SqlFieldName;
            compare.ComparedType = type;
            compare.SqlFunctionFormat = sqlFunctionFormat;
            if (type == CompareType.IN || type == CompareType.NotIn)
            {
                string[] paraNames = new string[Value.Length];
                //如果 field== DBNull.value 下面的类型判断将不准。
                //解决网友 Love๑ 的问题
                Type t=field ==null? typeof(string) : field.GetType();
                //网友Super Show 发现GUID和枚举问题 2014.3.11
                if (t == typeof(string) || t == typeof(DateTime) || t == typeof(Guid) || t.IsEnum) 
                {
                    for (int i = 0; i < Value.Length; i++)
                    {
                        paraNames[i] = compare.LinkedOQL.CreateParameter(tnf,Value[i]);
                    }
                    compare.ComparedParameterName = "(" + string.Join(",", paraNames) + ")";
                }
                else if (t.IsValueType)
                {
                    for (int i = 0; i < Value.Length; i++)
                    {
                        paraNames[i] = Value[i].ToString();
                    }
                    compare.ComparedParameterName = "(" + string.Join(",", paraNames) + ")";
                }
                else
                {
                    throw new ArgumentException("IN,Not In 操作只支持字符串数组或者值类型数组参数");
                }
               
            }
            else if (type == CompareType.IS || type == CompareType.IsNot)
            {
                compare.ComparedParameterName = "NULL";
            }
            else
            {
                throw new ArgumentException("当前方法只允许使用IN 或者 NOT IN，否则请使用另外的重载方法");
            }
            return compare;
        }
       
        #endregion

        public OQLCompare Comparer(object field, string typeString, OQL Value)
        {
            return Comparer(field, CompareString2Type(typeString), Value);
        }

        public OQLCompare Comparer(object field, CompareType type, OQL Value)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            var tnf=compare.LinkedOQL.TakeOneStackFields();
            compare.ComparedFieldName = tnf.SqlFieldName;
            compare.ComparedType = type;
            if (type == CompareType.IS || type == CompareType.IsNot)
            {
                throw new ArgumentOutOfRangeException("IS 操作符的不支持子查询！");
            }
            else
            {
                string childSql = Value.ToString();
                if (Value.Parameters.Count > 0)
                {
                    //先备份SQL语句中的参数名 ，感谢网友 null(yoli799480165) 发现此bug
                    foreach (string key in Value.Parameters.Keys)
                    {
                        childSql = childSql.Replace(key, key+"_C");
                    }
                    foreach (string key in Value.Parameters.Keys)
                    {
                        var vtnf = Value.Parameters[key];
                        string paraName = this.LinkedOQL.CreateParameter(vtnf);
                        childSql = childSql.Replace(key + "_C", paraName);
                    }
                }
                compare.ComparedParameterName = "\r\n(" + childSql + ")\r\n";
            }
            compare.SqlFunctionFormat = "";
            return compare;
        }

        public OQLCompare Comparer(object field, CompareType type, OQLChildFunc Value)
        {
            OQL childOql = Value(this.LinkedOQL);
            return Comparer(field, type,childOql);
        }

        public OQLCompare Comparer(object field, string typeString, OQLChildFunc Value)
        {
            OQL childOql = Value(this.LinkedOQL);
            return Comparer(field, CompareString2Type(typeString), childOql);
        }

        /// <summary>
        /// 将当前实体属性的值和要比较的值进行比较，得到一个新的实体比较对象
        /// </summary>
        /// <param name="field">实体对象属性</param>
        /// <param name="cmpTypeString">数据库比较类型字符串</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>比较表达式</returns>
        public OQLCompare Comparer<T>(T field, string cmpTypeString, T Value)
        {
            return Comparer<T>(field, cmpTypeString, Value, null);
        }

        private CompareType CompareString2Type(string cmpTypeString)
        {
            if (String.IsNullOrEmpty(cmpTypeString))
                cmpTypeString = "=";
            else
                cmpTypeString = cmpTypeString.Trim().ToLower();

            //替换成枚举比较好
            CompareType ct;
            switch (cmpTypeString)
            {
                case "=":
                    ct = CompareType.Equal;
                    break;
                case ">":
                    ct = CompareType.Greater;
                    break;
                case "<":
                    ct = CompareType.LessThan;
                    break;
                case "<>":
                    ct = CompareType.NotEqual;
                    break;
                case ">=":
                    ct = CompareType.GreaterThanOrEqual;
                    break;
                case "<=":
                    ct = CompareType.LessThanOrEqual;
                    break;
                case "like":
                    ct = CompareType.Like;
                    break;
                case "is":
                    ct = CompareType.IS;
                    break;
                case "is not":
                    ct = CompareType.IsNot;
                    break;
                case "in":
                    ct = CompareType.IN;
                    break;
                case "not in":
                    ct = CompareType.NotIn;
                    break;
                default:
                    throw new Exception("比较符号必须是 =,>,<,>=,<=,<>,like,is,in,not in 中的一种。");
            }
            return ct;
        }

        /// <summary>
        /// 将当前实体属性的值和要比较的值进行比较，得到一个新的实体比较对象
        /// </summary>
        /// <param name="field">实体对象属性</param>
        /// <param name="cmpTypeString">数据库比较类型字符串</param>
        /// <param name="Value">要比较的值</param>
        /// <param name="sqlFunctionFormat">SQL 函数格式串，例如 "DATEPART(hh, {0})"</param>
        /// <returns>比较表达式</returns>
        public OQLCompare Comparer<T>(T field, string cmpTypeString, T Value, string sqlFunctionFormat)
        {
            return this.Comparer<T>(field, CompareString2Type(cmpTypeString), Value, sqlFunctionFormat);
        }

        /// <summary>
        /// 将当前实体属性的值和要比较的数组值进行比较，得到一个新的实体比较对象
        /// </summary>
        /// <typeparam name="T">实体类属性类型</typeparam>
        /// <param name="field">实体类属性</param>
        /// <param name="cmpTypeString">比较类型字符串</param>
        /// <param name="Value">要比较的数组值</param>
        /// <param name="sqlFunctionFormat">附加的SQL函数格式串</param>
        /// <returns></returns>
        public OQLCompare Comparer<T>(T field, string cmpTypeString, T[] Value, string sqlFunctionFormat)
        {
            return this.Comparer(field, CompareString2Type(cmpTypeString), Value, sqlFunctionFormat);
        }

        //public OQLCompare Comparer(object field, string cmpTypeString, DateTime[] Value, string sqlFunctionFormat)
        //{
        //    return this.Comparer(field, CompareString2Type(cmpTypeString), Value, sqlFunctionFormat);
        //}

        //public OQLCompare Comparer(object field, string cmpTypeString, ValueType[] Value, string sqlFunctionFormat)
        //{
        //    return this.Comparer(field, CompareString2Type(cmpTypeString), Value, sqlFunctionFormat);
        //}

        /// <summary>
        /// 将当前实体属性的值作为比较的值，得到一个新的实体比较对象
        /// </summary>
        /// <param name="field">实体对象的属性字段</param>
        /// <returns>比较表达式</returns>
        public OQLCompare EqualValue(object field)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            var tnf=compare.LinkedOQL.TakeOneStackFields();
            compare.ComparedFieldName = tnf.SqlFieldName;
            compare.ComparedParameterName = compare.LinkedOQL.CreateParameter(tnf,field);
            compare.ComparedType = CompareType.Equal;

            compare.LinkedOQL.fieldStack.Clear();
            return compare;

        }

        /// <summary>
        /// 判断指定字段条件为空 Is NULL
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public OQLCompare IsNull(object field)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            compare.ComparedFieldName = compare.LinkedOQL.TakeOneStackFields().Field;
            compare.ComparedParameterName = "NULL";
            compare.ComparedType = CompareType.IS;

            compare.LinkedOQL.fieldStack.Clear();
            return compare;
        }

        /// <summary>
        /// 判断指定字段条件为空 Is Not NULL
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public OQLCompare IsNotNull(object field)
        {
            OQLCompare compare = this.IsNull(field);
            compare.ComparedType = CompareType.IsNot;
            return compare;
        }

        /// <summary>
        /// 指定条件的包含范围
        /// </summary>
        /// <typeparam name="T">属性字段的类型</typeparam>
        /// <param name="field">属性字段</param>
        /// <param name="beginValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns>比较对象</returns>
        public OQLCompare Between<T>(T field, T beginValue, T endValue)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            var tnf=compare.LinkedOQL.TakeOneStackFields();
            compare.ComparedFieldName = tnf.SqlFieldName;
            compare.ComparedParameterName =string.Format(" {0} AND {1} "
                , compare.LinkedOQL.CreateParameter(tnf,beginValue)
                , compare.LinkedOQL.CreateParameter(tnf,endValue));
            compare.ComparedType = CompareType.Between;

            compare.LinkedOQL.fieldStack.Clear();
            return compare;
        }

        /// <summary>
        /// 根据实体对象的属性，获取新的条件比较对象，用于比较操作符重载
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public OQLCompare Property(object field)
        {
            OQLCompare compare = new OQLCompare(this.LinkedOQL);
            return compare;
        }

        #endregion

        #region 操作符重载
        /// <summary>
        /// 将两个实体比较对象进行逻辑 与 比较，得到一个新的实体比较表达式
        /// </summary>
        /// <param name="compare1">左表达式</param>
        /// <param name="compare2">右表达式</param>
        /// <returns>实体比较表达式</returns>
        public static OQLCompare operator &(OQLCompare compare1, OQLCompare compare2)
        {
            //处理条件累加问题某一侧对象可能为空的情况 2015.3.7 感谢网友 广州-四糸奈 发现此问题
            if (!IsEmptyCompare(compare1) && !IsEmptyCompare(compare2))
            {
                return new OQLCompare(compare1, CompareLogic.AND, compare2);
            }
            else
            {
                if (IsEmptyCompare(compare1))
                    return compare2;
                else
                    return compare1;
            }
        }

        private static bool IsEmptyCompare(OQLCompare cmp)
        {
            if (object.Equals(cmp, null))
                return true;
            if (cmp.IsLeaf && string.IsNullOrEmpty(cmp.ComparedFieldName))
                return true;
            return false;
        }

        /// <summary>
        /// 将两个实体比较对象进行逻辑 与 比较，得到一个新的实体比较表达式
        /// </summary>
        /// <param name="compare1">左表达式</param>
        /// <param name="compare2">右表达式</param>
        /// <returns>实体比较表达式</returns>
        public static OQLCompare operator |(OQLCompare compare1, OQLCompare compare2)
        {
            //处理条件累加问题某一侧对象可能为空的情况
            if (!object.Equals(compare1, null) && !object.Equals(compare2, null))
            {
                return new OQLCompare(compare1, CompareLogic.OR, compare2);
            }
            else
            {
                if (object.Equals(compare1, null))
                    return compare2;
                else
                    return compare1;
            }
        }

        /// <summary>
        /// 设置等于某个实体属性的比较条件
        /// </summary>
        /// <param name="compare">当前实体比较对象</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>构造的实体比较对象</returns>
        public static OQLCompare operator ==(OQLCompare compare, object Value)
        {
            return BuildOperator(compare, Value, CompareType.Equal);
        }

        /// <summary>
        /// 设置不等于某个实体属性的比较条件
        /// </summary>
        /// <param name="compare">当前实体比较对象</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>构造的实体比较对象</returns>
        public static OQLCompare operator !=(OQLCompare compare, object Value)
        {
            return BuildOperator(compare, Value, CompareType.NotEqual);
        }

        /// <summary>
        /// 设置不小于某个实体属性的比较条件
        /// </summary>
        /// <param name="compare">当前实体比较对象</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>构造的实体比较对象</returns>
        public static OQLCompare operator >=(OQLCompare compare, object Value)
        {
            return BuildOperator(compare, Value, CompareType.GreaterThanOrEqual);
        }

        /// <summary>
        /// 设置不大于某个实体属性的比较条件
        /// </summary>
        /// <param name="compare">当前实体比较对象</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>构造的实体比较对象</returns>
        public static OQLCompare operator <=(OQLCompare compare, object Value)
        {
            return BuildOperator(compare, Value,CompareType.LessThanOrEqual);
        }

        /// <summary>
        /// 设置大于某个实体属性的比较条件
        /// </summary>
        /// <param name="compare">当前实体比较对象</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>构造的实体比较对象</returns>
        public static OQLCompare operator >(OQLCompare compare, object Value)
        {
            return BuildOperator(compare, Value,CompareType.Greater);
        }

        /// <summary>
        /// 设置小于某个实体属性的比较条件
        /// </summary>
        /// <param name="compare">当前实体比较对象</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>构造的实体比较对象</returns>
        public static OQLCompare operator <(OQLCompare compare, object Value)
        {
            return BuildOperator(compare, Value,CompareType.LessThan);
        }

        #endregion


        //新的变量

        /// <summary>
        /// 关联的OQL对象
        /// </summary>
        public OQL LinkedOQL { get;protected internal set; }

        //

        protected OQLCompare LeftNode { get; set; }

        protected OQLCompare RightNode { get; set; }

        protected CompareLogic Logic { get; set; }

        protected bool IsLeaf
        {
            get
            {
                return object.Equals(LeftNode, null) && object.Equals(RightNode, null);
            }
        }

        protected string ComparedFieldName;
        protected string ComparedParameterName;
        protected CompareType ComparedType;
        protected string SqlFunctionFormat;
        /// <summary>
        /// 获取比较类型的字符串形式
        /// </summary>
        /// <returns></returns>
        protected string GetComparedTypeString()
        {
            return this.GetDbCompareTypeStr(ComparedType);
        }

        /// <summary>
        /// 检查子节点的逻辑类型
        /// </summary>
        /// <param name="childCmp"></param>
        /// <param name="currLogic"></param>
        /// <returns></returns>
        private bool CheckChildLogicEquals(OQLCompare childCmp, CompareLogic currLogic)
        {
            //currCmp 不能是叶子结点
            //如果子节点的逻辑类型不同于当前逻辑类型，直接返回 非
            if (childCmp.Logic != currLogic)
                return false;

            //如果子节点的逻辑类型 同于当前逻辑类型，则需要检查子节点的左右子节点与当前逻辑类型的对比
            if (childCmp.LeftNode.IsLeaf && childCmp.RightNode.IsLeaf)
            {
                return childCmp.Logic == currLogic;
            }
            else
            {
                if (!childCmp.LeftNode.IsLeaf && !childCmp.RightNode.IsLeaf)
                {
                    bool left_flag = false;
                    bool right_flag = false;
                    left_flag = CheckChildLogicEquals(childCmp.LeftNode, currLogic);
                    right_flag = CheckChildLogicEquals(childCmp.RightNode, currLogic);
                    return left_flag && right_flag;
                }
                else if (!childCmp.LeftNode.IsLeaf && childCmp.RightNode.IsLeaf)
                {
                    return CheckChildLogicEquals(childCmp.LeftNode, currLogic);
                }
                else if (childCmp.LeftNode.IsLeaf && !childCmp.RightNode.IsLeaf)
                {
                    return CheckChildLogicEquals(childCmp.RightNode, currLogic);
                }
                else
                {
                    return false;
                }
            }
        }
    }

   
}
