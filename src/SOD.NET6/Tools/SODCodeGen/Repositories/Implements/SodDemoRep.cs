using Sod.Pwmis.com.Entity;
using Sod.Pwmis.com.Interface.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sod.Pwmis.com.Repository.Implements
{
    
    public partial class SodDemoRep : ISodDemoRep
    {
        //SodDemoRep.designer.cs
        protected override void SetPropertyDefaultValue(SodDemoEntity entity)
        {
            // //Example:
            //entity.Name = "";
            //entity.AtTime = DateTime.Now;
        }
    }
}
