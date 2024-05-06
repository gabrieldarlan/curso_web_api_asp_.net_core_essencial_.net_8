using APICatalogos.Models;
using APICatalogos.Pagination;

namespace APICatalogos.Repositories;

public interface IProdutoRepository : IRepository<Produto>
{
    IEnumerable<Produto> GetProdutosPorCategoria(int id);

    //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParams);
    PagedList<Produto> GetProdutos(ProdutosParameters produtosParams);

    PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroParams);
}
