using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootballHub.Services;
using System.Text.Json;

namespace FootballHub.Pages;

public class PartiteModel : PageModel
{
    private readonly SportmonksService _api;
    public JsonElement Partite { get; set; }
    public string Errore { get; set; } = string.Empty;
    public string DataSelezionata { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");

    public PartiteModel(SportmonksService api) { _api = api; }

    public async Task OnGetAsync(string? data)
    {
        if (!string.IsNullOrEmpty(data)) DataSelezionata = data;
        try { Partite = await _api.GetPartitePerDataAsync(DataSelezionata); }
        catch (Exception ex) { Errore = $"Errore: {ex.Message} | {ex.InnerException?.Message}"; }
    }
}
