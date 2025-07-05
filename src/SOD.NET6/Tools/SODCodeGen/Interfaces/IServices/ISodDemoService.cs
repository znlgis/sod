using Sod.Pwmis.com.DTO;
using Sod.Pwmis.com.Interface.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Interface.IServices
{
    
    public interface ISodDemoService : IServiceBase<ISodDemo, SodDemoDto, int>
    {
    }
}
