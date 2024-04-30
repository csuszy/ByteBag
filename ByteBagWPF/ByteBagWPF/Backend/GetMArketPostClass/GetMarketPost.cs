using System;

namespace ByteBagWPF.Backend.GetMArketPostClass
{
    class GetMarketPost
    {
        public int marketID { get; set; }
        public int userID { get; set; }
        public string username { get; set; }
        public string marketpost { get; set; }
        public string title { get; set; }
        public double price { get; set; }

        public DateTime marketDATE;
        public GetMarketPost(DateTime postedTime)
        {
            this.marketDATE = postedTime;
        }

    }
}
