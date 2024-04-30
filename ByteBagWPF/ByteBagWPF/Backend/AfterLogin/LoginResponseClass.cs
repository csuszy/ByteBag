namespace ByteBagWPF.Backend.AfterLogin
{
    public class LoginResponseClass
    {
        public string hitelesitve { get; set; }
        public int userID { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
        public LoginResponseClass(string responds)
        {
            string[] tmp = responds.Split(';');
            if (tmp.Length >= 1)
            {
                this.hitelesitve = tmp[0];
            }
            else
            {
                this.hitelesitve = string.Empty;
            }
            if (tmp.Length >= 2 && int.TryParse(tmp[1], out int userId))
            {
                this.userID = userId;
            }
            else
            {
                this.userID = 0;
            }

            if (tmp.Length >= 3)
            {
                this.userName = tmp[2];
            }
            else
            {
                this.userName = string.Empty;
            }
            if (tmp.Length >= 4)
            {
                this.Email = tmp[3];
            }
            else
            {
                this.Email = string.Empty;
            }

        }
    }
}
