using System;
namespace PWMIS.EnterpriseFramework.ModuleRoute
{
    /// <summary>
    /// 模块运行时
    /// </summary>
    public interface IModuleRuntime
    {
        /// <summary>
        /// 获取在模块中注册的模块提供程序
        /// </summary>
        /// <typeparam name="T">模块提供程序类型</typeparam>
        /// <returns></returns>
        T ResolveProvider<T>() where T : IModuleProvider;

        /// <summary>
        /// 跨模块调用一个方法，并将结果作为对象返回，通常用于目标模块方法的返回值类型在当前模块无法使用的情况。注意内部已经处理异常。
        /// </summary>
        /// <param name="moduleName">要调用的模块名称</param>
        /// <param name="providerName">提供程序名称</param>
        /// <param name="actionName">方法名称</param>
        /// <param name="paramters">参数数组对象</param>
        /// <returns>如果出错，将返回ErrorAction </returns>
        ActionResult ExecuteAction(string moduleName, string providerName, string actionName, object[] paramters);

        /// <summary>
        /// 执行模块方法调用，并将结果类型转换成指定的类型
        /// </summary>
        /// <typeparam name="TResult">结果类型转换指定的类型</typeparam>
        /// <param name="moduleName">模块名称</param>
        /// <param name="providerName">模块提供程序名称</param>
        /// <param name="actionName">模块提供程序的方法</param>
        /// <param name="parameter">模块的方法</param>
        /// <param name="resultMapper">将模块方法调用的结果转换成当前指定类型的委托函数</param>
        /// <returns>如果出错，请检查Error属性 </returns>
        ActionResult<TResult> ExecuteAction<TResult>(string moduleName, string providerName, string actionName, object parameter, Action<object, object> objectMapper)
           where TResult : new();

        /// <summary>
        /// 跨模块调用一个方法并直接使用它的结果，推荐使用此方式，它有较高的效率。注意内部已经处理异常。
        /// </summary>
        /// <typeparam name="T">要调用的方法的参数类型</typeparam>
        /// <typeparam name="TResult">方法返回的结果类型</typeparam>
        /// <param name="moduleName">要调用的模块名称</param>
        /// <param name="providerName">提供程序名称</param>
        /// <param name="actionName">方法名称</param>
        /// <param name="paramter">参数对象</param>
        /// <returns>如果出错，请检查Error属性 </returns>
        ActionResult<TResult> ExecuteAction<T, TResult>(string moduleName, string providerName, string actionName,T parameter);
    }
}
