using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DensingMidterms.Models
{
    [Table("tour_experiences")]
    public class TourExperience : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("country")]
        public string Country { get; set; } = string.Empty;

        [Column("category")]
        public string Category { get; set; } = string.Empty;

        [Column("duration_days")]
        public int DurationDays { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("image_class")]
        public string ImageClass { get; set; } = string.Empty;

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("highlights")]
        public List<string> Highlights { get; set; } = new();

        [Column("is_favorite")]
        public bool IsFavorite { get; set; }
    }
}