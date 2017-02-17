using System;
using System.Collections.Generic;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;

namespace PWMIS.DataMap.Entity
{
    /// <summary>
    /// 泛型OQL查询类，通常查询单表实体，如果是多表查询，请使用 EntityContainer
    /// </summary>
    /// <typeparam name="T">实体类类型或者POCO接口类型</typeparam>
    public class GOQL<T> where T:class
    {
        protected internal OQL currentOQL;
        private T currentEntity;
        public delegate object[] SelectFieldFunc(T s);

        /// <summary>
        /// 获取查询后的总记录数，该属性结合Limit方法使用，并且在调用ToList等方法之后再调用才有效。
        /// </summary>
        public int AllCount { get {
            return this.currentOQL.PageWithAllRecordCount;        
        } }

        public GOQL(OQL oql,T entity)
        {
            this.currentOQL = oql;
            this.currentEntity = entity;
        }
        public GOQL1<T> Select()
        {
            return new GOQL1<T>(this, currentOQL.Select());
        }
        public GOQL1<T> Select(SelectFieldFunc func)
        {
            return new GOQL1<T>(this, currentOQL.Select(func(currentEntity)));
        }
        /// <summary>
        /// 以每页不超过 pageSize 条记录，查询第 pageNumber 页的数据
        /// </summary>
        /// <param name="pageSize">每页显示的记录数量</param>
        /// <param name="pageNumber">所在页页码，从1开始</param>
        /// <returns>GOQL对象</returns>
        public GOQL<T> Limit(int pageSize, int pageNumber)
        {
            this.currentOQL.Limit(pageSize, pageNumber);
            return this;
        }
        /// <summary>
        /// 以每页不超过 pageSize 条记录，查询第 pageNumber 页的数据。如果没有指定排序方法，可以不指定记录总数，
        /// 但不可指定为0，或者使用另外一个重载。
        /// </summary>
        /// <param name="pageSize">每页显示的记录数量</param>
        /// <param name="pageNumber">所在页页码，从1开始</param>
        /// <param name="allCount">记录总数</param>
        /// <returns></returns>
        public GOQL<T> Limit(int pageSize, int pageNumber,int allCount)
        {
            this.currentOQL.PageWithAllRecordCount = allCount;
            this.currentOQL.Limit(pageSize, pageNumber);
            return this;
        }

        /// <summary>
        /// 以每页不超过 pageSize 条记录，查询第 pageNumber 页的数据，并可以指定查询后获取本次查询的总记录数。
        /// 可以通过 AllCount 属性获取此总记录数
        /// </summary>
        /// <param name="pageSize">每页显示的记录数量</param>
        /// <param name="pageNumber">所在页页码，从1开始</param>
        /// <param name="autoAllCount">是否自动查询总记录数</param>
        /// <returns></returns>
        public GOQL<T> Limit(int pageSize, int pageNumber, bool autoAllCount)
        {
            this.currentOQL.Limit(pageSize, pageNumber, autoAllCount);
            return this;
        }

        public GOQL<T> Print(out string sqlInfo)
        {
            sqlInfo = string.Format("SQL:{0}\r\n{1}",currentOQL.ToString(), currentOQL.PrintParameterInfo());
            return this;
        }

        /// <summary>
        /// 提供查询的锁限定(注意:只支持SQLSERVER)(注:本方法由网友 唔 提供)
        /// </summary>
        /// <param name="lockname"></param>
        /// <returns></returns>
        public GOQL<T> With(OQL.SqlServerLock lockname)
        {
            currentOQL.With(lockname);
            return this;
        }

        public List<T> ToList(AdoHelper db )
        {
            return EntityQuery.QueryList<T>(this.currentOQL, db);
        }

        /// <summary>
        /// 将查询结果映射到另外的具有同名属性的类型，比如DTO对象
        /// </summary>
        /// <typeparam name="T1">DTO对象的类型</typeparam>
        /// <param name="db">AdoHelper</param>
        /// <returns>DTO对象列表</returns>
        public List<T1> MapToPOCOList<T1>(AdoHelper db) where T1:class,new ()
        {
            List<T> list= EntityQuery.QueryList<T>(this.currentOQL, db);
            List<T1> dtoList = new List<T1>();

            foreach (var item in list)
            {
                T1 dto = new T1();
                EntityBase entity = item as EntityBase;
                if (entity != null)
                {
                    entity.MapToPOCO(dto);//实体类到dto的映射
                }

                dtoList.Add(dto);
            }
            return dtoList;
        }



        public List<T> ToList()
        {
            return ToList(MyDB.Instance);
        }
        public T ToObject(AdoHelper db)
        {
            return EntityQuery.QueryObject<T>(this.currentOQL, db);
        }
        public T ToObject()
        {
            return ToObject(MyDB.Instance);
        }
        public override string ToString()
        {
            return currentOQL.ToString();
        }

        public OQL END
        {
            get { return this.currentOQL; }
        }
    }

    public class GOQL1<T> : GOQL2<T> where T : class
    {
        private GOQL<T> currentGOQL;
        private OQL1 currentOQL1;

        public GOQL1(GOQL<T> gq,OQL1 q1):base(gq)
        {
            this.currentGOQL = gq;
            this.currentOQL1 = q1;
        }

        public GOQL2<T> Where(OQLCompareFunc<T> func)
        {
            this.currentOQL1.Where(func);
            return new GOQL2<T>(currentGOQL);
        }
    }

    public class GOQL2<T> where T : class
    {
        private GOQL<T> currentGOQL;

        public GOQL2(GOQL<T> gq)
        {
            this.currentGOQL = gq;
        }
        public GOQL<T> OrderBy(OQLOrderAction<T> orderAct)
        {
            OQL4 currentOQL4 = new OQL4(this.currentGOQL.currentOQL).OrderBy<T>(orderAct);
            return this.currentGOQL;
        }
        public GOQL<T> END
        {
            get { return this.currentGOQL; }
        }
    }
}

