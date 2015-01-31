using System;
using System.Collections.Generic;
using System.Text;

namespace PWMIS.Core
{
    class RuntimeHandle
    {
        public System.RuntimeMethodHandle MethodHandle { get; private set; }

        public System.RuntimeTypeHandle TypeHandle { get; private set; }

        public bool IsGenericType { get; private set; }

        public RuntimeHandle(RuntimeMethodHandle methodHandle, RuntimeTypeHandle typeHandle)
        {
            this.MethodHandle = methodHandle;
            this.TypeHandle = typeHandle;
            if (typeHandle != null)
                this.IsGenericType = true;
        }
    }
}
