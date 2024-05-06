using APICatalogos.Dtos;
using APICatalogos.Models;
using APICatalogos.Pagination;
using APICatalogos.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ProdutosController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }


    [HttpGet]
    public ActionResult<IEnumerable<ProdutoDTO>> Get()
    {
        List<Produto> produtos = _uof.ProdutoRepository.GetAll().ToList();
        if (produtos is null)
        {
            return NotFound("Produtos não encontrados");
        }
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
    public ActionResult<ProdutoDTO> Get(int id)
    {
        var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound("Produto não encontrado");

        var produtosDto = _mapper.Map<ProdutoDTO>(produto);
        return Ok(produtosDto);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> Post([FromBody] ProdutoDTO produtoDto)
    {
        if (produtoDto is null)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);
        Produto novoProduto = _uof.ProdutoRepository.Create(produto);
        _uof.Commit();
        var novoProdutoDto = _mapper.Map<ProdutoDTO>(novoProduto);
        return new CreatedAtRouteResult("ObterProduto", new { id = novoProdutoDto.ProdutoId }, novoProdutoDto);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
    {
        if (id != produtoDto.ProdutoId)
            return BadRequest();

        var produto = _mapper.Map<Produto>(produtoDto);

        Produto produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);
        return Ok(produtoAtualizadoDto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> Delete(int id)
    {
        var produtoRemovido = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produtoRemovido is null)
            return NotFound("Produto não localizado");

        _uof.ProdutoRepository.Delete(produtoRemovido);
        _uof.Commit();

        var produtoRemovidoDto = _mapper.Map<ProdutoDTO>(produtoRemovido);

        return Ok(produtoRemovidoDto);

    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        IEnumerable<Produto> produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
            return NotFound();

        //var destino = _mapper.Map<Destino>(origem);   
        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id,
        JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
            return BadRequest();

        var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

        if (produto is null)
            return NotFound();

        ProdutoDTOUpdateRequest produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        _mapper.Map(produtoUpdateRequest, produto);

        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

        return ObterProdutos(produtos);
    }

    [HttpGet("filter/preco/paginations")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoFilterPreco([FromQuery] ProdutosFiltroPreco produtosFiltroParameters)
    {
        var produtos = _uof.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltroParameters);

        return ObterProdutos(produtos);
    }
    private ActionResult<IEnumerable<ProdutoDTO>> ObterProdutos(PagedList<Produto> produtos)
    {
        var metadata = new
        {
            produtos.TotalCount,
            produtos.PageSize,
            produtos.CurrentPage,
            produtos.TotalPages,
            produtos.HasNext,
            produtos.HasPrevious,
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
        return Ok(produtosDto);
    }



}
