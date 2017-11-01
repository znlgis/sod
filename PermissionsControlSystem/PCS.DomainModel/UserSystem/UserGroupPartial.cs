using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserSystem;

namespace UserSystem
{
    public abstract partial class UserGroup 
    {
        public UserGroup()
        {
            this.ChildGroups = new List<UserGroup>();
            this.ChildUsers = new List<User>();
        }
        /// <summary>
        /// 增加子用户
        /// </summary>
        /// <param name="user">用户</param>
        public virtual void AddUser(User user)
        {
            ((List<User>)this.ChildUsers).Add(user);
        }

        /// <summary>
        /// 增加子用户组
        /// </summary>
        /// <param name="group">用户组</param>
        public virtual void AddChildGroup(UserGroup group)
        {
            ((List<UserGroup>)this.ChildGroups).Add(group);
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user">所属的用户</param>
        public virtual void RemoveUser(User user)
        {
             ((List<User>)this.ChildUsers).Remove(user);
        }

        /// <summary>
        /// 移除子用户组
        /// </summary>
        /// <param name="group">子用户组</param>
        public virtual void RemoveChildGroup(UserGroup group)
        {
            ((List<UserGroup>)this.ChildGroups).Remove(group);
        }

        public int ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool IsLeaf
        {
            get;
            set;
        }
    }
}
