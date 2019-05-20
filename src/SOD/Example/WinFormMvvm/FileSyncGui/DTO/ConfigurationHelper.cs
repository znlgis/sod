using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncGui.DTO
{
    public class ConfigurationHelper
    {
        private static ConfigurationHelper _ins;
        private static object lock_obj = new object();
        private Configuration _currentConfig;
        private bool isDllConfig = false;
        /// <summary>
        /// 初始化配置
        /// </summary>
        private ConfigurationHelper()
        {
            isDllConfig = !ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).HasFile;
        }

        /// <summary>
        /// 当前配置辅助类的实例
        /// </summary>
        public static ConfigurationHelper Instance
        {
            get
            {
                if (_ins == null)
                {
                    lock (lock_obj)
                    {
                        if (_ins == null)
                        {
                            _ins = new ConfigurationHelper();
                        }
                    }
                }
                return _ins;
            }
        }

        /// <summary>
        /// 获取或者设置包含配置文件的程序集名称，不包含扩展名。对于单元测试程序集，该属性为空。
        /// </summary>
        public string ConfigurationAssemblyName { get; set; }
        /// <summary>
        /// 当前配置
        /// </summary>
        public Configuration CurrentConfig
        {
            get
            {
                if (_currentConfig == null)
                {
                    if (isDllConfig)
                    {
                        _currentConfig = GetDllConfig();
                    }
                    else
                    {
                        _currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        Assembly ass = Assembly.GetEntryAssembly();
                        if (ass != null)
                            ConfigurationAssemblyName = ass.GetName().Name;
                    }
                }
                return _currentConfig;
            }
        }

        /// <summary>
        /// 获取数据库连接设置
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public ConnectionStringSettings ConnectionSettings(string connName)
        {
            return CurrentConfig.ConnectionStrings.ConnectionStrings[connName];
        }

        /// <summary>
        /// 取得配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String GetAppSetting(string key)
        {
            var setting = CurrentConfig.AppSettings.Settings[key];
            if (setting != null)
                return setting.Value;
            else
                return "";
        }

        private Configuration GetDllConfig()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (string.IsNullOrEmpty(ConfigurationAssemblyName))
            {
                ConfigurationAssemblyName = assembly.GetName().Name;
            }

            Uri uri;
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();

            uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            map.ExeConfigFilename = Path.Combine(uri.LocalPath, ConfigurationAssemblyName + ".dll.config");
            return ConfigurationManager.OpenMappedExeConfiguration(map, 0);
        }
        /*
        /// <summary>
        /// 获取DLL配置文件的应用程序配置信息，例如，名字为ABC程序集调用了GetDllAppSetting，那么应该有一个 ABC.dll.config文件
        /// </summary>
        /// <param name="key">配置的项名称</param>
        /// <returns>配置值</returns>
        public static string GetDllAppSetting(string key)
        {
            Uri uri;
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            Assembly assembly = Assembly.GetExecutingAssembly();
            uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            map.ExeConfigFilename = Path.Combine(uri.LocalPath, assembly.GetName().Name + ".dll.config");
            string str = ConfigurationManager.OpenMappedExeConfiguration(map, 0).AppSettings.Settings[key].Value;
            return str;
        }

        /// <summary>
        /// 获取DLL配置文件的应用程序配置的连接字符串
        /// </summary>
        /// <param name="connName">连接字符串名字</param>
        /// <returns></returns>
        public static ConnectionStringSettings GetDllConnectionSettings(string connName)
        {
            Uri uri;
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            Assembly assembly = Assembly.GetExecutingAssembly();
            uri = new Uri(Path.GetDirectoryName(assembly.CodeBase));
            map.ExeConfigFilename = Path.Combine(uri.LocalPath, assembly.GetName().Name + ".dll.config");
            ConnectionStringSettings connSetting = ConfigurationManager.OpenMappedExeConfiguration(map, 0).
                ConnectionStrings.ConnectionStrings[connName];
            //ConnectionStringSettingsCollection
            return connSetting;
        }
        */
    }
}
