using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOD.DataSync
{
    /// <summary>
    /// 导出表数据的接口，该接口必须作用于 EntityBase 实现类上
    /// </summary>
    public interface IExportTable
    {
        /// <summary>
        /// 导出操作的批次号，如果为0，表示数据发生了更改，需要导出
        /// </summary>
        int BatchNumber { get; set; }
    }
}
