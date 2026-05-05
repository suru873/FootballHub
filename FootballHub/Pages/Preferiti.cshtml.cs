using System.Text.Json;
using FootballHub.Data;
using FootballHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FootballHub.Pages;

public class PreferitiModel : PageModel
{
    private readonly AppDbContext _db;
    public List<SquadraPreferita> Preferiti { get; set; } = new();
    public string Messaggio { get; set; } = string.Empty;

    [BindProperty] public string NomeSquadra { get; set; } = string.Empty;
    [BindProperty] public string LogoUrl { get; set; } = string.Empty;
    [BindProperty] public string Campionato { get; set; } = string.Empty;
    [BindProperty] public int ApiId { get; set; }

    public PreferitiModel(AppDbContext db) { _db = db; }

    private async Task Carica()
    {
        int? uid = HttpContext.Session.GetInt32("UtenteId");
        if (uid.HasValue)
            Preferiti = await _db.SquadrePreferite.Where(s => s.UtenteId == uid.Value).ToListAsync();
    }

 
    public async Task OnGetAsync() => await Carica();

    public async Task<IActionResult> OnPostAggiungiAsync()
    {
        int? uid = HttpContext.Session.GetInt32("UtenteId");
        if (!uid.HasValue) return RedirectToPage("/Account/Login");

        bool esiste = await _db.SquadrePreferite.AnyAsync(s => s.UtenteId == uid.Value && s.NomeSquadra == NomeSquadra);
        if (!esiste)
        {
            _db.SquadrePreferite.Add(new SquadraPreferita
            {
                UtenteId = uid.Value,
                ApiSquadraId = ApiId,
                NomeSquadra = NomeSquadra,
                LogoUrl = LogoUrl,
                Campionato = Campionato
            });
            await _db.SaveChangesAsync();
            Messaggio = $"{NomeSquadra} aggiunta ai preferiti!";
        }
        else Messaggio = $"{NomeSquadra} e' gia' nei preferiti.";

        await Carica();
        return Page();
    }

    public async Task<IActionResult> OnPostRimuoviAsync(int id)
    {
        var s = await _db.SquadrePreferite.FindAsync(id);
        if (s != null) { _db.SquadrePreferite.Remove(s); await _db.SaveChangesAsync(); }
        return RedirectToPage();
    }
}
