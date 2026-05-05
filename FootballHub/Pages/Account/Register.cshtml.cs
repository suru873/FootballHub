using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FootballHub.Data;
using FootballHub.Models;
using System.Security.Cryptography;
using System.Text;

namespace FootballHub.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly AppDbContext _db;
    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Email { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    [BindProperty] public string ConfermaPassword { get; set; } = string.Empty;
    public string Errore { get; set; } = string.Empty;

    public RegisterModel(AppDbContext db) { _db = db; }
    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Password != ConfermaPassword) { Errore = "Le password non coincidono."; return Page(); }
        if (await _db.Utenti.AnyAsync(u => u.Username == Username || u.Email == Email))
        { Errore = "Username o email gia' in uso."; return Page(); }

        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Password)));
        _db.Utenti.Add(new Utente { Username = Username, Email = Email, PasswordHash = hash });
        await _db.SaveChangesAsync();
        return RedirectToPage("/Account/Login");
    }
}
