using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormMvvm.Model;

namespace WinFormMvvm.ViewModel
{
    public class SubmitedUsersViewModel
    {

        private UserModel model = new UserModel();
        public BindingList<UserEntity> Users { get; private set; }
        public UserEntity CurrentUser { get; private set; }

        
        UserEntity _selectUser;
        /// <summary>
        /// 当前选择的用户，如果设置，则会设置当前用户
        /// </summary>
        public UserEntity SelectedUser {
            get { return _selectUser; }
            set {
                _selectUser = value;
                this.CurrentUser.ID = value.ID;
                this.CurrentUser.Name = value.Name;
            }
        
        }

        int _selectedUserID;
        public int SelectedUserID
        {
            get { return _selectedUserID; }
            set {
                _selectedUserID = value;
                var obj = this.Users.FirstOrDefault(p=>p.ID==value);
                if (obj != null)
                {
                    this.CurrentUser.ID = obj.ID;
                    this.CurrentUser.Name = obj.Name;
                    _selectUser = this.CurrentUser;
                }
                
            }
        }

        public SubmitedUsersViewModel()
        {
            var data = model.GetAllUsers();
            Users = new BindingList<UserEntity>(data);
            CurrentUser = new UserEntity();

        }

        public void UpdateUser()
        {
            var obj = this.Users.FirstOrDefault(p => p.ID == this.CurrentUser.ID);
            if (obj != null)
            {
                obj.Name = this.CurrentUser.Name;
                //更新后必须调用 ResetBindings 方法，否则控件上的数据会丢失一行
                this.Users.ResetBindings();

                model.UpdateUser(obj);
            }
            
        }
        public void UpdateUser(int id,string name)
        {
            var obj = this.Users.FirstOrDefault(p => p.ID == id);
            if (obj != null)
            {
                obj.Name = name;
                //更新后必须调用 ResetBindings 方法，否则控件上的数据会丢失一行
                this.Users.ResetBindings();

                model.UpdateUser(obj);
            }
        }

        public void SubmitUsers(UserEntity user)
        {
            //UserEntity newUser = new UserEntity();
            //newUser.ID = user.ID;
            //newUser.Name = user.Name;
            //Users.Add(newUser);
            if (!Users.Contains(user))
            {
                Users.Add(user);
                model.SubmitUser(user);            
            }
        }
        public void SubmitCurrentUsers()
        {
            UserEntity newUser = model.CreateNewUser(CurrentUser.Name);
            SubmitUsers(newUser);
            CurrentUser.ID = newUser.ID;
        }

        public void RemoveUser()
        {
            if (SelectedUser == null)
            {

                return;
            }
            var obj = this.Users.FirstOrDefault(p => p.ID == SelectedUser.ID);
            if (obj != null)
            {
                this.Users.Remove(obj);
                //更新后必须调用 ResetBindings 方法，否则控件上的数据会丢失一行
                this.Users.ResetBindings();

                model.RemoveUser(obj);
            }
        }
    }
}
