using System;

namespace ByteBagWPF.Backend.GetPost
{
    public class GetPosts
    {
        public int posztID { get; set; }

        public int userID { get; set; }
        public string userName { get; set; }
        public string title { get; set; }
        public DateTime postDate;

        public GetPosts(DateTime regDate)
        {
            this.postDate = regDate;
        }

        public string post { get; set; }
    }
}
