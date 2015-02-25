using System;
namespace EntityTest
{
    public interface IUser
    {
        int Age { get; set; }
        string FirstName { get; set; }
        string LasttName { get; set; }
        int UserID { get; set; }
    }
}
