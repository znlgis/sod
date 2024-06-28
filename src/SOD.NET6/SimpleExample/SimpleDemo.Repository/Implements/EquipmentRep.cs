using SimpleDemo.Entity;
using SimpleDemo.Interface;
using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface.IRepositories;

namespace SimpleDemo.Repository.Implements
{
    public class EquipmentRep : BaseRepository<int, IEquipment, EquipmentEntity>, IEquipmentRep
    {
        public EquipmentRep(IUowManager dbContext) : base(dbContext)
        {
        }

        public int TestAdd(IEquipment equipment)
        {
            Console.WriteLine("Test Add equipment,ID={0},Name={1}", equipment.ID, equipment.EquipmentName);
            return 1;
        }
    }
}
