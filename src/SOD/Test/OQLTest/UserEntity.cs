using PWMIS.DataMap.Entity;

namespace OQLTest
{
    internal class UserEntity : EntityBase
    {
        public UserEntity()
        {
            TableName = "User";
            //PrimaryKeys.Add("主键字段名");
            PrimaryKeys.Add("ID");
        }

        public int ID
        {
            get => getProperty<int>("Id");
            set => setProperty("Id", value);
        }

        public string Name
        {
            get => getProperty<string>("User Name");
            set => setProperty("User Name", value, 50);
        }

        public int Age
        {
            get => getProperty<int>("Age");
            set => setProperty("Age", value);
        }
    }
}