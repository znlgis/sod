using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;
using System;
using System.Collections.Generic;
 

namespace TranstarAuction.Presenters.Presenter
{
    public class SelectTransferLocationPresenter : PresenterBase
    {
        /// <summary>
        /// 得到过户地址
        /// </summary>
        public void GetTransferLocation(int carSourceId, long pubid, Action<string[]> action)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "AuctionTransferAddressName";
            request.Parameters = new object[] { carSourceId, pubid };

            base.ServiceProxy.RequestService<string[]>(request,  DataType.Json, message =>
            {
                //message = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "b", "c", "d", "e", "f", "g", "h", "i", "b", "c", "d", "e", "f", "g", "h", "i", "b", "c", "d", "e", "f", "g", "h", "i", "b", "c", "d", "e", "f", "g", "h", "i", "b", "c", "d", "e", "f", "g", "h", "i", "b", "c", "d", "e", "f", "g", "h", "i" };
                if (action != null)
                    action(message);
            });
        }

        /// <summary>
        /// 获取 拍品表过户地点ID
        /// </summary>
        /// <param name="publishId">拍品ID</param>
        /// <param name="carSourceId">车源ID</param>
        /// <param name="isTakeBySelf">是否上门自提</param>
        /// <param name="guoHuAddrName">过户地址名称</param>
        /// <param name="transferType">1本地过户 2外迁过户</param>
        /// <returns></returns>
        public int GetSysAddressId(long publishId, int carSourceId, bool isTakeBySelf, string guoHuAddrName, int transferType)
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetSysAddressId";
            request.Parameters = new object[] { publishId, carSourceId, isTakeBySelf, guoHuAddrName, transferType };

            var r= base.ServiceProxy.GetServiceMessage<int>(request,  DataType.Text);
            if (r.Succeed)
                return r.Result;
            else
                return 0;
        }
    }
}
