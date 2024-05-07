using APICatalogos.Models;
using APICatalogos.Pagination;
using X.PagedList;

namespace APICatalogos.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
    Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParameters);


}
