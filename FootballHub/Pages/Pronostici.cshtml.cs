using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballHub.Data;
using FootballHub.Models;
using FootballHub.Services;
using System.Text.Json;

namespace FootballHub.Pages;

public class PronosticiModel : PageModel
{
    private readonly AppDbContext _db;
    private readonly SportmonksService _api;

    public List<Pronostico> ListaPronostici { get; set; } = new();
    public JsonElement PartiteConQuote { get; set; }
    public string Messaggio { get; set; } = string.Empty;
    public string Errore { get; set; } = string.Empty;

    [BindProperty] public string SquadraCasa { get; set; } = string.Empty;
    [BindProperty] public string SquadraOspite { get; set; } = string.Empty;
    [BindProperty] public string Risultato { get; set; } = string.Empty;
    [BindProperty] public DateTime DataPartita { get; set; } = DateTime.Today;

    public PronosticiModel(AppDbContext db, SportmonksService api) { _db = db; _api = api; }

    public async Task OnGetAsync()
    {
        int? uid = HttpContext.Session.GetInt32("UtenteId");
        if (uid.HasValue)
            ListaPronostici = await _db.Pronostici
                .Where(p => p.UtenteId == uid.Value)
                .OrderByDescending(p => p.DataInserimento)
                .ToListAsync();

        try { PartiteConQuote = await _api.GetPronosticiApiAsync(); }
        catch (Exception ex) { Errore = ex.Message; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        int? uid = HttpContext.Session.GetInt32("UtenteId");
        if (!uid.HasValue) return RedirectToPage("/Account/Login");

        if (!string.IsNullOrWhiteSpace(SquadraCasa) && !string.IsNullOrWhiteSpace(SquadraOspite) && !string.IsNullOrWhiteSpace(Risultato))
        {
            _db.Pronostici.Add(new Pronostico
            {
                UtenteId = uid.Value,
                SquadraCasa = SquadraCasa,
                SquadraOspite = SquadraOspite,
                RisultatoPronosticato = Risultato,
                DataPartita = DataPartita
            });
            await _db.SaveChangesAsync();
            Messaggio = "Pronostico salvato!";
        }
        await OnGetAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostEliminaAsync(int id)
    {
        var p = await _db.Pronostici.FindAsync(id);
        if (p != null) { _db.Pronostici.Remove(p); await _db.SaveChangesAsync(); }
        return RedirectToPage();
    }
}
