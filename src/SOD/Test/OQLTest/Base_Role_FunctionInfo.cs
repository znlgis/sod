using PWMIS.DataMap.Entity;

namespace UPMS.Core.Model
{
    /// <summary>
    ///     瀹炰綋鏄犲皠锛氳鑹插姛鑳藉叧绯昏〃
    /// </summary>
    public class Base_Role_FunctionInfo : EntityBase
    {
        public Base_Role_FunctionInfo()
        {
            TableName = "Base_Role_Function";
            PrimaryKeys.Add("Id");
        }

        /// <summary>
        ///     缂栧彿
        /// </summary>
        public string Id
        {
            get => getProperty<string>("Id");
            set => setProperty("Id", value);
        }

        /// <summary>
        ///     瑙掕壊缂栧彿
        /// </summary>
        public int RoleId
        {
            get => getProperty<int>("RoleId");
            set => setProperty("RoleId", value);
        }

        /// <summary>
        ///     鍔熻兘缂栧彿
        /// </summary>
        public string FunctionId
        {
            get => getProperty<string>("FunctionId");
            set => setProperty("FunctionId", value);
        }

        protected override void SetFieldNames()
        {
            PropertyNames = new[] { "Id", "RoleId", "FunctionId" };
        }
    }
}