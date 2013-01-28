using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserSystem;

namespace UserSystem
{
    public abstract partial class UserGroup 
    {
        /// <summary>
        /// 增加子用户
        /// </summary>
        /// <param name="user">用户</param>
        public virtual void AddUser(User user)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 增加子用户组
        /// </summary>
        /// <param name="group">用户组</param>
        public virtual void AddChildGroup(UserGroup group)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user">所属的用户</param>
        public virtual void RemoveUser(User user)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 移除子用户组
        /// </summary>
        /// <param name="group">子用户组</param>
        public virtual void RemoveChildGrooup(UserGroup group)
        {
            throw new System.NotImplementedException();
        }

        public int ID
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

        public string Name
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

        public bool IsLeaf
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
