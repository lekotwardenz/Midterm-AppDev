using Microsoft.AspNetCore.Components;

namespace DensingMidterms.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private string searchDestination = string.Empty;
        private string searchCheckIn = string.Empty;
        private string searchCheckOut = string.Empty;
        private int? searchGuests;

        private void SearchAndBook()
        {
            var guestsParam = searchGuests.HasValue && searchGuests > 0 ? $"&guests={searchGuests.Value}" : "";
            var destParam = !string.IsNullOrWhiteSpace(searchDestination)
                ? $"?search={Uri.EscapeDataString(searchDestination)}{guestsParam}"
                : (guestsParam.Length > 0 ? $"?{guestsParam.TrimStart('&')}" : "");
            Navigation.NavigateTo($"/booking{destParam}");
        }

        private void GoToBooking(long tourId) => Navigation.NavigateTo($"/booking?tourId={tourId}");
    }
}