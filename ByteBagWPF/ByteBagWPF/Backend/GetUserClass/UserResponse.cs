using System.Collections.Generic;

namespace ByteBagWPF.Backend.GetUserClass
{
    class UserResponse//a beérkező JSON választ reprezentálja, és tartalmaz egy listát a felhasználókról.
    {
        public List<GetUser> Users { get; set; }
    }//mondom te gering csz

}
