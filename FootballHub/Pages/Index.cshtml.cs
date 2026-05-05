using Microsoft.AspNetCore.Mvc.RazorPages;
using FootballHub.Services;
using System.Text.Json;

namespace FootballHub.Pages;

public class IndexModel : PageModel
{
    private readonly SportmonksService _api;
    public JsonElement Livescores { get; set; }
    public string Errore { get; set; } = string.Empty;

    public IndexModel(SportmonksService api) { _api = api; }

    public async Task OnGetAsync()
    {
        try { Livescores = await _api.GetLivescoresAsync(); }
        catch (Exception ex) { Errore = $"Errore: {ex.Message} | {ex.InnerException?.Message}"; }
    }
}
