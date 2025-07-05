using Sod.Pwmis.com.DTO;
using Sod.Pwmis.com.Interface.Infrastructure;
using Sod.Pwmis.com.Interface.IRepositories;
using Sod.Pwmis.com.Interface;
using Sod.Pwmis.com.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Services.Implements
{
    public partial class SodDemoService : ServiceBase<ISodDemo, SodDemoDto, int>
    {
        private readonly ISodDemoRep _repository;
        public SodDemoService(ISodDemoRep repository, ISimpleServiceProvider serviceProvider)
            : base(repository, serviceProvider)
        {
            _repository = repository;
        }
    }
}
