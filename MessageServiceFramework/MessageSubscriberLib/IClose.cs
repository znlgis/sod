using System;
using System.Collections.Generic;
using System.Text;

namespace MessageSubscriber
{
    public interface IClose
    {
        void Close(int flag);
    }
}
