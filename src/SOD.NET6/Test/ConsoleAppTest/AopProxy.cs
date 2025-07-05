using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    public class Example
    {
        public int Add(int a, int b)
        {
            //Console.WriteLine("Before method call");
            var result = a + b;
            //Console.WriteLine("After method call");
            return result;
        }
    }

    public class AopProxy<T>
    {
        private readonly T _target;

        public AopProxy(T target)
        {
            _target = target;
        }

        public TProxy CreateProxy<TProxy>()
        {
            var proxyType = CreateProxyType();
            var constructor = proxyType.GetConstructor(new[] { typeof(T) });
            return (TProxy)constructor.Invoke(new object[] { _target });
        }

        private Type CreateProxyType()
        {
            // 创建表达式树来定义代理类型
            var method = typeof(T).GetMethod("Add");
            var parameters = method.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();

            // 表达式树定义方法调用前的行为
            var beforeCall = Expression.Call(Expression.Constant(_target), method, parameters);

            // 创建Lambda表达式
            var lambda = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(method.DeclaringType, method.ReturnType),
                Expression.Block(
                    Expression.Call(
                        null,
                        typeof(Console).GetMethod("WriteLine", new[] { typeof(string) }),
                        Expression.Constant("Before method call")
                    ),
                    beforeCall
                ),
                parameters
            );

            // 创建代理类型
            var proxyType = Expression.Lambda<Func<Type>>(
                Expression.New(
                    typeof(AopProxy<>).MakeGenericType(method.DeclaringType).GetConstructor(Type.EmptyTypes),
                    Expression.Constant(_target)
                )
            ).Compile()();

            // 省略其他代码...
            return proxyType;
        }
    }
}
