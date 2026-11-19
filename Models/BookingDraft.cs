using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DensingMidterms.Models
{
    [Table("booking_drafts")]
    public class BookingDraft : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("tour_id")]
        public long TourId { get; set; }

        [Column("tour_title")]
        public string TourTitle { get; set; } = string.Empty;

        [Column("guest_count")]
        public int GuestCount { get; set; } = 1;

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("draft_created")]
        public DateTime DraftCreated { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}