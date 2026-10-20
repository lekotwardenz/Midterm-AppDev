namespace DensingMidterms.Models
{
    public class BookingDraft
    {
        public int TourId { get; set; }
        public string TourTitle { get; set; } = string.Empty;
        public int GuestCount { get; set; } = 1;
        public decimal TotalPrice { get; set; }
        public DateTime DraftCreated { get; set; } = DateTime.Now;
    }
}