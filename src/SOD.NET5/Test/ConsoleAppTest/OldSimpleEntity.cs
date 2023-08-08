using PWMIS.DataMap.Entity;

namespace ConsoleAppTest;

internal class OldSimpleEntity : EntityBase
{
    public OldSimpleEntity()
    {
        Meta = new EntityMetaData();
        TableName = "Table_1";
        IdentityName = "ID";
        PrimaryKeys.Add("ID");
    }


    public int ID
    {
        get => getProperty<int>("ID");
        set => setProperty("ID", value);
    }

    public string Name
    {
        get => getProperty<string>(nameof(Name));
        set => setProperty(nameof(Name), value, 100);
    }
}