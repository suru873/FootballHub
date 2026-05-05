using FootballHub.Data;
using HubCalcio.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. REGISTRAZIONE SERVIZI (Dopo aver creato il builder)
builder.Services.AddRazorPages();

// Configurazione DB SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Configurazione HttpClient con SSL bypass per SportmonksService
builder.Services.AddHttpClient<SportmonksService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
    });

// Configurazione Sessione
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 2. COSTRUZIONE DELL'APP
var app = builder.Build();

// 3. CONFIGURAZIONE MIDDLEWARE (L'ordine è fondamentale!)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Consigliato
app.UseStaticFiles();
app.UseRouting();

// UseSession deve essere dopo UseRouting e prima di MapRazorPages
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();

app.Run();