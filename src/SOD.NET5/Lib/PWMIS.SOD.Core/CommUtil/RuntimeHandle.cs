using System;

namespace PWMIS.Core
{
    internal class RuntimeHandle
    {
        public RuntimeHandle(RuntimeMethodHandle methodHandle, RuntimeTypeHandle typeHandle)
        {
            MethodHandle = methodHandle;
            TypeHandle = typeHandle;
            if (typeHandle != null)
                IsGenericType = true;
        }

        public RuntimeMethodHandle MethodHandle { get; private set; }

        public RuntimeTypeHandle TypeHandle { get; private set; }

        public bool IsGenericType { get; private set; }
    }
}