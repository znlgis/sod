using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PermissionControlSystem;

namespace PCS.DomainModel.FileSystem
{
    public class FileOperation : IOperation
    {
        public FileOperation()
        { 
        
        }

        public FileOperation(string optName)
        {
            this.OperationName = optName;
        }

        public string OperationName
        {
            get;
            set;
        }
        /// <summary>
        /// 执行实现类的操作，如果需要参数，需要先设置本类的参数
        /// </summary>
        /// <returns></returns>
        public object Execute()
        {
           return dnycExecute(this, this.OperationName, this.Parameters);
        }

        private object dnycExecute(object tartgetObj, string method, object[] paras)
        {
            //调用动态执行方法
            return null;
        }

        /// <summary>
        /// 操作的参数
        /// </summary>
       public  object[] Parameters { get; set; }


    }

    public class FileCreate : FileOperation
    {
        public FileCreate()
        {
            base.OperationName = "Create";
        }

        //具体的操作方法
        public bool Create(string fileName)
        {
            try
            {
                var f = System.IO.File.Create(fileName);
                f.Close();
                return true;
            }
            catch
            {

            }
            return false;
        }
    }
}
