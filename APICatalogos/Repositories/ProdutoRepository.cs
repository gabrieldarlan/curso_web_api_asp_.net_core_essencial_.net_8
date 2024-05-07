using APICatalogos.Context;
using APICatalogos.Models;
using APICatalogos.Pagination;
using X.PagedList;

namespace APICatalogos.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        private const string igual = "igual";
        private const string maior = "maior";
        private const string menor = "menor";

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParams)
        {
            var produtos = await GetAllAsync();
            IQueryable<Produto> produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
            var resultado = await produtosOrdenados.ToPagedListAsync(produtosParams.PageNumber, produtosParams.PageSize);
            return resultado;
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = await GetAllAsync();

            if (produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if (produtosFiltroParams.PrecoCriterio.Equals(maior, StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals(menor, StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroParams.PrecoCriterio.Equals(igual, StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(p => p.Preco);
                }
            }

            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber,
                produtosFiltroParams.PageSize);

            return produtosFiltrados;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();

            IEnumerable<Produto> produtosCategoria = produtos.Where(c => c.CategoriaId == id);
            return produtosCategoria;
        }
    }
}
