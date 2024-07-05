using SimpleDemo.Model;

namespace SimpleDemo.Interface
{
    public interface IIdentityProperty<T>
    {
        T ID { get; set; }
    }

    public interface IAutoID<Tid> : IIdentityProperty<Tid>
    {
        Tid GetID();
    }
    public class StringAutoID : IAutoID<string>
    {
        public StringAutoID() { this.ID = GetID(); }
        public string ID { get; set; }
        public string GetID()
        {
            return Guid.NewGuid().ToString();
        }
    }
    public class GuidAutoID : IAutoID<Guid>
    {
        public GuidAutoID() { this.ID = GetID(); }
        public Guid ID { get; set; }
        public Guid GetID()
        {
            return Guid.NewGuid();
        }
    }
    public class LongAutoID : IAutoID<long>
    {
        public LongAutoID() { this.ID = GetID(); }
        public long ID { get; set; }
        public long GetID()
        {
            return CommonUtil.NextSeqNumber();
        }
    }
}
