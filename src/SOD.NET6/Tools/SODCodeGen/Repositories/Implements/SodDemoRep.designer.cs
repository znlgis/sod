using Sod.Pwmis.com.Entity;
using Sod.Pwmis.com.Interface.Infrastructure;
using Sod.Pwmis.com.Interface;
//@ using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sod.Pwmis.com.Common;

namespace Sod.Pwmis.com.Repository.Implements
{
    //自动生成的代码
    public partial class SodDemoRep : BaseRepository<int, ISodDemo, SodDemoEntity>
    {
        public SodDemoRep(/*[FromKeyedServices(ServiceConst.Key_MyWorkDb)]*/ IUowManager dbContext) : base(dbContext)
        {

        }

    }
}
