namespace Sod.Pwmis.com.Interface.Infrastructure
{
    public class SimpleServiceConfigSection
    { 
        public SimpleServiceConfigSection() {
            this.AssemblyName = "";
            this.TypeName = "";
            this.Enable = true;
        }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public bool Enable { get; set; }
    }
    public interface ISimpleServiceContainer
    {
        void ConfigureServices(object container);
    }
    public interface ISimpleServiceProvider
    {
        T GetService<T>() where T : notnull;
        T GetService<TPara,T>(TPara paras);
    }

    public interface IRepositoryProvider
    {
        T GetRepository<T>(IUowManager? context=null) where T : IRepository;
    }
}
