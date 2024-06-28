using SimpleDemo.Interface.Infrastructure;

namespace SimpleDemo.Interface.IRepositories
{
    public interface IEquipmentRep: IRepository<IEquipment, int>
    {
        int TestAdd(IEquipment equipment);
    }
}
