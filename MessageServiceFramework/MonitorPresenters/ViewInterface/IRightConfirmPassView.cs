using System;
using System.Collections.Generic;
 

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IRightConfirmPassView
    {
        /// <summary>
        /// 取目的地名称
        /// </summary>
        /// <param name="cityName"></param>
        void GetCity(string cityName);

    }
}
