using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Interface.IRepositories
{
    /// <summary>
    /// 需要对属性值进行编码解码的对象接口
    /// </summary>
    public interface IDecodingObject
    {
        /// <summary>
        /// 获取属性值的时候是否从特定编码（如GB2312）到UTF8解码，默认不解码。
        /// </summary>
         bool DecodingInGetter { get; set; }
        /// <summary>
        /// 设置数据的时候是否从UTF8编码为特定编码（如GB2312），默认编码。
        /// </summary>
         bool EncodingInSetter { get; set; }
    }
}
