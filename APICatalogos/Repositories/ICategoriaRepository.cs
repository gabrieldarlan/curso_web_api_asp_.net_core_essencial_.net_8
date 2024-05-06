using APICatalogos.Models;
using APICatalogos.Pagination;

namespace APICatalogos.Repositories;

public interface ICategoriaRepository : IRepository<Categoria>
{
    PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
    PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParameters);


}
