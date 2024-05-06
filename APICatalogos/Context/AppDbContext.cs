using APICatalogos.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogos.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }

}
