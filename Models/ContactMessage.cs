using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DensingMidterms.Models
{
    [Table("contact_messages")]
    public class ContactMessage : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("destination")]
        public string Destination { get; set; } = string.Empty;

        [Column("submitted_at")]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}