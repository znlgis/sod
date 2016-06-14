/*
 *
 * 有关OQL的概念，参考下面的文章内容：
 * 《ORM查询语言（OQL）简介--概念篇》 http://www.cnblogs.com/bluedoctor/archive/2012/10/06/2712699.html
 * 
 * 版本说明：OQL ver 5.2
 * 当前版本是在 4.X的版本提供的功能基础上面的重构，新增了实体类内连接支持，Insert，InserForm支持，
 * 并完全重写了内部实现，优化了代码体系，使得扩展性更好。
 * 
 * 注意：当前版本废除了OQL1的功能定义，与原有版本的OQL1已经完全不同，但仍可以使用OQL.Condition 对象，
 * 使用方法不变。另外，OQL的几个构造函数已经过时，具体看源码说明。
 * 
 */
#region 旧版本文件头注释信息
/*
 * 
 * 作者：邓太华     时间：2008-10-12
 * 版本：V4.5
 * 
 * 修改者：         时间：2011-4-17                
 * 修改说明：为所有字段增加括号，以兼容其它数据库
 * 
 *  * 修改者：         时间：2011-12-14                
 * 修改说明：解决3个以上的实体类连接查询生成的SQL不正确的问题
 * 
 *  * 修改者：         时间：2012-2-28                
 * 修改说明：实现OQLCompare对象比较的时候操作符重载，修复在多实体连接下面的SQL函数问题。
 * 
 *  * 修改者：         时间：2012-10-11                
 * 修改说明：修复在多实体连接查询的时候，OrderBy 字段名丢失的问题。
 *           修改了 CompareType 的枚举名称定义，使之可读性更好
 *           
 *  * 修改者：         时间：2012-10-25                
 * 修改说明：OQLCompare 对象执行比较的时候，支持SQL函数格式串。
 * 
 *  * 修改者：         时间：2012-12-26                
 * 修改说明：OQL 相关对象实现IDisposable接口，避免OQL对象使用了具有长生命周期的实体类导致可能的内存泄漏问题。
 * 
 *  修改者：         时间：2013-1-16                
 * 修改说明：(网友[有事M我]提供 )根据传入的多字段排序，从而进行动态的排序；
 * 适用于不能在OQL表达式里面直接指明排序方式的场景（比如需要从前台传入）。
 * 
 * 增加了OQLOrder对象，根据外部传入条件，动态构造排序条件供OQL使用。
 * 
 * 修改者：         时间：2013-1-17                
 * 修改说明：建立PWMIS.Core.Extensions 项目，扩展OQL的使用方式；
 * 
 * 修改者：         时间：2013-5-27                
 * 修改说明：修复OQL2.In 的子查询的时候，主查询采用OQLCompare 条件引起的Bug；
 *           感谢 博客园@张善友 发现此Bug
 *           
 * 修改者：         时间：2013-9-17                
 * 修改说明：修复在使用ＩＮ的子查询中使用排序，导致查询语句不正确的问题；
 *           感谢 网友　GIV-顺德 发现此Bug
 *           
 * 修改者：         时间：2013-10-15                
 * 修改说明：修改OQL的事件挂钩处理方式，在EntityQuery 执行后确保解除事件挂钩。
 * 
 * 修改者：         时间：2014-1-6                
 * 修改说明：修复 网友※DS发现问题， 调用本方法的时候，发现调用的第一个实体类属性是bool类型，
       引起少了一个字段查询的问题
 *     
 * 修改者：泸州-雷皇         时间：2014.10.29                
 * 修改说明：在查询中使用多个函数操作的问题

 * 修改者：         时间：2015.1.6                
 * 修改说明：修改 TableNameField 的Name 属性问题，这可能在SqlMap类型的实体类上遇到问题。
 * 
  * 修改者：         时间：2015.1.15                
 *  修改说明：OQL.From<T>() 方法修改修改成返回GOQL的方式，避免之前可能的误用。
 * 
 *  修改者：         时间：2015.4.29                
 *  修改说明：修改了条件比较过程中，OQL字段堆栈 的处理方式，解决了“调试陷阱”的问题。
 *  
 *  修改者：         时间：2015.4.29                
 *  修改说明：修改了条件比较过程中，OQLCompare对象Comparer 方法上处理可空类型可能发生的问题。
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using PWMIS.DataMap.Entity;

namespace PWMIS.DataMap.Entity
{

    public delegate OQL OQLChildFunc(OQL parent);
    /// <summary>
    /// 获取OQL缓存的委托函数
    /// </summary>
    /// <param name="paraValueObject">构造OQL对象所需要的参数值的对象，比如一个实体类</param>
    /// <returns>OQL</returns>
    public delegate OQL OQLCacheFunc(object paraValueObject);
    /// <summary>
    /// OQL排序的委托方法，用于指定排序的对象
    /// </summary>
    /// <param name="order">OQL排序对象</param>
    public delegate void OQLOrderAction(OQLOrder order);

    public delegate void OQLOrderAction<T>(OQLOrder order, T para) where T : class;
    /// <summary>
    /// 表名称字段类型。OQL内部使用
    /// </summary>
    public class TableNameField
    {
        /// <summary>
        /// 获取表名称
        /// </summary>
        public string Name
        {
            get
            {
                if (Entity == null)
                    return null;
                return Entity.GetTableName();
            }
        }
        /// <summary>
        /// 原始字段名
        /// </summary>
        public string Field;
        /// <summary>
        /// 关联的实体类
        /// </summary>
        public EntityBase Entity;
        /// <summary>
        /// 在一系列字段使用中的索引号或者当前字段在实体类字段名字数组中的索引
        /// </summary>
        public int Index;
        /// <summary>
        /// 字段对应的值
        /// </summary>
        public object FieldValue;

        private string _sqlFieldName;
        /// <summary>
        /// 在ＳＱＬ语句中使用的字段名
        /// </summary>
        public string SqlFieldName
        {
            get {
                if (string.IsNullOrEmpty(_sqlFieldName))
                    return this.Field;
                else
                    return _sqlFieldName;
            }
            set
            {
                _sqlFieldName = value;
            }
        }
    }

    /// <summary>
    /// PDF.NET ORM Query Language,实体对象查询表达式
    /// </summary>
    public class OQL : IOQL, IDisposable
    {
        /// <summary>
        /// SqlServer上的锁类型枚举。请注意，OQL应用这个枚举之后将只能在SqlServer上面使用。
        /// </summary>
        public enum SqlServerLock
        {
            /// <summary>
            /// 在该表上保持共享锁，直到整个事务结束，而不是在语句执行完立即释放所添加的锁。
            /// </summary>
            HOLDLOCK,
            /// <summary>
            /// 不添加共享锁和排它锁，当这个选项生效后，可能读到未提交读的数据或“脏数据”，这个选项仅仅应用于SELECT语句
            /// </summary>
            NOLOCK,
            /// <summary>
            /// 指定添加页锁（否则通常可能添加表锁）
            /// </summary>
            PAGLOCK,
            /// <summary>
            /// 用与运行在提交读隔离级别的事务相同的锁语义执行扫描。默认情况下，SQL Server 2000 在此隔离级别上操作
            /// </summary>
            READCOMMITTED,
            /// <summary>
            /// 跳过已经加锁的数据行，这个选项将使事务读取数据时跳过那些已经被其他事务锁定的数据行，而不是阻塞直到其他事务释放锁，READPAST仅仅应用于READ COMMITTED隔离性级别下事务操作中的SELECT语句操作
            /// </summary>
            READPAST,
            /// <summary>
            /// 等同于NOLOCK
            /// </summary>
            READUNCOMMITTED,
            /// <summary>
            /// 设置事务为可重复读隔离性级别
            /// </summary>
            REPEATABLEREAD,
            /// <summary>
            /// 使用行级锁，而不使用粒度更粗的页级锁和表级锁
            /// </summary>
            ROWLOCK,
            /// <summary>
            /// 用与运行在可串行读隔离级别的事务相同的锁语义执行扫描。等同于 HOLDLOCK。
            /// </summary>
            SERIALIZABLE,
            /// <summary>
            /// 指定使用表级锁，而不是使用行级或页面级的锁，SQL Server在该语句执行完后释放这个锁，而如果同时指定了HOLDLOCK，该锁一直保持到这个事务结束
            /// </summary>
            TABLOCK,
            /// <summary>
            /// 指定在表上使用排它锁，这个锁可以阻止其他事务读或更新这个表的数据，直到这个语句或整个事务结束
            /// </summary>
            TABLOCKX,
            /// <summary>
            /// 指定在读表中数据时设置更新 锁（update lock）而不是设置共享锁，该锁一直保持到这个语句或整个事务结束，使用UPDLOCK的作用是允许用户先读取数据（而且不阻塞其他用户读数据），并且保证在后来再更新数据时，这一段时间内这些数据没有被其他用户修改
            /// </summary>
            UPDLOCK,
            /// <summary>
            /// 未知（OQL默认）
            /// </summary>
            UNKNOW
        }

        #region 分页相关
        /// <summary>
        /// 查询前N条记录，目前仅支持Access/SqlServer，其它数据库可以使用Limit(N) 方法替代。只有设置成不小于0有效
        /// </summary>
        public int TopCount = -1;
        /// <summary>
        /// 是否开启分页功能，如果启用，OQL不能设定“排序”信息，分页标识字段将作为排序字段
        /// </summary>
        public bool PageEnable = false;
        /// <summary>
        /// 分页时候每页的记录大小，默认为10
        /// </summary>
        public int PageSize = 10;
        /// <summary>
        /// 分页时候的当前页码，默认为1
        /// </summary>
        public int PageNumber = 1;
        /// <summary>
        /// 分页时候的记录标识字段，默认为主键字段。不支持多主键。
        /// </summary>
        public string PageField = "";
        /// <summary>
        /// 分页的时候记录按照倒序排序（对Oracle数据库不起效）
        /// </summary>
        public bool PageOrderDesc = true;
        /// <summary>
        /// 分页的时候，记录的总数量，如未设置虚拟为999条。如需准确分页，应设置该值。
        /// </summary>
        public int PageWithAllRecordCount = 999;

        /// <summary>
        /// 是否排除重复记录
        /// </summary>
        public bool Distinct;

        /// <summary>
        /// 限制查询的记录数量，对于SQLSERVER/ACCESS，将采用主键作为标识的高速分页方式。
        /// 注：调用该方法不会影响生OQL.ToString()结果，仅在最终执行查询的时候才会去构造当前特点数据库的SQL语句。
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public OQL Limit(int pageSize)
        {
            this.PageEnable = true;
            this.PageSize = pageSize;
            return this;
        }

        /// <summary>
        /// 限制查询的记录数量，对于SQLSERVER/ACCESS，将采用主键作为标识的高速分页方式。
        /// 注：调用该方法不会影响生OQL.ToString()结果，仅在最终执行查询的时候才会去构造当前特点数据库的SQL语句。
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNumber">页号码</param>
        /// <returns></returns>
        public OQL Limit(int pageSize, int pageNumber)
        {
            this.PageEnable = true;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            return this;
        }

        /// <summary>
        /// 限制查询的记录数量，对于SQLSERVER/ACCESS，将采用主键作为标识的高速分页方式。
        /// 注：调用该方法不会影响生OQL.ToString()结果，仅在最终执行查询的时候才会去构造当前特点数据库的SQL语句。
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNumber">页号码</param>
        /// <param name="autoRecCount">是否允许自动查询本次分页查询前的记录总数，
        /// 如果允许，那么查询成功后可以从OQL对象的PageWithAllRecordCount 字段得到实际的记录数量</param>
        /// <returns></returns>
        public OQL Limit(int pageSize, int pageNumber,bool autoRecCount)
        {
            this.PageEnable = true;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            if (autoRecCount) this.PageWithAllRecordCount = 0;
            return this;
        }

        /// <summary>
        /// 限制查询的记录数量，对于SQLSERVER/ACCESS，将采用指定字段作为标识的高速分页方式。
        /// 注：调用该方法不会影响生OQL.ToString()结果，仅在最终执行查询的时候才会去构造当前特点数据库的SQL语句。
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNumber">页号码</param>
        /// <param name="pageField">要排序的字段</param>
        /// <returns></returns>
        public OQL Limit(int pageSize, int pageNumber, string pageField)
        {
            this.PageEnable = true;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.PageField = pageField;
            return this;
        }
        #endregion

        #region 旧版本保留的变量定义
        /// <summary>
        /// 当前实体类
        /// </summary>
        protected internal EntityBase currEntity;
        /// <summary>
        /// 是否已经发生了连接操作
        /// </summary>
        protected internal bool haveJoinOpt;
        /// <summary>
        /// 是否有排序操作
        /// </summary>
        protected internal bool haveOrderBy;
        /// <summary>
        /// SQL选择的字段
        /// </summary>
        protected internal string sql_fields = string.Empty;
        /// <summary>
        /// 查询对应的表
        /// </summary>
        protected internal string sql_table = string.Empty;
        /// <summary>
        /// 查询条件
        /// </summary>
        protected internal string sql_condition = string.Empty;
        #endregion

        #region 新增变量定义
        const int OQL_SELECT = 1, OQL_UPDATE = 2, OQL_INSERT = 3, OQL_DELETE = 4, OQL_INSERT_FROM = 5, OQL_UPDATE_SELFT = 6;
        private Dictionary<object, string> dictAliases = null;
        private string mainTableName = "";
        private List<string> selectedFieldNames = new List<string>();
        private List<string> _groupbyFieldNames;
        private string sql_from = string.Empty;//Select时候的表名或者Upate，Insert的前缀语句
        private char updateSelfOptChar;
        private int paraIndex = 0;
        private int optFlag = OQL_SELECT;
        private OQL insertFromOql = null;
        private OQL parentOql = null;
        private int fieldGetingIndex = 0;//字段获取顺序的索引，如果有子查询，那么子查询使用父查询的该索引进行递增
        private SqlServerLock serverLock = SqlServerLock.UNKNOW;
        private bool disposed;//是否已经调用了Dispose方法

        protected internal int GetFieldGettingIndex()
        {
            if (parentOql != null)
                return parentOql.GetFieldGettingIndex();
            return ++fieldGetingIndex;
        }
        /// <summary>
        /// 是否具有子查询
        /// </summary>
        protected internal bool haveChildOql = false;

        /// <summary>
        /// Where之后的OQL字符串
        /// </summary>
        protected internal string oqlString = "";
        /// <summary>
        /// 字段堆栈
        /// </summary>
        protected internal Stack<TableNameField> fieldStack = new Stack<TableNameField>();

        //private Dictionary<string, TableNameField> dictParaNameField = new Dictionary<string, TableNameField>();
        /// <summary>
        /// SQL 函数
        /// </summary>
        protected internal string sqlFunctionString = string.Empty;
        /// <summary>
        /// 分组字段名
        /// </summary>
        protected internal List<string> GroupbyFieldNames
        {
            get
            {
                if (_groupbyFieldNames == null)
                    _groupbyFieldNames = new List<string>();
                return _groupbyFieldNames;
            }
        }

        protected internal List<TableNameField> selectedFieldInfo = new List<TableNameField>();

        #endregion

        #region 旧方法

        private Dictionary<string, TableNameField> _parameters;
        /// <summary>
        /// 获取条件参数
        /// </summary>
        public Dictionary<string, TableNameField> Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new Dictionary<string, TableNameField>();
                return _parameters;
            }
        }

        /// <summary>
        /// 实体类映射的类型
        /// </summary>
        public PWMIS.Common.EntityMapType EntityMap
        {
            get;
            protected set;
        }

        /// <summary>
        /// 根据用户自定义的查询（临时视图），从该查询进一步获取指定的记录的查询语句
        /// </summary>
        /// <param name="tempViewSql">作为子表的用户查询（临时视图）</param>
        /// <returns>符合当前限定条件的查询语句</returns>
        public string GetMapSQL(string tempViewSql)
        {
            if (string.IsNullOrEmpty(tempViewSql))
                throw new Exception("用户的子查询不能为空。");
            this.mainTableName = " (" + tempViewSql + " ) tempView ";
            return ToSelectString("");
        }

        /// <summary>
        /// 要初始化的的参数值，用于自定义查询的实体类
        /// </summary>
        public Dictionary<string, object> InitParameters { get; set; }

        #endregion

        #region 新增非公开方法
        /// <summary>
        /// 获取OQL使用的字段名
        /// </summary>
        /// <param name="tnf"></param>
        /// <returns></returns>
        protected internal string GetOqlFieldName(TableNameField tnf)
        {
            if (dictAliases == null)
                return string.Format(" [{0}]", tnf.Field);
            else
            {
                string aliases = "";
                if (dictAliases.TryGetValue(tnf.Entity, out aliases))
                {
                    return string.Format(" {0}.[{1}]", aliases, tnf.Field);//关联查询，此处可能需要考虑字段AS别名 问题
                }
                else
                {
                    return string.Format(" M.[{0}]", tnf.Field);
                }
            }

        }
        /// <summary>
        /// 获取表的别名
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected internal string GetTableAliases(EntityBase entity)
        {
            string aliases = "";
            if (dictAliases.TryGetValue(entity, out aliases))
                return aliases;
            else
                return "";
        }
        /// <summary>
        /// 从堆栈上取一个以逗号间隔字段名数组字符串
        /// </summary>
        /// <returns></returns>
        protected internal string TakeStackFields()
        {
            string[] fieldNames = new string[fieldStack.Count];
            for (int i = fieldStack.Count - 1; i >= 0; i--)
            {
                TableNameField tnf = fieldStack.Pop();
                fieldNames[i] = GetOqlFieldName(tnf);
            }
            return string.Join(",", fieldNames);
        }

        private TableNameField TackOneParentStackField()
        {
            if (parentOql == null)
                throw new InvalidOperationException("OQL的父对象为空！");
            var tnf=parentOql.TakeOneStackFields();
            string parentField = tnf.SqlFieldName;
            if (parentField.IndexOf('.') == -1)
                tnf.SqlFieldName = "M." + parentField;

            return tnf;
        }
        /// <summary>
        /// 从堆栈上只取一个字段名
        /// </summary>
        /// <returns></returns>
        protected internal TableNameField TakeOneStackFields()
        {
            if (fieldStack.Count == 0)
            {
                //如果父OQL不为空，则从父对象获取字段堆栈
                if (parentOql != null)
                    return TackOneParentStackField();
                else
                    throw new ArgumentException("OQL 字段堆栈为空！可能为方法参数未曾调用过OQL关联的实体类的属性。");

            }
            TableNameField tnf = fieldStack.Pop();
            tnf.SqlFieldName= GetOqlFieldName(tnf);
            return tnf;
        }

        /*
        /// <summary>
        /// 从堆栈上取一个字段
        /// </summary>
        /// <param name="index">使用的索引号</param>
        /// <returns></returns>
        protected internal string TakeOneStackFields(out int index)
        {
            if (fieldStack.Count == 0)
            {
                //如果父OQL不为空，则从父对象获取字段堆栈
                if (parentOql != null)
                    return TackOneParentStackField(out index);
                else
                    throw new ArgumentException("OQL 字段堆栈为空！可能为方法参数未曾调用过OQL关联的实体类的属性。");

            }
            TableNameField tnf = fieldStack.Pop();
            index = tnf.Index;
            return GetOqlFieldName(tnf);
        }

        /// <summary>
        /// 尝试获取另一个堆栈中的字段，仅当堆栈中剩下一个的时候有效
        /// </summary>
        /// <returns></returns>
        protected internal string TryTakeOneStackFields()
        {
            if (fieldStack.Count == 1)
            {
                TableNameField tnf = fieldStack.Pop();
                return GetOqlFieldName(tnf);
            }
            else
            {
                if (parentOql != null)
                {
                    int index = 0;
                    return TackOneParentStackField(out index);
                }

            }
            return "";
        }
         */

        /// <summary>
        /// 从堆栈上获取2个字段信息，可能只获取到一个字段信息并自动判断字段是左还是右
        /// </summary>
        /// <typeparam name="T">属性字段的类型</typeparam>
        /// <param name="leftParaValue">左边参数的值</param>
        /// <param name="rightParaValue">右边待比较参数的值</param>
        /// <param name="leftField">输出的左字段</param>
        /// <param name="rightField">输出的右字段</param>
        protected internal void TakeTwoStackFields<T>(T leftParaValue, object rightParaValue, out TableNameField leftField, out TableNameField rightField)
        {
            leftField = null;
            rightField = null;

            int count = fieldStack.Count;
            if (count == 0)
            {
                //在子查询中条件比较左右字段都用父查询的字段，是不合理的
                throw new ArgumentException("OQL 字段堆栈为空！可能原因为方法使用的实体类不是OQL使用的，或者未使用任何实体类属性，或者使用了父查询的OQL的实体类属性。");
            }
            else if (count == 1)
            {
                TableNameField tnf = fieldStack.Pop();
                //string fieldName = GetOqlFieldName(tnf);
                tnf.SqlFieldName = GetOqlFieldName(tnf);
                //如果当前是子查询，还需要检查父查询的字段堆栈
                if (parentOql != null)
                {
                    var tnfParent = TackOneParentStackField();
                    //int parentFieldIndex = tnfParent.Index;
                    //string parentField = tnfParent.SqlFieldName;
                    if (tnf.Index < tnfParent.Index)
                    {
                        leftField = tnf;
                        rightField = tnfParent;
                    }
                    else
                    {
                        rightField = tnf;
                        leftField = tnfParent;
                    }
                }
                else
                {
                    //通过获取的字段名关联的值，来确定参数所在的顺序
                    /*
                     * //下面注释掉的代码 与后面的5 行代码等效 dth,2015.4.29
                    object Value = tnf.Entity.PropertyList(tnf.Field);
                    if (Value == DBNull.Value) Value = null;
                    if (Value != null)
                    {
                        //实体属性字段已经赋值过或者为string 类型
                        if (object.Equals(leftParaValue, Value))
                            leftField = tnf;
                        else
                            rightField = tnf;

                    }
                    else
                    {
                        //日期类型必须特殊处理，感谢网友 Sharp_C发现此问题
                        if (typeof(T) == typeof(DateTime) && object.Equals(leftParaValue, new DateTime(1900, 1, 1)))
                            leftField = tnf;
                        else if (object.Equals(default(T), leftParaValue))
                            leftField = tnf;
                        else
                            rightField = tnf;
                    }
                     */ 
                    //
                    T Value = PWMIS.Core.CommonUtil.ChangeType<T>(tnf.Entity.PropertyList(tnf.Field));
                    if (object.Equals(leftParaValue, Value))
                        leftField = tnf;
                    else if (object.Equals(rightParaValue, Value)) //增加对右边参数值的判断 2015.5.11
                        rightField = tnf;
                    else
                        leftField = tnf;
                }
            }
            else if (count >= 2)
            {
                //必定是连接查询，左右参数都是字段，而不是值
                TableNameField tnf1 = fieldStack.Pop();
                string fieldName1 = GetOqlFieldName(tnf1);
                tnf1.SqlFieldName = fieldName1;

               
                //正常情况应该是 tnf1.Index > tnf2.Index
                //tnf1的值如果不是OQLCompare.Comparer方法比较类别右边参数的值（即第三个参数），那么tnf1必定是左边参数的值，参照下面这这样的情况：
                /* //cmp 为 OQLCompare 对象
                 * OQLCompare cmpResult=null; 
                 * if(user.City=="bei jing")
                 *   cmpResult =cmp.Comparer(user.Address,"like","chao yang %");
                 * 
                 * //上面2行代码并不安全，假如 user.Address=="chao yang %" ，框架无法确定user.Address 是方法上面的第一个调用参数，因为之前有
                 * //user.City=="bei jing" 调用，对实体类属性获取了值，这样在OQL字段堆栈上，将有 2个元素，而不是1个。正常情况下，字段堆栈首先弹出
                 * // 的字段 ，也就是属性 user.Address,它会跟 Comparer 方法的第三个参数的值进行比较，如果不一致，则可以确定 user.Address 
                 * // 是在第一个参数上调用的，否则就可能是在第三个参数上调用的，然后把 user.City 从OQL字段堆栈上弹出来，作为 Comparer 方法的第一个参数，
                 * // 从而构造这样错误的条件：
                 *      "[City] like  [Address]"
                 * //当然，如果能够确信 Comparer 方法的第一个参数与第三个参数值不会相等，那么上面的代码可以放心的使用。如果不能确信，那么最好将
                 * //代码改写成：
                 * if(user.City=="bei jing")
                 *   cmpResult =cmp.NewCompare().Comparer(user.Address,"like","chao yang %");
                 *   
                 * //或者改写成下面这样：
                 * if(user.City=="bei jing")
                 *   cmpResult =cmp.Comparer(user.Address,"like","chao yang %");
                 * else
                 *   cmp.NewCompare();
                 *   
                 * //其它情况，如果在构造比较条件之前，要对“同一个实体类属性”进行检查之类，那么是不会有上面问题的担心的，比如下面的代码：
                 * string userName=user.Name;
                 * if(string.IsNullOrEmpty(userName))
                 *    cmpResult =cmp.Comparer(user.Name,"=",userName);
                 *    //如果是相等比较，推荐用这种方式实现上面的比较： cmpResult =cmp.EqualValue(userName);
                 */

                T Value = PWMIS.Core.CommonUtil.ChangeType<T>( tnf1.Entity.PropertyList(tnf1.Field));
               
                if (object.Equals(Value,leftParaValue) && !object.Equals(Value, rightParaValue))
                {
                    //修改 2015.7.5
                    //如果 T 是 int 等基础类型，下面代码无法创建泛型类型，注释 
                    //Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();
                    //if (genericTypeDefinition == typeof(Nullable<>))
                    //{
                    //    throw new ArgumentException("当前情况下 Comparer 方法调用的参数有可空类型于非可空类型的属性进行比较，无法推断参数调用顺序！");
                    //}
                    leftField = tnf1;
                }
                else
                {
                    /* //示例：假设cmp对象是一个 OQLCompare对象，user是一个实体类，看下面的代码：
                     * if(user.Age > 20)
                     *    cmpResult =cmp.Comparer(user.Name,"<>","");
                     *    
                     * //如果用户名恰好等于空字符串，那么这个比较条件可能生成下面的查询：
                     *   " Name <> Age "
                     * //显然这是不正确的，应该避免这种第一个参数和第三个参数相等，可行的办法就是给 user.Name 赋予一个不同的值，或者调用 cmp.NewCompare() 方法
                     * //参见上面的说明
                     */
                    TableNameField tnf2 = fieldStack.Pop();
                    string fieldName2 = GetOqlFieldName(tnf2);
                    tnf2.SqlFieldName = fieldName2;

                    leftField = tnf2;
                    rightField = tnf1;
                }
                fieldStack.Clear();
            }
            else
            {
                throw new InvalidOperationException("当前OQL对象的字段堆栈出现了未期望的字段数量：" + count);

            }

        }
        /// <summary>
        /// 使用当前参数值，创建一个参数名，并将参数的值放到当前对象的参数字典中去
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        protected internal string CreateParameter(TableNameField Value)
        {
            string paraName = "@P" + paraIndex++;
            Parameters.Add(paraName, Value);
            return paraName;
        }

        protected internal string CreateParameter(TableNameField field,object Value)
        {
            TableNameField tnf = new TableNameField(); 
            if (field != null)
            {
                tnf.Entity = field.Entity;
                tnf.Field = field.Field;
                tnf.Index = field.Index;
                tnf.SqlFieldName = field.SqlFieldName;
            }
            tnf.FieldValue = Value;
            return CreateParameter(tnf);
        }
        /// <summary>
        /// 获取当前OQL对象正在使用的实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected internal T GetUsedEntity<T>() where T : class
        {
            if (this.currEntity is T)
                return this.currEntity as T;
            if (dictAliases != null)
            {
                foreach (object key in dictAliases.Keys)
                {
                    if (key is T)
                        return key as T;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取当前OQL使用的所有实体类
        /// </summary>
        /// <returns></returns>
        protected internal EntityBase[] GetAllUsedEntity()
        {
            List<EntityBase> list = new List<EntityBase>();
            list.Add(this.currEntity);
            if (dictAliases != null)
            {
                foreach (object key in dictAliases.Keys)
                {
                    list.Add(key as EntityBase);
                }
            }
            return list.ToArray();
        }

        void e_PropertyGetting(object sender, PropertyGettingEventArgs e)
        {
            TableNameField tnf = new TableNameField()
            {
                Field = e.PropertyName,
                Entity = (EntityBase)sender,
                Index = this.GetFieldGettingIndex()
            };
            //tnf.FieldValue = tnf.Entity.PropertyList(tnf.Field);
            fieldStack.Push(tnf);
        }

        private JoinEntity Join(EntityBase entity, string joinTypeString)
        {
            if (dictAliases == null)
                dictAliases = new Dictionary<object, string>();
            dictAliases.Add(entity, "T" + dictAliases.Count);
            haveJoinOpt = true;
            entity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(e_PropertyGetting);
            JoinEntity je = new JoinEntity(this, entity, joinTypeString);

            return je;
        }

        #endregion

        #region OQL CRUD 方法
        /// <summary>
        /// 选取要调用的实体类属性字段。该方法可以在OQL实例对象上多次调用
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public OQL1 Select(params object[] fields)
        {
            if (disposed)
                throw new Exception("当前OQL对象已经执行过数据查询，不能再次调用本方法。在执行查询前是可以再次调用本方法的。");
            //防止在调用本方法之前访问关联的实体类属性带来的问题。
            //感谢网友 GIV-顺德 发现此问题
            if (fields.Length > 0)
            {
                int count = fieldStack.Count;
                if (count > fields.Length) //防止才执行本方法前访问了实体类的属性
                    count = fields.Length;
                for (int i = 0; i < count; i++)
                {
                    TableNameField tnf = fieldStack.Pop();
                    //排除重复的选择字段信息
                    string fullFieldName = string.Format("\r\n    {0}", GetOqlFieldName(tnf));
                    if (!selectedFieldNames.Contains(fullFieldName))
                    {
                        selectedFieldNames.Add(fullFieldName);
                        selectedFieldInfo.Add(tnf); 
                    }
                }
            }
            fieldStack.Clear();
            selectedFieldNames.Reverse();//恢复正常的字段选取顺序
            selectedFieldInfo.Reverse(); 
            return new OQL1(this);
        }

        /// <summary>
        /// 使用是否排除重复记录的方式，来选取实体对象的属性
        /// <remarks>2014.1.6  网友※DS 调用本方法的时候，发现调用的第一个实体类属性是bool类型，
        /// 引起少了一个字段查询的问题
        /// 2016.1.21 去除原来Select的重载，修改为SelectDistinct ，以解决只有一个参数且属性值为bool类型引起的问题
        /// </remarks>
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public OQL1 SelectDistinct( params object[] fields)
        {
            this.Distinct = true ;
            return Select(fields);
        }


        private string PreUpdate()
        {
            string sqlUpdate = "UPDATE " + mainTableName + " SET ";
            string[] updateFieldsString = new string[selectedFieldNames.Count];
            //先将Where条件的参数保存起来
            Dictionary<string, TableNameField> paraTemp = new Dictionary<string, TableNameField>();
            foreach (string key in this.Parameters.Keys)
                paraTemp.Add(key, this.Parameters[key]);
            this.Parameters.Clear();
            //
            for (int i = 0; i < selectedFieldNames.Count; i++)
            {
                int a = selectedFieldNames[i].IndexOf('[');
                int b = selectedFieldNames[i].IndexOf(']');
                string realField = selectedFieldNames[i].Substring(a + 1, b - a - 1);
                updateFieldsString[i] = selectedFieldNames[i];
                object Value = currEntity.PropertyList(realField);

                TableNameField tnf = new TableNameField();
                tnf.Entity = this.currEntity;
                tnf.Field = realField;
                tnf.FieldValue = Value;
                tnf.Index = i;
                string paraName = CreateParameter(tnf);//参数应该在Ｗｈｅｒｅ的参数前面
                updateFieldsString[i] += " = " + paraName;
            }
            sqlUpdate += string.Join(",", updateFieldsString);
            //恢复条件参数
            foreach (string key in paraTemp.Keys)
                this.Parameters.Add(key, paraTemp[key]);

            return sqlUpdate;
        }

        private string PreUpdateSelf()
        {
            string sqlUpdate = "UPDATE " + mainTableName + " SET ";
            string[] updateFieldsString = new string[selectedFieldNames.Count];
            for (int i = 0; i < selectedFieldNames.Count; i++)
            {
                int a = selectedFieldNames[i].IndexOf('[');
                int b = selectedFieldNames[i].IndexOf(']');
                string realField = selectedFieldNames[i].Substring(a + 1, b - a - 1);
                object Value = currEntity.PropertyList(realField);

                TableNameField tnf = new TableNameField();
                tnf.Entity = this.currEntity;
                tnf.Field = realField;
                tnf.FieldValue = Value;
                tnf.Index = i;
                string paraName = CreateParameter(tnf);
                updateFieldsString[i] = string.Format(" {0} = {1} {2} {3} "
                    , selectedFieldNames[i], selectedFieldNames[i], this.updateSelfOptChar, paraName);
            }
            sqlUpdate += string.Join(",", updateFieldsString);
            return sqlUpdate;
        }

        /// <summary>
        /// 更新实体类的某些属性值，如果未指定条件，则使用主键值为条件。
        /// </summary>
        /// <param name="fields">实体熟悉列表</param>
        /// <returns>条件表达式</returns>
        public OQL1 Update(params object[] fields)
        {
            if (fields.Length == 0)
                throw new ArgumentException("OQL Update 操作必须指定要操作的实体类的属性！");

            optFlag = OQL_UPDATE;
            OQL1 q1 = Select(fields);
            this.sql_from = PreUpdate();
            return q1;
        }

        /// <summary>
        /// 执行自操作的字段更新，比如为某一个数值性字段执行累加
        /// </summary>
        /// <param name="selfOptChar">自操作类型，有+，-，*，/ 四种类型</param>
        /// <param name="fields">字段列表</param>
        /// <returns></returns>
        public OQL1 UpdateSelf(char selfOptChar, params object[] fields)
        {
            if (selfOptChar == '+' || selfOptChar == '-' || selfOptChar == '*' || selfOptChar == '/')
            {
                optFlag = OQL_UPDATE_SELFT;
                updateSelfOptChar = selfOptChar;

                var q1 = Select(fields);
                this.sql_from = PreUpdateSelf();
                return q1;
            }
            throw new Exception("OQL的字段自操作只能是+，-，*，/ 四种类型");
        }

        public OQL Insert(params object[] fields)
        {
            if (fields.Length == 0)
                throw new ArgumentException("OQL Insert 操作必须指定要操作的实体类的属性！");
            optFlag = OQL_INSERT;
            Select(fields);
            return this;
        }

        public OQL InsertFrom(OQL childOql, params object[] targetTableFields)
        {
            if (targetTableFields.Length == 0)
                throw new ArgumentException("OQL Insert 操作必须指定要操作的实体类的属性！");
            optFlag = OQL_INSERT_FROM;
            Select(targetTableFields);
            insertFromOql = childOql;
            return this;
        }

        /// <summary>
        /// 删除实体类，如果未指定条件，则使用主键值为条件。
        /// </summary>
        /// <returns>条件表达式</returns>
        public OQL1 Delete()
        {
            optFlag = OQL_DELETE;
            return new OQL1(this);
        }

        #endregion

        #region 构造或者获取实例相关

        public OQL End
        {
            get
            {
                return this;
            }
        }

        public OQL(EntityBase e)
        {
            currEntity = e;
            mainTableName = e.GetSchemeTableName();
            sql_table = mainTableName;
            EntityMap = e.EntityMap;
            e.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(e_PropertyGetting);
        }


        public OQL(EntityBase e, params EntityBase[] others)
            : this(e)
        {
            if (dictAliases == null)
                dictAliases = new Dictionary<object, string>();
            foreach (EntityBase entity in others)
            {
                string aliases = "T" + dictAliases.Count;
                dictAliases.Add(entity, aliases);
                entity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(e_PropertyGetting);
                oqlString += string.Format(",{0} {1}", entity.GetSchemeTableName(), aliases);
            }
        }

        protected internal void AddOtherEntitys(params EntityBase[] others)
        {
            if (dictAliases == null)
                dictAliases = new Dictionary<object, string>();
            foreach (EntityBase entity in others)
            {
                string aliases = "T" + dictAliases.Count;
                dictAliases.Add(entity, aliases);
                entity.PropertyGetting += new EventHandler<PropertyGettingEventArgs>(e_PropertyGetting);
            }
        }

        public OQL(OQL parent, EntityBase e)
            : this(e)
        {
            parentOql = parent;
            parent.haveChildOql = true;
        }

        /// <summary>
        /// 以一个实体类实例对象初始化OQL对象。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static OQL From(EntityBase e)
        {
            return new OQL(e);
        }

        public static OQL From(EntityBase e, params EntityBase[] others)
        {
            return new OQL(e, others);
        }

        public static OQL From(OQL parent, EntityBase e)
        {
            return new OQL(parent, e);
        }

        /// <summary>
        /// 根据接口类型，返回查询数据的泛型OQL表达式
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   List<User> users=OQL.From<User>.ToList();
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns>OQL表达式</returns>
        public static GOQL<T> FromObject<T>() where T : class
        {
            T obj = EntityBuilder.CreateEntity<T>();
            EntityBase eb = obj as EntityBase;
            if (eb == null)
                throw new ArgumentException("类型的实例必须是继承EntityBase的子类！");
            OQL q = OQL.From(eb);

            return new GOQL<T>(q, obj);
        }

        /// <summary>
        /// 根据实体类类型，返回查询数据的泛型OQL表达式
        /// <remarks>2015.2.15 修改成返回GOQL的方式，避免之前可能的误用</remarks>
        /// <example>
        /// <code>
        /// <![CDATA[
        ///   List<User> users=OQL.From<User>.ToList<User>();
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <returns>OQL表达式</returns>
        public static GOQL<T> From<T>() where T :EntityBase,new ()
        {
            T entity = new T();
            OQL q = new OQL(entity);
            return new GOQL<T>(q, entity);
        }

        //public static GOQL<Entity> FromObject<T, Entity>() 
        //    where T : new() 
        //    where Entity:EntityBase
        //{
        //    T entity = new T();
        //    EntityBase eb = entity as EntityBase;
        //    if (eb == null)
        //        throw new ArgumentException("类型的实例必须是继承EntityBase的子类！");
        //    OQL q = OQL.From(eb);
        //    //return new GOQL<T>(q, entity);
        //    return null;
        //}

        #endregion

        #region 连接查询
        /// <summary>
        /// 内连接查询
        /// </summary>
        /// <param name="e">要连接的实体对象</param>
        /// <returns>连接对象</returns>
        public JoinEntity Join(EntityBase e)
        {
            return Join(e, "INNER JOIN");
        }

        /// <summary>
        /// 内连接查询
        /// </summary>
        /// <param name="e">要连接的实体对象</param>
        /// <returns>连接对象</returns>
        public JoinEntity InnerJoin(EntityBase e)
        {
            return Join(e, "INNER JOIN");
        }
        /// <summary>
        /// 左连接查询
        /// </summary>
        /// <param name="e">要连接的实体对象</param>
        /// <returns>连接对象</returns>
        public JoinEntity LeftJoin(EntityBase e)
        {
            return Join(e, "LEFT JOIN");
        }
        /// <summary>
        /// 右连接查询
        /// </summary>
        /// <param name="e">要连接的实体对象</param>
        /// <returns>连接对象</returns>
        public JoinEntity RightJoin(EntityBase e)
        {
            return Join(e, "RIGHT JOIN");
        }

        #endregion

        #region 获取查询字符串 辅助内部方法
        private string ToInsertFromString(string sql)
        {
            Parameters.Clear();
            string sqlTemplate = "INSERT INTO {0}({1}\r\n\t) \r\n{2} ";
            int count = selectedFieldNames.Count;
            string[] insertFieldsString = new string[count];

            for (int i = 0; i < count; i++)
            {
                int a = selectedFieldNames[i].IndexOf('[');
                int b = selectedFieldNames[i].IndexOf(']');
                string realField = selectedFieldNames[i].Substring(a + 1, b - a - 1);
                insertFieldsString[i] = selectedFieldNames[i];
            }

            sql = string.Format(sqlTemplate, mainTableName
                , string.Join(",", insertFieldsString)
                , insertFromOql);

            foreach (string key in insertFromOql.Parameters.Keys)
            {
                Parameters.Add(key, insertFromOql.Parameters[key]);
            }
            return sql;
        }

        private string ToInsertString(string sql)
        {
            Parameters.Clear();
            string sqlTemplate = "INSERT INTO {0}({1}) \r\nVALUES\r\n    ({2}) ";
            int count = selectedFieldNames.Count;
            string[] insertFieldsString = new string[count];
            string[] valuesString = new string[count];
            for (int i = 0; i < count; i++)
            {
                int a = selectedFieldNames[i].IndexOf('[');
                int b = selectedFieldNames[i].IndexOf(']');
                string realField = selectedFieldNames[i].Substring(a + 1, b - a - 1);
                insertFieldsString[i] = selectedFieldNames[i];
                object Value = currEntity.PropertyList(realField);

                TableNameField tnf = new TableNameField();
                tnf.Entity = this.currEntity;
                tnf.Field = realField;
                string paraName = CreateParameter(tnf,Value);
                valuesString[i] = paraName;
            }

            sql = string.Format(sqlTemplate, mainTableName
                , string.Join(",", insertFieldsString)
                , string.Join(",", valuesString));
            return sql;
        }

        private string ToUpdateString(string sql)
        {
            //if (selectedFieldNames.Count == 0)
            //    throw new ArgumentException("UPDATE 操作未指定任何要更新的字段！");
            sql = this.sql_from + GetWhereString();
            return sql;
        }

        private string ToSelectString(string sql)
        {
            string sqlVar = "";
            if (this.Distinct)
                sqlVar += " DISTINCT ";
            if (TopCount >= 0)
                sqlVar += " TOP " + TopCount + " ";//仅限于SQLSERVER/ACCESS

            #region 校验GROUP BY 子句
            string sqlFunTemp = string.Empty;
            if (sqlFunctionString.Length > 0) //是否有聚合函数
            {
                sqlFunTemp = sqlFunctionString;
                if (selectedFieldNames.Count > 0)
                {
                    //GROUP BY 
                    if (GroupbyFieldNames.Count == 0)
                        throw new FormatException("在SELECT 子句中使用聚合、统计函数，如果同时选取了查询的列，那么SQL必须使用GROUP BY 子句！");
                    sqlFunTemp = "," + sqlFunTemp;
                }
            }
            else
            {
                //没有聚合函数，也得检查选择的字段是否在分组的字段内
                int count = GroupbyFieldNames.Count;
                if (count > 0)
                {
                    if (selectedFieldNames.Count == 0)
                        throw new FormatException("如果使用GROUP BY 子句，那么在SELECT 子句中中必须指明要选取的列！");
                    foreach (string str in selectedFieldNames)
                    {
                        string item = str.Trim();
                        if (!GroupbyFieldNames.Contains(item))
                            throw new FormatException("如果使用GROUP BY 子句，那么在SELECT 子句中查询的列必须也在GROUP BY 子句中出现！错误的列：" + item);
                    }

                }
            }
            #endregion

            

            if (dictAliases != null)//有关联查询
            {
                //处理字段别名问题
                string aliases = null;
                sql_fields = string.Empty;
                foreach (var tnf in selectedFieldInfo)
                {
                    if (dictAliases.TryGetValue(tnf.Entity, out aliases))
                    {
                        sql_fields += ",\t\r\n" + string.Format(" {0}.[{1}] AS [{0}_{1}]", aliases, tnf.Field);//关联查询，此处可能需要考虑字段AS别名 问题
                    }
                    else
                    {
                        sql_fields += ",\t\r\n" + string.Format(" M.[{0}]", tnf.Field);
                        //sql_fields += ",\t\r\n" + string.Format(" M.[{0}] AS [M_{0}]", tnf.Field);//test only
                    }
                }
               
                sql_fields = sql_fields.TrimStart(',');

                sql_from = mainTableName + " M ";
                if (sql_fields == "" && sqlFunctionString.Length == 0)
                {
                    if (SelectStar)
                        sql_fields = "*";//网友 大大宝 增加该分支
                    else
                    {
                        sql_fields = "M.*";
                        foreach (string str in dictAliases.Values)
                        {
                            sql_fields += string.Format(",{0}.*", str);
                        }
                    }
                }
            }
            else
            {
                sql_fields = string.Join(",", selectedFieldNames.ToArray());
                sql_from = mainTableName;
                if (sql_fields == "" && sqlFunctionString.Length == 0)
                {
                    if (SelectStar)
                        sql_fields = "*";
                    else
                        sql_fields = "[" + string.Join("],[", this.currEntity.PropertyNames) + "]";// "*";
                }
                if (haveChildOql)
                    sql_from = mainTableName + " M ";
            }



            sql = string.Format("SELECT {0} {1} {2} \r\nFROM {3} {4} {5} "
                , sqlVar
                , sql_fields
                , sqlFunTemp
                , sql_from
                , serverLock == SqlServerLock.UNKNOW ? "" : "WITH(" + serverLock.ToString() + ") "
                , oqlString);

            if (this.PageEnable)
            {
                if (this.PageField == "" && sql.IndexOf(" order by ", StringComparison.OrdinalIgnoreCase) <= 0)
                {
                    if (this.currEntity.PrimaryKeys == null || this.currEntity.PrimaryKeys.Count == 0)
                        throw new Exception("OQL 分页错误，没有指明分页标识字段，也未给当前实体类设置主键。");
                    this.PageField = this.currEntity.PrimaryKeys[0];
                }
            }
            return sql;
        }

        /// <summary>
        /// 获取条件字符串，如果未限定条件，则使用主键的值
        /// </summary>
        /// <returns></returns>
        private string GetWhereString()
        {
            string whereString = oqlString;
            if (whereString.Length < 8)
            {
                whereString = " Where 1=1 ";

                if (this.currEntity.PrimaryKeys.Count == 0)
                    throw new Exception("未指定操作实体的范围，也未指定实体的主键。");
                foreach (string pk in this.currEntity.PrimaryKeys)
                {
                    TableNameField tnf = new TableNameField();
                    tnf.Entity = this.currEntity;
                    tnf.Field = pk;
                    string paraName = CreateParameter(tnf,currEntity.PropertyList(pk));
                    whereString += " And [" + pk + "] =" + paraName + ",";

                }
                whereString = whereString.TrimEnd(',');
                //去除下一次生成重复的条件
                oqlString = whereString;
            }
            return whereString;
        }

        #endregion

        #region 其它方法

        private OQLCondition _condtion;
        /// <summary>
        /// 获取当前条件比较对象
        /// </summary>
        public OQLCondition Condition
        {
            get
            {
                if (_condtion == null)
                    _condtion = new OQLCondition(this);
                return _condtion;
            }
        }

        public override string ToString()
        {
            string sql = string.Empty;
            if (optFlag == OQL_SELECT)
            {
                sql = ToSelectString(sql);
            }
            else if (optFlag == OQL_UPDATE || optFlag == OQL_UPDATE_SELFT)
            {
                sql = ToUpdateString(sql);
            }
            else if (optFlag == OQL_DELETE)
            {
                string sqlUpdate = "DELETE FROM " + mainTableName + " ";
                sql = sqlUpdate + GetWhereString();
            }
            else if (optFlag == OQL_INSERT)
            {
                sql = ToInsertString(sql);

            }
            else if (optFlag == OQL_INSERT_FROM)
            {
                sql = ToInsertFromString(sql);

            }

            return sql;
        }

        public string PrintParameterInfo()
        {
            if (Parameters == null || Parameters.Count == 0)
                return "-------No paramter.--------\r\n";
            StringBuilder sb = new StringBuilder();
            foreach (var item in Parameters)
            {
                object fieldValue = item.Value.FieldValue;
                string type = fieldValue == null ? "NULL" : fieldValue.GetType().Name;
                sb.Append(string.Format("  {0}={1} \t Type:{2} \r\n", item.Key, fieldValue, type));
            }
            string paraInfoString = string.Format("--------OQL Parameters information----------\r\n have {0} parameter,detail:\r\n{1}", Parameters.Count, sb);
            return paraInfoString + "------------------End------------------------\r\n";
        }

        /// <summary>
        /// 释放实体类事件挂钩。如果没页手工调用，该方法会在EntityQuery 调用
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;
            this.currEntity.PropertyGetting -= new EventHandler<PropertyGettingEventArgs>(e_PropertyGetting);
            if (this.dictAliases != null)
            {
                foreach (EntityBase item in dictAliases.Keys)
                {
                    item.PropertyGetting -= new EventHandler<PropertyGettingEventArgs>(e_PropertyGetting);
                }
            }
            disposed = true;
        }
        /// <summary>
        /// 如果未选择任何列，生成的SQL语句Select 后面是否用 * 代替。
        /// 用于不想修改实体类结构但又想增加表字段的情况。
        /// </summary>
        public bool SelectStar { get; set; }
        /// <summary>
        /// 制定实体查询的时候表的锁定类型。仅支持SqlServer。
        /// </summary>
        /// <param name="lockType"></param>
        /// <returns></returns>
        public OQL With(SqlServerLock lockType)
        {
            serverLock = lockType;
            return this;
        }

        public OQL With(string sqlLockType)
        {
            sqlLockType = sqlLockType.ToUpper();
            try
            {
                SqlServerLock lockType = (SqlServerLock)Enum.Parse(typeof(SqlServerLock), sqlLockType);
                serverLock = lockType;
                return this;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(sqlLockType + " 不是SQLSERVER 要求的锁定类型！请参见SqlServerLock 枚举定义。");
            }
        }

        /// <summary>
        /// 获取关联的实体类的表名字，如果是关联查询，返回空
        /// </summary>
        /// <returns></returns>
        protected internal string GetEntityTableName()
        {
            if (this.haveChildOql || this.haveJoinOpt)
                return "";
            else
                return this.currEntity.TableName;
        }

        #endregion
    }

    public class OQL1 : OQL4, IOQL1
    {
        private OQL CurrentOQL;


        private string GetWhereFields()
        {
            int count = CurrentOQL.fieldStack.Count;
            TableNameField[] tnfs = new TableNameField[count];
            for (int i = count - 1; i >= 0; i--)
                tnfs[i] = CurrentOQL.fieldStack.Pop();

            string[] fieldNames = new string[count];
            for (int i = 0; i < count; i++)
            {
                TableNameField tnf = tnfs[i];
                string sqlField = CurrentOQL.GetOqlFieldName(tnf);
                tnf.SqlFieldName = sqlField;
                string paraName = CurrentOQL.CreateParameter(tnf,tnf.Entity.PropertyList(tnf.Field));
                fieldNames[i] = string.Format("{0}={1}", sqlField, paraName);
            }
            return string.Join(" AND ", fieldNames);
        }

        public OQL1(OQL oql)
            : base(oql)
        {
            CurrentOQL = oql;
        }

        public OQL2 Where(OQLCondition condition)
        {
            CurrentOQL.sql_condition = condition.ConditionString;
            CurrentOQL.oqlString += "\r\n     WHERE " + CurrentOQL.sql_condition;
            return new OQL2(CurrentOQL);
        }

        /// <summary>
        /// 根据传入的查询参数数组，对字段名执行不区分大小写的比较，生成查询条件。
        /// 注意目前要求QueryParameter 用的是要查询的表的字段名称，而不是实体类的属性名称
        /// </summary>
        /// <param name="queryParas">查询参数数组</param>
        /// <returns>条件表达式</returns>
        public OQL2 Where(QueryParameter[] queryParas)
        {
            Dictionary<string, TableNameField> paras = CurrentOQL.Parameters;
            string[] fields = CurrentOQL.currEntity.PropertyNames;
            string str = "";
            int count = 0;
            foreach (QueryParameter para in queryParas)
            {
                foreach (string temp in fields)//比较字段是否在实体类中
                {
                    if (string.Compare(temp, para.FieldName, true) == 0)
                    {
                        string paraName = temp + (count++);
                        if (!paras.ContainsKey(paraName))
                        {
                            paras.Add(paraName, new TableNameField() { 
                                FieldValue = para.FieldValue , 
                                Field=para.FieldName,
                                Entity = CurrentOQL.currEntity //必须指定，网友[长得没礼貌]发现此问题
                            });
                            string cmpType = "";
                            switch (para.CompareType)
                            {
                                case PWMIS.Common.enumCompare.Equal:
                                    cmpType = "=";
                                    break;
                                case PWMIS.Common.enumCompare.Greater:
                                    cmpType = ">";
                                    break;
                                case PWMIS.Common.enumCompare.Like:
                                    cmpType = " LIKE ";
                                    break;
                                case PWMIS.Common.enumCompare.NoGreater:
                                    cmpType = "<=";
                                    break;
                                case PWMIS.Common.enumCompare.NoSmaller:
                                    cmpType = ">=";
                                    break;
                                case PWMIS.Common.enumCompare.NotEqual:
                                    cmpType = "<>";
                                    break;
                                case PWMIS.Common.enumCompare.Smaller:
                                    cmpType = "<";
                                    break;
                                case PWMIS.Common.enumCompare.IsNull:
                                    cmpType = " IS NULL ";
                                    break;
                                case PWMIS.Common.enumCompare.IsNotNull:
                                    cmpType = " IS NOT NULL ";
                                    break;
                                default:
                                    cmpType = "=";
                                    break;
                            }
                            if (para.CompareType != PWMIS.Common.enumCompare.IsNull && para.CompareType != PWMIS.Common.enumCompare.IsNotNull)
                                str += " AND [" + temp + "]" + cmpType + "@" + paraName;
                            else
                                str += " AND [" + temp + "]" + cmpType;
                        }
                        break;
                    }
                }
            }

            if (str != "")
                str = str.Substring(" AND ".Length);

            CurrentOQL.sql_condition = str;
            CurrentOQL.oqlString += "\r\n     WHERE " + CurrentOQL.sql_condition;
            return new OQL2(CurrentOQL);
        }

        public OQL2 Where(OQLCompare cmpResult)
        {
            return GetOQL2ByOQLCompare(cmpResult);
        }

        public OQL2 Where(OQLCompareFunc cmpFun)
        {
            OQLCompare compare = new OQLCompare(this.CurrentOQL);
            OQLCompare cmpResult = cmpFun(compare);

            return GetOQL2ByOQLCompare(cmpResult);
        }

        public OQL2 Where<T>(OQLCompareFunc<T> cmpFun)
            where T : class
        {
            OQLCompare compare = new OQLCompare(this.CurrentOQL);
            T p1 = GetInstance<T>();

            OQLCompare cmpResult = cmpFun(compare, p1);
            return GetOQL2ByOQLCompare(cmpResult);
        }

        public OQL2 Where<T1, T2>(OQLCompareFunc<T1, T2> cmpFun)
            where T1 : EntityBase
            where T2 : EntityBase
        {
            OQLCompare compare = new OQLCompare(this.CurrentOQL);
            T1 p1 = GetInstance<T1>();
            T2 p2 = GetInstance<T2>();
            OQLCompare cmpResult = cmpFun(compare, p1, p2);
            return GetOQL2ByOQLCompare(cmpResult);
        }

        public OQL2 Where<T1, T2, T3>(OQLCompareFunc<T1, T2, T3> cmpFun)
            where T1 : EntityBase
            where T2 : EntityBase
            where T3 : EntityBase
        {
            OQLCompare compare = new OQLCompare(this.CurrentOQL);
            T1 p1 = GetInstance<T1>();
            T2 p2 = GetInstance<T2>();
            T3 p3 = GetInstance<T3>();
            OQLCompare cmpResult = cmpFun(compare, p1, p2, p3);
            return GetOQL2ByOQLCompare(cmpResult);
        }


        private OQL2 GetOQL2ByOQLCompare(OQLCompare cmpResult)
        {
            if (!object.Equals(cmpResult, null))
            {
                if (CurrentOQL != cmpResult.LinkedOQL)
                    throw new ArgumentException("OQLCompare 关联的OQL 对象不是当前OQL本身对象，请使用OQLCompareFunc或者它的泛型对象。");
                CurrentOQL.sql_condition = cmpResult.CompareString;
                CurrentOQL.oqlString += "\r\n     WHERE " + CurrentOQL.sql_condition;
            }
            return new OQL2(CurrentOQL);
        }
        private T GetInstance<T>() where T : class
        {
            T entity = this.CurrentOQL.GetUsedEntity<T>();
            if (entity == null)
                throw new ArgumentException(typeof(T).ToString() + " 类型的实例没有被OQL对象所使用");

            return entity;
        }

        #region 接口方法
        /// <summary>
        /// 使用实体类选定的属性作为查询条件和条件的值，必须有至少一个参数。该方法不可以多次调用。
        /// 如果想构造动态的查询条件，请使用OQLCompare 对象
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public OQL2 Where(params object[] fields)
        {
            CurrentOQL.sql_condition = GetWhereFields();
            CurrentOQL.oqlString += "\r\n     WHERE " + CurrentOQL.sql_condition;
            return new OQL2(CurrentOQL);
        }
        public OQL3 GroupBy(object field)
        {
            string fieldName = CurrentOQL.TakeOneStackFields().SqlFieldName;
            CurrentOQL.GroupbyFieldNames.Add(fieldName.Trim());
            CurrentOQL.oqlString += "\r\n          GROUP BY " + fieldName;
            return new OQL3(CurrentOQL);
        }

        public OQL3 GroupBy(object field, params object[] others)
        {
            string strTemp = string.Empty;
            string fieldName = CurrentOQL.TakeOneStackFields().SqlFieldName;
            CurrentOQL.GroupbyFieldNames.Add(fieldName.Trim());

            for (int i = 0; i < others.Length; i++)
            {
                string fieldNameTemp = CurrentOQL.TakeOneStackFields().SqlFieldName;
                CurrentOQL.GroupbyFieldNames.Add(fieldNameTemp.Trim());
                strTemp += "," + fieldNameTemp;
            }

            CurrentOQL.oqlString += "\r\n          GROUP BY " + fieldName + strTemp;
            return new OQL3(CurrentOQL);
        }

        public OQL4 Having(object field, object Value, string sqlFunctionFormat)
        {
            OQL3 q3 = new OQL3(CurrentOQL);
            return q3.Having(field, Value, sqlFunctionFormat);
        }

        #endregion

        #region 聚合函数
        /// <summary>
        /// OQL1表达式之统计数量，请在结果实体类中使用PropertyList["字段别名"] 的方式获取查询值
        /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="asFieldName">别名，如果不指定，则使用字段名称</param>
        /// <returns>OQL1</returns>
        public OQL1 Count(object field, string asFieldName)
        {
            string currFieldName = CurrentOQL.TakeStackFields();
            if (string.IsNullOrEmpty(currFieldName))
                currFieldName = "*";
            return sqlFunction("COUNT", currFieldName, asFieldName);
        }

        /// <summary>
        /// OQL1表达式之求最大值，请在结果实体类中使用PropertyList["字段别名"] 的方式获取查询值
        /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="asFieldName">别名，如果不指定，则使用字段名称</param>
        /// <returns>OQL1</returns>
        public OQL1 Max(object field, string asFieldName)
        {
            string currFieldName = CurrentOQL.TakeStackFields();
            return sqlFunction("MAX", currFieldName, asFieldName);
        }

        /// <summary>
        /// OQL1表达式之求最小值，请在结果实体类中使用PropertyList["字段别名"] 的方式获取查询值
        /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="asFieldName">别名，如果不指定，则使用字段名称</param>
        /// <returns>OQL1</returns>
        public OQL1 Min(object field, string asFieldName)
        {
            string currFieldName = CurrentOQL.TakeStackFields();
            return sqlFunction("MIN", currFieldName, asFieldName);
        }

        /// <summary>
        /// OQL1表达式之求合计，请在结果实体类中使用PropertyList["字段别名"] 的方式获取查询值
        /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="asFieldName">别名，如果不指定，则使用字段名称</param>
        /// <returns>OQL1</returns>
        public OQL1 Sum(object field, string asFieldName)
        {
            string currFieldName = CurrentOQL.TakeStackFields();
            return sqlFunction("SUM", currFieldName, asFieldName);
        }

        /// <summary>
        /// OQL1表达式之求平均，请在结果实体类中使用PropertyList["字段别名"] 的方式获取查询值
        /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="asFieldName">字段别名，如果不指定，则使用字段名称</param>
        /// <returns>OQL1</returns>
        public OQL1 Avg(object field, string asFieldName)
        {
            string currFieldName = CurrentOQL.TakeStackFields();
            return sqlFunction("AVG", currFieldName, asFieldName);
        }

        private OQL1 sqlFunction(string sqlFunctionName, string fieldName, string asFieldName)
        {
            if (string.IsNullOrEmpty(asFieldName))
            {
                if (this.CurrentOQL.haveJoinOpt)
                    throw new Exception("有表连接查询的时候，" + sqlFunctionName + " 结果必须指定别名！");
                else
                    asFieldName = fieldName;

            }

            //CurrentOQL.sqlFunctionString = sqlFunctionName + "(" + fieldName + ") AS " + asFieldName;
            //上面一行代码被注释,修改成下面的代码,修正在查询中使用多个函数操作的问题
            //感谢网友 泸州-雷皇 2014.10.29 发现并修复该问题
            if (!string.IsNullOrEmpty(CurrentOQL.sqlFunctionString))
                CurrentOQL.sqlFunctionString += ", ";
            CurrentOQL.sqlFunctionString += sqlFunctionName + "(" + fieldName + ") AS " + asFieldName;

            //asFieldName 必须处理，否则找不到字段。感谢网友Super Show 发现问题 
            if(fieldName==asFieldName)
                this.CurrentOQL.currEntity.setProperty(asFieldName.Trim().TrimStart('[').TrimEnd(']'), 0);
            return this;
        }

        #endregion

    }

    public class OQL2 : OQL4, IOQL2
    {
        private OQL CurrentOQL;
        public OQL2(OQL oql)
            : base(oql)
        {
            CurrentOQL = oql;
        }
        public OQL3 GroupBy(object field)
        {
            string fieldName = CurrentOQL.TakeOneStackFields().SqlFieldName;
            CurrentOQL.GroupbyFieldNames.Add(fieldName.Trim());
            CurrentOQL.oqlString += "\r\n          GROUP BY " + fieldName;
            return new OQL3(CurrentOQL);
        }

        public OQL3 GroupBy(object field, params object[] others)
        {
            string strTemp = string.Empty;
            string fieldName = CurrentOQL.TakeOneStackFields().SqlFieldName;
            CurrentOQL.GroupbyFieldNames.Add(fieldName.Trim());

            for (int i = 0; i < others.Length; i++)
            {
                string fieldNameTemp = CurrentOQL.TakeOneStackFields().SqlFieldName;
                CurrentOQL.GroupbyFieldNames.Add(fieldNameTemp.Trim());
                strTemp += "," + fieldNameTemp;
            }

            CurrentOQL.oqlString += "\r\n          GROUP BY " + fieldName + strTemp;
            return new OQL3(CurrentOQL);
        }
        public OQL4 Having(object field, object Value, string sqlFunctionFormat)
        {
            OQL3 q3 = new OQL3(CurrentOQL);
            return q3.Having(field, Value, sqlFunctionFormat);
        }

        public OQL4 Having(OQLCompareFunc cmpFun)
        {
            OQL3 q3 = new OQL3(CurrentOQL);
            return q3.Having(cmpFun);
        }

    }

    public class OQL3 : OQL4, IOQL3
    {
        private OQL CurrentOQL;
        public OQL3(OQL oql)
            : base(oql)
        {
            CurrentOQL = oql;
        }
        public OQL4 Having(object field, object Value, string sqlFunctionFormat)
        {
            if (string.IsNullOrEmpty(sqlFunctionFormat))
                throw new ArgumentNullException("SQL 格式函数不能为空！");
            if (sqlFunctionFormat.Contains("--") || sqlFunctionFormat.Contains("\'"))
                throw new ArgumentException("SQL 格式函数不合法！");
            if (sqlFunctionFormat.Contains("{0}") && sqlFunctionFormat.Contains("{1}"))
            {
                var tnf = CurrentOQL.TakeOneStackFields();
                string fieldName = tnf.Field;
                string paraName = CurrentOQL.CreateParameter(tnf);
                string havingString = string.Format(sqlFunctionFormat, fieldName, paraName);
                CurrentOQL.oqlString += "\r\n             HAVING " + havingString;
                return new OQL4(CurrentOQL);
            }
            throw new ArgumentException("SQL 格式函数要求类似这样的格式：SUM({0}) > {1}");
        }

        public OQL4 Having(OQLCompareFunc cmpFun)
        {
            OQLCompare compare = new OQLCompare(this.CurrentOQL);
            OQLCompare cmpResult = cmpFun(compare);

            if (!object.Equals(cmpResult, null))
            {
                CurrentOQL.oqlString += "\r\n             HAVING " + cmpResult.CompareString;
            }
            return new OQL4(CurrentOQL);
        }
    }

    public class OQL4 : IOQL4
    {
        private OQL CurrentOQL;

        public OQL4(OQL oql)
        {
            CurrentOQL = oql;
        }

        protected internal void AddOrderType(string orderType)
        {
            CurrentOQL.oqlString += orderType;
        }

        public OQLOrderType OrderBy(object field)
        {
            string temp = CurrentOQL.haveOrderBy ? "," : "\r\n                 ORDER BY ";
            CurrentOQL.haveOrderBy = true;
            CurrentOQL.oqlString += temp + CurrentOQL.TakeOneStackFields().SqlFieldName;
            return new OQLOrderType(this);
        }

        public OQL4 OrderBy(object field, string orderType)
        {
            string strTemp = orderType.ToLower();
            if (strTemp != "asc" && strTemp != "desc")
                throw new FormatException("排序类型错误！");
            string temp = CurrentOQL.haveOrderBy ? "," : "\r\n                 ORDER BY ";
            CurrentOQL.haveOrderBy = true;
            CurrentOQL.oqlString += temp + CurrentOQL.TakeOneStackFields().SqlFieldName + " " + orderType;

            return this;
        }

        public OQL4 OrderBy(OQLOrder order)
        {
            string temp = CurrentOQL.haveOrderBy ? "," : "\r\n                 ORDER BY ";
            CurrentOQL.haveOrderBy = true;
            CurrentOQL.oqlString += temp + order.ToString();

            return this;
        }

        public OQL4 OrderBy(OQLOrderAction orderAct)
        {
            OQLOrder order = new OQLOrder(CurrentOQL);
            orderAct(order);
            return OrderBy(order);
        }

        public OQL4 OrderBy<T>(OQLOrderAction<T> orderAct) where T : class
        {
            OQLOrder order = new OQLOrder(CurrentOQL);
            T para = CurrentOQL.GetUsedEntity<T>();
            orderAct(order, para);
            return OrderBy(order);
        }

        /// <summary>
        /// （网友[有事M我]、[左眼]提供）根据传入的实体类的多个属性的排序信息，从而进行动态的排序；
        /// 适用于不能在OQL表达式里面直接指明排序方式的场景（比如需要从前台传入）。
        /// </summary>
        /// <param name="orderStr">排序数组，形如{"ID desc","Name asc"}</param>
        /// <returns></returns>
        public OQL4 OrderBy(string[] orderStr)
        {
            if (orderStr == null || orderStr.Length <= 0)
                return this;

            string temp = CurrentOQL.haveOrderBy ? "," : "\r\n                 ORDER BY ";
            CurrentOQL.haveOrderBy = true;

            foreach (string str in orderStr)
            {
                string[] tempArr = str.Split(' ');
                string[] orderArr = new string[2];

                string tempArr_0 = tempArr[0].Trim();
                //访问属性名称对应的属性值，得到真正的排序字段
                object Value = CurrentOQL.currEntity[tempArr_0];
                //如果要排序的属性未包含在实体类的属性定义里面，下面将出现异常
                string orderField = CurrentOQL.TakeOneStackFields().SqlFieldName;
                orderArr[0] = tempArr_0;
                if (tempArr.Length == 1)
                {
                    orderArr[1] = "ASC";
                }
                else
                {
                    if (tempArr[1].Equals("desc", StringComparison.OrdinalIgnoreCase))//非desc则全部以asc进行排列
                        orderArr[1] = "DESC";
                    else
                        orderArr[1] = "ASC";
                }

                temp = string.Format("{0} {1} {2},", temp, orderField, orderArr[1]);
            }
            CurrentOQL.oqlString += temp.TrimEnd(',');
            return this;
        }


        public OQL END
        {
            get
            {
                return CurrentOQL;
            }
        }
    }

    public class OQLOrderType
    {
        private OQL4 CurrentOQL4;

        public OQLOrderType(OQL4 oql4)
        {
            CurrentOQL4 = oql4;
        }

        public OQL4 Desc
        {
            get
            {
                CurrentOQL4.AddOrderType(" DESC ");
                return CurrentOQL4;
            }
        }

        public OQL4 Asc
        {
            get
            {
                CurrentOQL4.AddOrderType(" ASC ");
                return CurrentOQL4;
            }
        }

        public OQL END
        {
            get
            {
                return CurrentOQL4.END;
            }
        }
    }

    /// <summary>
    /// OQL语法之实体连接
    /// </summary>
    public class JoinEntity
    {
        private OQL _mainOql;
        private string _joinType;
        private EntityBase _joinedEntity;

        public string JoinedString { get; private set; }
        public string LeftString { get; private set; }
        public string RightString { get; private set; }
        /// <summary>
        /// 以一个OQL对象关联本类，内部使用的构造函数
        /// </summary>
        /// <param name="mainOql">关联的主OQL对象</param>
        /// <param name="entity">要关联的实体类</param>
        /// <param name="joinType">关联类型，分为左连接，右连接，外连接</param>
        public JoinEntity(OQL mainOql, EntityBase entity, string joinType)
        {
            this._mainOql = mainOql;
            this._joinType = joinType;
            this._joinedEntity = entity;

        }
        /// <summary>
        /// 指定要关联查询的实体类属性（内部对应字段）
        /// <example>
        /// <code>
        /// <![CDATA[
        /// var q1 = OQL.From(a)
        ///             .InnerJoin(b).On(a.ID,b.ID)
        ///             .Select(a.ID,a.Name,b.Name)
        ///          .END;
        /// //感谢网友 Tony 扩展了下面的使用方式         
        /// var q2 =  OQL.From(a)
        ///              .LeftJoin(b).On(a.ID,b.ID, a.Name,b.Name)
        ///              .Select(a.ID,a.Name,b.OtherInfo)
        ///          .END;
        /// ]]>
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="fields">主实体类的关联的属性与从实体关联的属性，必须成对出现</param>
        /// <returns></returns>
        public OQL On(params object[] fields)
        {
            if (fields.Length > 0)
            {
                if (fields.Length % 2 > 0)
                    throw new Exception("参数个数应为偶数！");
                int count = fields.Length;

                TableNameField tnfRight = this._mainOql.fieldStack.Pop();
                TableNameField tnfLeft = this._mainOql.fieldStack.Pop();
                LeftString = this._mainOql.GetOqlFieldName(tnfLeft);
                RightString = this._mainOql.GetOqlFieldName(tnfRight);

                this.JoinedString = string.Format("\r\n{0} [{1}] {2}  ON {3} ={4} ", _joinType,
                    _joinedEntity.GetTableName(),
                    this._mainOql.GetTableAliases(_joinedEntity),
                    LeftString,
                    RightString);
                this._mainOql.oqlString += this.JoinedString;
                for (int i = 0; i < (count - 2) / 2; i++)
                {
                    TableNameField tnf1 = this._mainOql.fieldStack.Pop();
                    TableNameField tnf2 = this._mainOql.fieldStack.Pop();
                    string tnf1String = this._mainOql.GetOqlFieldName(tnf1);
                    string tnf2String = this._mainOql.GetOqlFieldName(tnf2);
                    this._mainOql.oqlString += string.Format(" AND {0} = {1} ", tnf1String, tnf2String);
                }
            }
            return this._mainOql;
        }
        /// <summary>
        /// （OQL内部使用）添加要关联的字段名
        /// </summary>
        /// <param name="fieldName"></param>
        public void AddJoinFieldName(string fieldName)
        {
            if (string.IsNullOrEmpty(this.LeftString))
                this.LeftString = fieldName;
            else if (string.IsNullOrEmpty(this.RightString))
                this.RightString = fieldName;
        }
    }

    /// <summary>
    /// OQL 条件对象，兼容老版本的OQL2对象。建议使用OQLCompare对象构造复杂的条件
    /// </summary>
    public class OQLCondition
    {
        private OQL CurrentOQL;
        private string conditionString;

        public OQLCondition(OQL oql)
        {
            CurrentOQL = oql;
            conditionString = " 1=1 ";
        }

        public string ConditionString
        {
            get { return conditionString; }
        }

        private OQLCondition subCondition(string logicType, string currFieldName, string compareType, object Value)
        {
            if (compareType == null || compareType == "")
                compareType = "=";
            else
                compareType = compareType.Trim().ToLower();

            if (compareType == "=" || compareType == ">=" || compareType == ">" || compareType == "<=" || compareType == "<" || compareType == "<>" || compareType.StartsWith("like"))
            {
                TableNameField tnf = new TableNameField();
                tnf.Entity = this.CurrentOQL.currEntity;
                tnf.Field = currFieldName;
                tnf.FieldValue = Value;
                conditionString += logicType +"[" +currFieldName + "] " + compareType + " " + CurrentOQL.CreateParameter(tnf);

            }
            else if (compareType.StartsWith("is"))
            {
                string strValue = Value.ToString().ToUpper();
                if (strValue == "NULL" || strValue == "NOT NULL")
                    conditionString += logicType + currFieldName + " IS " + strValue;
                else
                    throw new FormatException("IS 条件只能是NULL或者 NOT NULL");
            }
            else
            {
                throw new Exception("比较符号必须是 =,>,>=,<,<=,<>,like,is 中的一种。");
            }
            return this;
        }

        /// <summary>
        /// 选取 与 条件
        /// </summary>
        /// <param name="field">实体对象的属性</param>
        /// <param name="compareType">SQL 比较条件，如"=","LIKE","IS" 等</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>多条件表达式</returns>
        public OQLCondition AND(object field, string compareType, object Value)
        {
            string currFieldName = CurrentOQL.TakeOneStackFields().Field;
            return subCondition(" AND ", currFieldName, compareType, Value);
        }

        /// <summary>
        /// 选取 或 条件
        /// </summary>
        /// <param name="field">实体对象的属性</param>
        /// <param name="compareType">SQL 比较条件，如"=","LIKE","IS" 等</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>多条件表达式</returns>
        public OQLCondition OR(object field, string compareType, object Value)
        {
            string currFieldName = CurrentOQL.TakeOneStackFields().Field;
            return subCondition(" OR  ", currFieldName, compareType, Value);

        }


        /// <summary>
        /// 选取 非 条件
        /// </summary>
        /// <param name="field">实体对象的属性</param>
        /// <param name="compareType">SQL 比较条件，如"=","LIKE","IS" 等</param>
        /// <param name="Value">要比较的值</param>
        /// <returns>多条件表达式</returns>
        public OQLCondition NOT(object field, string compareType, object Value)
        {
            string currFieldName = CurrentOQL.TakeOneStackFields().Field;
            return subCondition(" NOT ", currFieldName, compareType, Value);

        }


        /// <summary>
        /// 选取 字段 列表条件
        /// </summary>
        /// <param name="field">实体对象的属性</param>
        /// <param name="Values">值列表</param>
        /// <returns>条件表达式</returns>
        public OQLCondition IN(object field, object[] Values)
        {
            conditionString += getInCondition(field, Values, "IN ");
            return this;
        }

        /// <summary>
        /// 构造Not In查询条件
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="Values">值数组</param>
        /// <returns></returns>
        public OQLCondition NotIn(object field, object[] Values)
        {
            conditionString += getInCondition(field, Values, " NOT IN ");
            return this;
        }

        private string getInCondition(object field, object[] Values, string inType)
        {
            var tnf=CurrentOQL.TakeOneStackFields();
            string currFieldName = tnf.Field;
            string strInTemp = string.Empty;
            string strResult = string.Empty;
            if (field == null || field.GetType() == typeof(string))
            {
                foreach (object obj in Values)
                {
                    //原来的tnf参数位置是null，如果字段是字符串类型会报错
                    //感谢网友 其其 发现问题，2014.4.17
                    strInTemp += "," + CurrentOQL.CreateParameter(tnf,obj.ToString());
                }
            }
            else if (field.GetType() == typeof(DateTime))
            {
                foreach (object obj in Values)
                {
                    strInTemp += "," + CurrentOQL.CreateParameter(tnf, (DateTime)obj);
                }
            }
            else
            {
                foreach (object obj in Values)
                {
                    if (obj != null)
                        strInTemp += "," + obj.ToString();
                    else
                        strInTemp += ",NULL";

                }
            }

            if (strInTemp != "")
            {
                strResult = " AND [" + currFieldName + "] " + inType + " (" + strInTemp.TrimStart(',') + ")";
            }
            return strResult;
        }

        /// <summary>
        /// 以另外一个OQL条件作为In的子查询
        /// <example>参见 http://www.cnblogs.com/bluedoctor/archive/2011/02/24/1963606.html </example>
        /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="q">OQL表达式</param>
        /// <returns></returns>
        public OQLCondition IN(object field, OQL q)
        {
            return IN(field, q, true);
        }

        /// <summary>
        /// 以另外一个OQL条件作为Not In的子查询
        /// <example>
        /// 参见 http://www.cnblogs.com/bluedoctor/archive/2011/02/24/1963606.html
        /// </example>
         /// </summary>
        /// <param name="field">属性字段</param>
        /// <param name="q">OQL表达式</param>
        /// <returns></returns>
        public OQLCondition NotIn(object field, OQL q)
        {
            return IN(field, q, false);
        }

        private OQLCondition IN(object field, OQL q, bool isIn)
        {
            string inString = isIn ? " IN " : " NOT IN ";
            string childSql = q.ToString().Replace("@P", "@INP").Replace("@CP", "@INCP");
            if (q.sql_fields.IndexOf(',') > 0)
                throw new Exception("OQL 语法错误，包含在In查询中的子查询只能使用1个实体属性，请修改子查询的Select参数。");

            //２０１３．５．２７　增加替换.Replace("@CP","@INCP")，感谢张善友发现此Ｂｕｇ
            string currFieldName = CurrentOQL.TakeOneStackFields().Field;
            conditionString += " AND [" + currFieldName + "]" + inString + "  (\r\n" + childSql + ")";

            //感谢网友 null 发现下面的参数问题
            foreach (string key in q.Parameters.Keys)
                CurrentOQL.Parameters.Add(key.Replace("@P", "@INP").Replace("@CP", "@INCP"), q.Parameters[key]);

            return this;
        }



    }

    /// <summary>
    /// OQL 动态排序对象，用于OQL表达式的OrderBy参数
    /// </summary>
    public class OQLOrder
    {
        private string orderString = string.Empty;
        private OQL CurrentOQL;

        /// <summary>
        /// 获取排序字符串，OQL内部使用
        /// </summary>
        public string OrderByString
        {
            get { return orderString; }
        }

        public OQLOrder(OQL oql)
        {
            this.CurrentOQL = oql;
        }

        /// <summary>
        /// 以一个实体类对象初始化构造函数
        /// </summary>
        /// <param name="entity"></param>
        [Obsolete("构造函数已经过时，请使用其它构造函数")]
        public OQLOrder(EntityBase entity)
        {

        }
        /// <summary>
        /// 以一个或者多个实体类，来构造排序条件类。在OQL多实体类关联查询中需要使用该方法。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="joinedEntitys"></param>
        [Obsolete("构造函数已经过时，请使用其它构造函数")]
        public OQLOrder(EntityBase e, params EntityBase[] joinedEntitys)
        {

        }

        private OQLOrder OrderBy(object field, string orderType)
        {
            if (CurrentOQL == null)
                throw new InvalidOperationException("当前对象实例化的时候需要使用关联的OQL实例，但使用了过期的构造函数。");
            orderString += "," + CurrentOQL.TakeStackFields() + " " + orderType;
            CurrentOQL.fieldStack.Clear();
            return this;
        }

        /// <summary>
        /// 默认排序（ASC）
        /// </summary>
        /// <param name="field">要排序的实体属性</param>
        /// <returns></returns>
        public OQLOrder OrderBy(object field)
        {
            return OrderBy(field, "ASC");
        }
        /// <summary>
        /// 升序排序ASC
        /// </summary>
        /// <param name="field">要排序的实体属性</param>
        /// <returns></returns>
        public OQLOrder Asc(object field)
        {
            return OrderBy(field, "ASC");
        }
        /// <summary>
        /// 倒序排序DESC
        /// </summary>
        /// <param name="field">要排序的实体属性</param>
        /// <returns></returns>
        public OQLOrder Desc(object field)
        {
            return OrderBy(field, "DESC");
        }
        /// <summary>
        /// 重置排序状态
        /// </summary>
        public void ReSet()
        {
            //currPropName = string.Empty;
            orderString = string.Empty;
        }
        /// <summary>
        /// 获取排序信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return orderString.Substring(1);
        }

    }

}
