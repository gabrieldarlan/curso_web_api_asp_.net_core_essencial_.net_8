using APICatalogos.Context;
using APICatalogos.Models;
using APICatalogos.Pagination;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogos.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            IQueryable<Categoria> categoriasOrdernadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            //var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdernadas,
            //   categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var resultado = await categoriasOrdernadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = await GetAllAsync();



            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.ToLower().Contains(categoriasParameters.Nome.ToLower()));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(),
            //    categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas;

        }
    }
}
