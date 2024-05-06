using APICatalogos.Models;
using Humanizer;

namespace APICatalogos.Dtos.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if (categoria == null)
                return null;

            return new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            };
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDto)
        {

            if (categoriaDto == null)
                return null;

            return new Categoria
            {
                CategoriaId = categoriaDto.CategoriaId,
                Nome = categoriaDto.Nome,
                ImagemUrl = categoriaDto.ImagemUrl,
            };
        }


        public static IEnumerable<CategoriaDTO> ToCategoriaDTOList(this IEnumerable<Categoria> categorias)
        {
            if ((categorias is null) || (categorias.Count() < 0))
            {
                return new List<CategoriaDTO>();
            }

            return categorias.Select(categoria => new CategoriaDTO
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.ImagemUrl,
            }).ToList();
        }
    }
}
