using System;
using PWMIS.DataMap.Entity;

namespace ConsoleAppTest;

internal class SimpleEntity : EntityBase
{
    public SimpleEntity()
    {
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

    public DateTime AtTime
    {
        get => getProperty<DateTime>(nameof(AtTime));
        set => setProperty(nameof(AtTime), value);
    }
}