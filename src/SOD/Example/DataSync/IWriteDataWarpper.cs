using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOD.DataSync
{
    /// <summary>
    /// 数据写操作包装器接口
    /// </summary>
    public interface IWriteDataWarpper : System.IDisposable
    {
        /// <summary>
        /// 快速插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        void QuickInsert<T>(List<T> list) where T : EntityBase, new();
        /// <summary>
        /// 快速删除数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        void QuickDelete<T>(List<T> list) where T : EntityBase, new();
    }
}
