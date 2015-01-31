using System;
namespace ConsoleTest
{
    public interface ITable_User
    {
        DateTime Birthday { get; set; }
        float Height { get; set; }
        string Name { get; set; }
        bool Sex { get; set; }
        int UID { get; set; }
    }
}
