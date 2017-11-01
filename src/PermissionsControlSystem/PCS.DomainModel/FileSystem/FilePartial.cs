using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystem;
using PermissionControlSystem;
using PCS.DomainModel.FileSystem;

namespace FileSystem
{
    public partial class File 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">文件名</param>
        public File(string name)
        {
            this.Name = name;
            this.Operations = new List<FileOperation>() {
            new FileCreate(),
            new FileOperation("Read"),
            new FileOperation("Update"),
            new FileOperation("Delete")
            };
        }

        public bool IsDir
        {
            get;
            set;
        }

        public DateTime CreateTime
        {
            get;
            set;
        }

        public DateTime UpdateTime
        {
            get;
            set;
        }

        public global::UserSystem.User CreatedWithUser
        {
            get;
            set;
        }

        public global::UserSystem.User UpdatedWithUser
        {
            get;
            set;
        }
        /// <summary>
        /// 创建新文件
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            FileOperation operation = this.Operations.First(p => p.OperationName == "Create") as FileOperation;
            object result= operation.Execute();
            return (bool)result;
        }
        /// <summary>
        /// 是否允许读取文件
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 是否允许修改文件
        /// </summary>
        /// <returns></returns>
        public bool Update(string text)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 是否允许删除文件
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            throw new NotImplementedException();
        }


        public string Name
        {
            get;
            private set;
        }

        public IEnumerable<global::PermissionControlSystem.IOperation> Operations
        {
            get;
            set;
        }

    }
}
