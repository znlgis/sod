using System;
using System.Collections.Generic;
using PWMIS.DataProvider.Data;
using PWMIS.DataProvider.Adapter;

namespace PWMIS.DataMap.Entity
{
    public class GOQL<T> where T:class
    {
        protected internal OQL currentOQL;
        private T currentEntity;
        public delegate object[] SelectFieldFunc(T s);

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
        public GOQL<T> Limit(int pageSize, int pageNumber)
        {
            this.currentOQL.Limit(pageSize, pageNumber);
            return this;
        }
        public GOQL<T> Print(out string sqlInfo)
        {
            sqlInfo = string.Format("SQL:{0}\r\n{1}",currentOQL.ToString(), currentOQL.PrintParameterInfo());
            return this;
        }
        public List<T> ToList(AdoHelper db )
        {
            return EntityQuery.QueryList<T>(this.currentOQL, db);
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

