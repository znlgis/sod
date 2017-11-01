using System;
using System.Collections.Generic;
 
using TranstarAuction.Presenters.Presenter;

namespace TranstarAuction.Presenters.ViewInterface
{
    public interface IBaseView
    {
        /// <summary>
        /// 主界面调用者
        /// </summary>
        IGlobalPresenter MainPresenter { get; set; }
    }
}
