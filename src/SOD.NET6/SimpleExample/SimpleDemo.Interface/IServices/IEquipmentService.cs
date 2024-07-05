using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemo.Interface.IServices
{
    public interface IEquipmentService : IServiceBase<IEquipment, EquipmentDto, int>
    {
    }
}
