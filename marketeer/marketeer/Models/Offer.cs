namespace marketeer.Models
{
    public class Offer
    {
        public int offerID { get; set; }

        public string item  { get; set; }

        public double amount { get; set; }

        public Boolean deleted { get; set; }
    }
}
