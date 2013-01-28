using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PermissionControlSystem;

namespace PermissionControlSystem
{
    public partial class PermissionSystem
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual bool AddPermission(IPermission permission)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 移除权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual bool RemovePermission(IPermission permission)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色</param>
        public virtual bool AddRole(Role role)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="role">角色</param>
        public virtual bool RemoveRole(Role role)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取访问者是否拥有权限
        /// </summary>
        /// <param name="visitor">访问者</param>
        public virtual IPermission GetPermission(IVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}
