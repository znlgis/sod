using PWMIS.EnterpriseFramework.Common;
using PWMIS.EnterpriseFramework.Service.Basic;
using System;
using System.Collections.Generic;
 

namespace TranstarAuction.Presenters.Presenter
{
    public class ShowPicturePresenter : PresenterBase
    {
        private int _carSourceId;
        public event EventHandler<ResponsePictureListArgs> ReponsePictureList;

        public ShowPicturePresenter(int carSourceId)
        {
            _carSourceId = carSourceId;
        }

        public void OnReponsePictureList(List<string> picturelist)
        {
            if (ReponsePictureList != null)
            {
                //for (int i = 0; i < 5; i++)
                //    picturelist.AddRange(picturelist.ToArray());
                ReponsePictureList(this, new ResponsePictureListArgs() { PictureList = picturelist });
            }
        }

        public void GetPictureURLList()
        {
             ServiceRequest request = new  ServiceRequest();
            request.ServiceName = "AuctionMainFormService";
            request.MethodName = "GetAllPictureUrlStrings";
            request.Parameters = new object[] { _carSourceId };

            base.ServiceProxy.RequestService<List<string>>(request,  DataType.Json, message =>
            {
                OnReponsePictureList(message);
            }); 
        }
        public enum ImageSize { Big, Middle, Small, Smaller }
        public string GetImgLocation(string orglocation,ImageSize imgsize)
        {
            string destlocation = string.Empty;
            if (!string.IsNullOrEmpty(orglocation))
            {
                switch (imgsize)
                {
                    case ImageSize.Big:
                        destlocation = orglocation.Remove(orglocation.LastIndexOf('_') + 1) + "big" + orglocation.Substring(orglocation.LastIndexOf('.'));
                        break;
                    case ImageSize.Middle:
                        destlocation = orglocation.Remove(orglocation.LastIndexOf('_') + 1) + "middle" + orglocation.Substring(orglocation.LastIndexOf('.'));
                        break;
                    case ImageSize.Small:
                        destlocation = orglocation.Remove(orglocation.LastIndexOf('_') + 1) + "small" + orglocation.Substring(orglocation.LastIndexOf('.'));
                        break;
                }
            }
            return destlocation;
        }
    }

    public class ResponsePictureListArgs : EventArgs
    {
        public List<string> PictureList { get; set; }
    }
}
