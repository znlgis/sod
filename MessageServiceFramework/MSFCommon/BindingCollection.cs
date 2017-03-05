using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace TranstarAuction.Common
{
    public class BindingCollection<T> : BindingList<T>
    {
        public BindingCollection(IList<T> list)
            : base(list)
        { }
        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        private PropertyDescriptor GetPropertyDescriptor(string propertyName)
        {
            bool findName = false;
            PropertyDescriptorCollection propertyCollection = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor item in propertyCollection)
            {
                if (item.Name == propertyName)
                {
                    findName = true;
                    return item;
                }
            }
            if (!findName)
            {
                throw (new Exception("排序字段名不存在!"));
            }
            else
                return null;
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;

            if (items != null)
            {
                ObjectPropertyCompare<T> pc = new ObjectPropertyCompare<T>(property, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }

            sortProperty = property;
            sortDirection = direction;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        //排序
        public void Sort(PropertyDescriptor property, ListSortDirection direction)
        {
            this.ApplySortCore(property, direction);
        }

        public void Sort(string sortPropertyName, ListSortDirection direction)
        {
            //if (isAscending)
            //    sortDirection = ListSortDirection.Ascending;
            //else
            //    sortDirection = ListSortDirection.Descending;
            PropertyDescriptor propertyDescriptor = GetPropertyDescriptor(sortPropertyName);
            Sort(propertyDescriptor, direction);
        }

        public List<T> FindAll(Predicate<T> match)
        {
            List<T> items = this.Items as List<T>;
            return items.FindAll(match);
        }

        public T Find(Predicate<T> match)
        {
            List<T> items = this.Items as List<T>;
            return items.Find(match);
        }

        public List<T> GetRange(int index, int count)
        {
            List<T> items = this.Items as List<T>;
            return items.GetRange(index, count);
        }

        public bool Exists(Predicate<T> match)
        {
            List<T> items = this.Items as List<T>;
            return items.Exists(match);
        }
    }

    class ObjectPropertyCompare<T> : System.Collections.Generic.IComparer<T>
    {
        private PropertyDescriptor property;
        private ListSortDirection direction;

        public ObjectPropertyCompare(PropertyDescriptor property, ListSortDirection direction)
        {
            this.property = property;
            this.direction = direction;
        }

        #region IComparer<T>

        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="x">相对属性x</param>
        /// <param name="y">相对属性y</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            object xValue = x.GetType().GetProperty(property.Name).GetValue(x, null);
            object yValue = y.GetType().GetProperty(property.Name).GetValue(y, null);

            int returnValue;

            if (xValue is IComparable)
            {
                returnValue = ((IComparable)xValue).CompareTo(yValue);
            }
            else if (xValue.Equals(yValue))
            {
                returnValue = 0;
            }
            else
            {
                returnValue = xValue.ToString().CompareTo(yValue.ToString());
            }

            if (direction == ListSortDirection.Ascending)
            {
                return returnValue;
            }
            else
            {
                return returnValue * -1;
            }
        }

        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
