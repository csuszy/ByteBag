using System;

namespace ByteBagWPF.Backend.GetUserClass
{
    public class GetUser//osztály a felhasználók adatainak reprezentálására.
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public DateTime registerDATE;

        public GetUser(DateTime regDate)
        {
            this.registerDATE = regDate;
        }

        public int admin { get; set; }
    }
}
