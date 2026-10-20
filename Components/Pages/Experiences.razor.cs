using Microsoft.AspNetCore.Components;
using DensingMidterms.Models;

namespace DensingMidterms.Components.Pages
{
    public partial class Experiences : ComponentBase
    {
        protected string SearchQuery { get; set; } = "";
        protected string SelectedCategory { get; set; } = "All";
        protected string CurrentSort { get; set; } = "featured";
        protected TourExperience? SelectedTour { get; set; }
        protected int GuestCount { get; set; } = 1;
        protected bool IsBookingConfirmed { get; set; } = false;

        protected readonly List<string> Categories = new() { "All", "Cultural", "Adventure", "Relaxation", "Wildlife", "Cruise" };

        protected List<TourExperience> Tours = new()
        {
            new TourExperience
            {
                Id = 1,
                Title = "The Golden Route",
                Country = "Japan",
                Category = "Cultural",
                DurationDays = 10,
                Price = 2499,
                ImageClass = "bg-pkg-japan",
                Description = "Travel from the neon streets of Tokyo to the historic temples of Kyoto via bullet train.",
                Highlights = new() { "Bullet Train Pass Included", "Tea Ceremony in Kyoto", "Mt. Fuji Day Tour", "Guided Market Food Walks" }
            },
            new TourExperience
            {
                Id = 2,
                Title = "Alpine Explorer",
                Country = "Switzerland",
                Category = "Adventure",
                DurationDays = 7,
                Price = 3150,
                ImageClass = "bg-pkg-swiss",
                Description = "Hike through mountain valleys, ride the Glacier Express, and stay in premium alpine chalets.",
                Highlights = new() { "Glacier Express Scenic Train", "Matterhorn Cable Car", "Daily Guided Treks", "Traditional Swiss Fondue Evening" }
            },
            new TourExperience
            {
                Id = 3,
                Title = "Overwater Paradise",
                Country = "Maldives",
                Category = "Relaxation",
                DurationDays = 5,
                Price = 4200,
                ImageClass = "bg-pkg-maldives",
                Description = "Unwind in a private bungalow with direct ocean access to pristine coral reefs.",
                Highlights = new() { "Private Water Villa", "Sunset Dolphin Cruise", "Snorkeling Equipment Included", "All-Inclusive Dining" }
            },
            new TourExperience
            {
                Id = 4,
                Title = "Great Rift Safari",
                Country = "Kenya",
                Category = "Wildlife",
                DurationDays = 8,
                Price = 2890,
                ImageClass = "bg-pkg-kenya",
                Description = "Witness the Big Five across Masai Mara and Amboseli with expert naturalists.",
                Highlights = new() { "4x4 Open-Top Game Drives", "Luxury Tented Camp Stay", "Masai Village Cultural Tour", "Hot Air Balloon Flight" }
            },
            new TourExperience
            {
                Id = 5,
                Title = "Amalfi Coast Discovery",
                Country = "Italy",
                Category = "Cruise",
                DurationDays = 6,
                Price = 1980,
                ImageClass = "bg-pkg-amalfi",
                Description = "Sail past dramatic cliffs, lemon groves, and pastel villages along southern Italy.",
                Highlights = new() { "Private Yacht Day Excursion", "Limoncello Tasting", "Capri Island Grotto Tour", "Clifftop Hotel Accommodations" }
            },
            new TourExperience
            {
                Id = 6,
                Title = "Aurora Borealis Quest",
                Country = "Iceland",
                Category = "Adventure",
                DurationDays = 6,
                Price = 2250,
                ImageClass = "bg-pkg-iceland",
                Description = "Hunt for the Northern Lights, soak in geothermal lagoons, and traverse glacial caverns.",
                Highlights = new() { "Blue Lagoon Comfort Pass", "Superjeep Northern Lights Hunt", "Golden Circle Tour", "Glacier Cave Expedition" }
            }
        };

        protected List<TourExperience> FilteredTours
        {
            get
            {
                var items = Tours.AsEnumerable();

                if (SelectedCategory != "All")
                {
                    items = items.Where(t => t.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    items = items.Where(t => t.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                             t.Country.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                                             t.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase));
                }

                items = CurrentSort switch
                {
                    "price-low" => items.OrderBy(t => t.Price),
                    "price-high" => items.OrderByDescending(t => t.Price),
                    "duration" => items.OrderBy(t => t.DurationDays),
                    _ => items.OrderBy(t => t.Id)
                };

                return items.ToList();
            }
        }

        protected int WishlistCount => Tours.Count(t => t.IsFavorite);

        protected void SelectCategory(string category) => SelectedCategory = category;

        protected void OnSortChanged(ChangeEventArgs e) => CurrentSort = e.Value?.ToString() ?? "featured";

        protected void ToggleWishlist(TourExperience tour) => tour.IsFavorite = !tour.IsFavorite;

        protected void ResetFilters()
        {
            SearchQuery = "";
            SelectedCategory = "All";
            CurrentSort = "featured";
        }

        protected void OpenModal(TourExperience tour)
        {
            SelectedTour = tour;
            GuestCount = 1;
            IsBookingConfirmed = false;
        }

        protected void CloseModal() => SelectedTour = null;

        protected void ConfirmBooking() => IsBookingConfirmed = true;
    }
}