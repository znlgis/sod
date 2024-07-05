using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface.IRepositories;
using SimpleDemo.Model.Dtos;

namespace SimpleDemo.Service
{
    public class TestService
    {
        private readonly IUnitOfWork _uow;
        public TestService(IUnitOfWork uow) 
        {
            _uow = uow;
        }

        public void TestUow()
        {
            EquipmentDto equipment = new EquipmentDto();
            //equipment.ID = 1;
            equipment.EquipmentName = "设备名"+DateTime.Now.ToString("HHmmss");
            equipment.EquipmentID = "GB" + DateTime.Now.ToString("yyyyMMddHHmmss");

            var equRep = _uow.GetRepository<IEquipmentRep>();
            var testRep= _uow.GetRepository<ITestRep>();

            _uow.BeginTransaction();
            equRep.Insert(equipment);
            testRep.Add("123");
            _uow.Commit();

            Console.WriteLine("UOW ID=" + _uow.ID);
            Console.WriteLine("UOW Type="+ _uow.GetType().Name);
            //Console.WriteLine("UOW Manager ID="+_uow.)

            Console.WriteLine("EquipmentRep ID="+equRep.ID);
            Console.WriteLine("EquipmentRep UOW ID="+equRep.UnitOfWorkManager.ID);
        }
    }
}
