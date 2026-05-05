using Microsoft.AspNetCore.Mvc.RazorPages;
using FootballHub.Services;
using System.Text.Json;

namespace FootballHub.Pages;

public class IndexModel : PageModel
{
    private readonly SportmonksService _api;
    public JsonElement Livescores { get; set; }
    public JsonElement PartiteOggi { get; set; }
    public string Errore { get; set; } = string.Empty;

    public IndexModel(SportmonksService api) { _api = api; }

    public async Task OnGetAsync()
    {
        try
        {
            Livescores = await _api.GetLivescoresAsync();
            if (!Livescores.TryGetProperty("data", out var d) || d.GetArrayLength() == 0)
                PartiteOggi = await _api.GetPartiteOggiAsync();
        }
        catch (Exception ex) { Errore = $"Errore: {ex.Message} | {ex.InnerException?.Message}"; }
    }
}