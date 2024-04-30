using System;

namespace ByteBagWPF.Backend.GetAdminClass
{
    public class GetAdmin//osztály a felhasználók adatainak reprezentálására.
    {
        public int userID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public DateTime registerDATE;

        public GetAdmin(DateTime regDate)
        {
            this.registerDATE = regDate;
        }

        public int adminE { get; set; }
    }
}
