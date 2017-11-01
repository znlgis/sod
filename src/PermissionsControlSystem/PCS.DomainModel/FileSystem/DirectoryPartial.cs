using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystem;

namespace FileSystem
{
    public partial class Directory 
    {
        /// <summary>
        /// 增加文件
        /// </summary>
        /// <param name="file">文件</param>
        public virtual bool AddFile(File file)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 增加目录
        /// </summary>
        /// <param name="dir">目录</param>
        public virtual bool AddDirector(Directory dir)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">目录名称</param>
        public Directory(string name)
        {
        }


        public bool IsDir
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime CreateTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime UpdateTime
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public UserSystem.User CreatedWithUser
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public UserSystem.User UpdatedWithUser
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Create()
        {
            throw new NotImplementedException();
        }

        public string Read()
        {
            throw new NotImplementedException();
        }

        public bool Update(string text)
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<PermissionControlSystem.IOperation> Operations
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
