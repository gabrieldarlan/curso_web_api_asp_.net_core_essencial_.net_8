using APICatalogos.Dtos;
using APICatalogos.Dtos.Mappings;
using APICatalogos.Filters;
using APICatalogos.Models;
using APICatalogos.Pagination;
using APICatalogos.Repositories;
using APICatalogos.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger, IUnitOfWork uof)
        {

            _configuration = configuration;
            _logger = logger;
            _uof = uof;
        }

        [HttpGet("LerArquivoConfiguracao")]
        public string GetValores()
        {
            var valor1 = _configuration["chave1"];
            var valor2 = _configuration["chave2"];

            var secao1 = _configuration["secao1:chave2"];

            return $"Chave1 = {valor1} \nChave2 = {valor2} \nSecao => Chave2 = {secao1}";

        }

        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet("SemUsarFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoSemFromServices(IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {

            _logger.LogInformation("===========GET api/categorias ===================");

            var categorias = _uof.CategoriaRepository.GetAll();
            if (categorias is null)
            {
                return NotFound("Categorias não encontradas");
            }

            IEnumerable<CategoriaDTO> dtos = categorias.ToCategoriaDTOList();
            return base.Ok(dtos);

        }

        #region
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            _logger.LogInformation($"===========GET api/categorias/id = {id}============");
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogInformation("========= NOT FOUND ================");
                return NotFound($"Categoria com id={id} não encontrada");
            }

            return Ok(categoria.ToCategoriaDTO());
        }
        #endregion

        [HttpPost]
        public ActionResult<CategoriaDTO> Post([FromBody] CategoriaDTO categoria)
        {
            if (categoria is null)
                return BadRequest();

            Categoria categoriaCriada = _uof.CategoriaRepository.Create(categoria.ToCategoria());
            _uof.Commit();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoriaCriada.ToCategoriaDTO());
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            Categoria categoriaCriada = _uof.CategoriaRepository.Update(categoria.ToCategoria());
            _uof.Commit();

            return Ok(categoriaCriada);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não localizada");
            }
            _uof.CategoriaRepository.Delete(categoria);

            return Ok(categoria.ToCategoriaDTO());
        }

        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            PagedList<Categoria> categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);

            return ObterCategorias(categorias);

        }

        [HttpGet("filter/nome/pagination")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas([FromQuery] CategoriasFiltroNome categoriasFiltroNome)
        {
            PagedList<Categoria> categoriasFiltradas = _uof.CategoriaRepository.GetCategoriasFiltroNome(categoriasFiltroNome);
            return ObterCategorias(categoriasFiltradas);
        }


        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };

            Response.Headers.Append("X-pagination", JsonConvert.SerializeObject(metadata));

            var categoriasDto = categorias.ToCategoriaDTOList();
            return Ok(categoriasDto);
        }
    }
}
