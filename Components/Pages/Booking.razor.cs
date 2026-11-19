using Microsoft.AspNetCore.Components;
using DensingMidterms.Models;
using static Supabase.Postgrest.Constants;

namespace DensingMidterms.Components.Pages
{
    public partial class Booking : ComponentBase
    {
        [Inject] private Supabase.Client Db { get; set; } = default!;

        [SupplyParameterFromQuery] public long? TourId { get; set; }
        [SupplyParameterFromQuery] public string? Search { get; set; }
        [SupplyParameterFromQuery] public int? Guests { get; set; }

        private string selectedCategory = "All";
        private string searchQuery = string.Empty;
        private bool onlyFavorites = false;
        private bool savingDraft = false;

        private ContactMessage contactDraft = new();
        private bool messageSent = false;
        private bool savingContact = false;

        private readonly List<string> Categories = new() { "All", "Alpine", "Cultural", "Safari", "Coastal" };

        private List<TourExperience> availableTours = new();
        private BookingDraft currentDraft = new();
        private List<BookingDraft> savedDrafts = new();

        private TourExperience? SelectedTour => availableTours.FirstOrDefault(t => t.Id == currentDraft.TourId);

        private IEnumerable<TourExperience> FilteredTours => availableTours.Where(t =>
            (selectedCategory == "All" || t.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)) &&
            (!onlyFavorites || t.IsFavorite) &&
            (string.IsNullOrWhiteSpace(searchQuery) ||
             t.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
             t.Country.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
        );

        protected override async Task OnInitializedAsync()
        {
            await LoadTours();
            await LoadDrafts();

            if (availableTours.Any())
            {
                SelectTour(availableTours.First().Id);
                ApplyQueryParameters();
            }
        }

        protected override void OnParametersSet()
        {
            if (availableTours.Any())
            {
                ApplyQueryParameters();
            }
        }

        private void ApplyQueryParameters()
        {
            if (!string.IsNullOrWhiteSpace(Search))
            {
                searchQuery = Search;
                var matched = availableTours.FirstOrDefault(t =>
                    t.Country.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                    t.Title.Contains(Search, StringComparison.OrdinalIgnoreCase));
                if (matched != null) SelectTour(matched.Id);
            }

            if (TourId.HasValue && availableTours.Any(t => t.Id == TourId.Value))
            {
                SelectTour(TourId.Value);
            }

            if (Guests.HasValue && Guests.Value > 0)
            {
                currentDraft.GuestCount = Guests.Value;
                RecalculateTotal();
            }
        }

        private async Task LoadTours()
        {
            var res = await Db.From<TourExperience>()
                .Order(t => t.Id, Ordering.Ascending)
                .Range(0, 49)
                .Get();

            availableTours = res.Models;
        }

        private async Task LoadDrafts()
        {
            var res = await Db.From<BookingDraft>()
                .Where(d => d.DeletedAt == null)
                .Order(d => d.Id, Ordering.Descending)
                .Range(0, 49)
                .Get();

            savedDrafts = res.Models;
        }

        private async Task ToggleFavorite(TourExperience tour)
        {
            var nextState = !tour.IsFavorite;
            await Db.From<TourExperience>()
                .Where(t => t.Id == tour.Id)
                .Set(t => t.IsFavorite, nextState)
                .Update();

            tour.IsFavorite = nextState;
        }

        private async Task SaveBookingDraft()
        {
            if (savingDraft || SelectedTour == null) return;
            savingDraft = true;

            await Db.From<BookingDraft>().Insert(new BookingDraft
            {
                TourId = currentDraft.TourId,
                TourTitle = currentDraft.TourTitle,
                GuestCount = currentDraft.GuestCount,
                TotalPrice = currentDraft.TotalPrice,
                DraftCreated = DateTime.UtcNow
            });

            savingDraft = false;
            await LoadDrafts();
        }

        private async Task IncrementSavedDraftGuests(BookingDraft draft)
        {
            var tour = availableTours.FirstOrDefault(t => t.Id == draft.TourId);
            var unitPrice = tour?.Price ?? (draft.TotalPrice / Math.Max(1, draft.GuestCount));
            var newCount = draft.GuestCount + 1;
            var newTotal = unitPrice * newCount;

            await Db.From<BookingDraft>()
                .Where(d => d.Id == draft.Id)
                .Set(d => d.GuestCount, newCount)
                .Set(d => d.TotalPrice, newTotal)
                .Update();

            await LoadDrafts();
        }

        private async Task RemoveDraft(BookingDraft draft)
        {
            await Db.From<BookingDraft>()
                .Where(d => d.Id == draft.Id)
                .Set(d => d.DeletedAt, DateTime.UtcNow)
                .Update();

            await LoadDrafts();
        }

        private async Task HandleQuickContact()
        {
            if (savingContact) return;
            savingContact = true;

            contactDraft.SubmittedAt = DateTime.UtcNow;
            await Db.From<ContactMessage>().Insert(contactDraft);

            savingContact = false;
            messageSent = true;
        }

        private void ResetContact()
        {
            contactDraft = new ContactMessage();
            messageSent = false;
        }

        private void ResetFilters()
        {
            selectedCategory = "All";
            searchQuery = string.Empty;
            onlyFavorites = false;
        }

        private void OnTourChanged(ChangeEventArgs e)
        {
            if (long.TryParse(e.Value?.ToString(), out long id)) SelectTour(id);
        }

        private void SelectTour(long id)
        {
            var tour = availableTours.FirstOrDefault(t => t.Id == id);
            if (tour == null) return;

            currentDraft.TourId = tour.Id;
            currentDraft.TourTitle = tour.Title;
            if (currentDraft.GuestCount < 1) currentDraft.GuestCount = 1;
            RecalculateTotal();
        }

        private void IncrementGuests()
        {
            if (currentDraft.GuestCount < 20)
            {
                currentDraft.GuestCount++;
                RecalculateTotal();
            }
        }

        private void DecrementGuests()
        {
            if (currentDraft.GuestCount > 1)
            {
                currentDraft.GuestCount--;
                RecalculateTotal();
            }
        }

        private void RecalculateTotal()
        {
            var price = SelectedTour?.Price ?? 0m;
            currentDraft.TotalPrice = price * currentDraft.GuestCount;
        }
    }
}