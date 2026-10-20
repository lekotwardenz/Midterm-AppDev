namespace DensingMidterms.Models
{
    public class TourExperience
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public string ImageClass { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Highlights { get; set; } = new();
        public bool IsFavorite { get; set; }
    }
}