using PWMIS.DataMap.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormMvvm.Model
{
    public class UserModel
    {
        private static int index = 0;
        private LocalDbContext context;
        public UserModel()
        {
            context = new LocalDbContext();
        }

        public List<UserEntity> GetAllUsers()
        {
            return OQL.From<UserEntity>().ToList(context.CurrentDataBase);
        }
        public void UpdateUser(UserEntity user)
        {
            int count= context.Update<UserEntity>(user);
        }

        public void AddUsers(IList<UserEntity> users)
        {
            int count = context.AddList(users);
        }

        public void SubmitUser(UserEntity user)
        {
           int count = context.Add(user);
        }

        public void RemoveUser(UserEntity user)
        {
            int count = context.Remove(user);
        }

        public UserEntity CreateNewUser(string userName="NoName")
        {
            return new UserEntity()
            {
                 ID= ++index,
                 Name =userName
            };
        }
    }
}
