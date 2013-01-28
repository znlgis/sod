using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileSystem;

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

        public global::UserSystem.User CreatedWithUser
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

        public global::UserSystem.User UpdatedWithUser
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

        public bool Read()
        {
            throw new NotImplementedException();
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

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
