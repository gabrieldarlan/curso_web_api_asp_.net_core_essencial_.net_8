using APICatalogos.Context;
using APICatalogos.Models;
using APICatalogos.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogos.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = GetAll().OrderBy(c => c.CategoriaId).AsQueryable();

            var categoriaOrdernadas = PagedList<Categoria>.ToPagedList(categorias,
               categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriaOrdernadas;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.ToLower().Contains(categoriasParameters.Nome.ToLower()));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias,
                categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas;

        }
    }
}
