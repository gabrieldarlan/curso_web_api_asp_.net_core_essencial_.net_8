using APICatalogos.Models;
using APICatalogos.Pagination;
using X.PagedList;

namespace APICatalogos.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
    Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams);
    Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
}
