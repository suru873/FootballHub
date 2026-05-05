using Microsoft.AspNetCore.Mvc.RazorPages;
using FootballHub.Services;
using System.Text.Json;

namespace FootballHub.Pages;


public class ClassificaModel : PageModel
{
    private readonly SportmonksService _api;
    public JsonElement Classifica { get; set; }
    public string Errore { get; set; } = string.Empty;

    public ClassificaModel(SportmonksService api) { _api = api; }

    public async Task OnGetAsync()
    {
        try { Classifica = await _api.GetClassificaAsync(); }
        catch (Exception ex) { Errore = $"Errore: {ex.Message} | {ex.InnerException?.Message}"; }
    }
}
