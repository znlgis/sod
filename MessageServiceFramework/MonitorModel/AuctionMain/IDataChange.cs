using System;
using System.Collections.Generic;
using System.Text;

namespace TranstarAuction.Model
{
   public  interface IDataChange
    {
       void OnDataChange(object sender,ShowDelayEntity newEntity, ShowDelayEntity oldEntity);
    }

    
}
