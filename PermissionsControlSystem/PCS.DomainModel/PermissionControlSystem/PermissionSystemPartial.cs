using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PermissionControlSystem;

namespace PermissionControlSystem
{
    public partial class PermissionSystem
    {
        public PermissionSystem()
        {
            this.Permissions = new List<IPermission>();
            this.Roles = new List<Role>();
            this.IsEquityModel = true;
        }

        public PermissionSystem(string systemName):this()
        {
            //可以考虑从持久化媒体加载指定名称的权限系统，初始化Permissions、Roles、IsEquityModel
            this.SystemName = systemName;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual bool AddPermission(IPermission permission)
        {
            ((List<IPermission>)this.Permissions).Add(permission);
            return true;
        }

        /// <summary>
        /// 移除权限
        /// </summary>
        /// <param name="permission">权限</param>
        public virtual bool RemovePermission(IPermission permission)
        {
            return ((List<IPermission>)this.Permissions).Remove(permission);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色</param>
        public virtual bool AddRole(Role role)
        {
            ((List<Role>)this.Roles).Add(role);
            return true;
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="role">角色</param>
        public virtual bool RemoveRole(Role role)
        {
            return ((List<Role>)this.Roles).Remove(role);
        }

        /// <summary>
        /// 获取访问者拥有的权限
        /// </summary>
        /// <param name="visitor">访问者</param>
        public virtual IEnumerable<IPermission> GetPermission(IVisitor visitor)
        {
            var result= this.Permissions.Where(p => p.Visitors.Contains(visitor) && p.IsOwner);
            foreach (Role r in this.Roles)
            {
                if (r.Visitors.Contains(visitor))
                    result.Concat( r.Permissions);
            }
            return result;
        }
    }
}
