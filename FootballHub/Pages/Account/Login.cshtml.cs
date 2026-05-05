using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballHub.Data;
using System.Security.Cryptography;
using System.Text;

namespace FootballHub.Pages.Account;

public class LoginModel : PageModel
{
    private readonly AppDbContext _db;
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string Errore { get; set; } = string.Empty;

    public LoginModel(AppDbContext db) { _db = db; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Password)));
        var utente = await _db.Utenti.FirstOrDefaultAsync(u => u.Username == Username && u.PasswordHash == hash);

        if (utente == null) { Errore = "Username o password non corretti."; return Page(); }

        HttpContext.Session.SetString("Username", utente.Username);
        HttpContext.Session.SetInt32("UtenteId", utente.Id);

        return RedirectToPage("/Index");
    }
}