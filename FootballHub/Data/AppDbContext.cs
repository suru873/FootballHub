using Microsoft.EntityFrameworkCore;
using FootballHub.Models;

namespace FootballHub.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    DbContextOptions<AppDbContext> _options;
   

    public DbSet<Utente> Utenti { get; set; }
    public DbSet<SquadraPreferita> SquadrePreferite { get; set; }
    public DbSet<Pronostico> Pronostici { get; set; }
}
