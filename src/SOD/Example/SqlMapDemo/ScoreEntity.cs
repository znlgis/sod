using PWMIS.Common;
using PWMIS.DataMap.Entity;

namespace SqlMapDemo
{
    public class ScoreEntity : EntityBase
    {
        public ScoreEntity()
        {
            TableName = "Score";
            EntityMap = EntityMapType.Table;
            PrimaryKeys.Add("stuID");
            IdentityName = "stuID";
        }

        public int StudentID
        {
            get => getProperty<int>("stuID");
            set => setProperty("stuID", value);
        }

        public string CategoryName
        {
            get => getProperty<string>("category");
            set => setProperty("category", value, 50);
        }

        public int Score
        {
            get => getProperty<int>("score");
            set => setProperty("score", value);
        }
    }
}