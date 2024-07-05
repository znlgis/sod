using SimpleDemo.Interface.IRepositories;
using SimpleDemo.Interface.IServices;
using SimpleDemo.Interface;
using SimpleDemo.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleDemo.Model;

namespace SimpleDemo.Service.Implements
{
    //自动生成的代码
    public partial class EquipmentService : ServiceBase<IEquipment, EquipmentDto, int>
    {
        public EquipmentService(IEquipmentRep repository) : base(repository)
        {
        }
    }

    //自定义的代码
    public partial class EquipmentService : IEquipmentService
    {
        public new bool Insert(IEquipment data)
        {
            if (string.IsNullOrEmpty(data.EquipmentID))
            {
                data.EquipmentID = CommonUtil.GetTimeSeqNumberString(data.EquipmentType.Replace("-", ""));
            }
            return base.Insert(data);
        }

    }
}
