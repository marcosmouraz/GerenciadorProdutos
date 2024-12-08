using AutoMapper;
using GerenciadorProdutos.Data;
using GerenciadorProdutos.Data.Dtos.Produto;
using GerenciadorProdutos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorProdutos.Controllers;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private UnifiedDbContext _context;
    private IMapper _mapper;

    public ProdutoController(UnifiedDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionarProduto([FromBody] CreateProdutoDto produtoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool categoriaExiste = _context.Categorias.Any(c => c.Id == produtoDto.CategoriaId);
        if (!categoriaExiste)
        {
            return BadRequest(new { Message = "CategoriaId inválido. A categoria especificada não existe." });
        }

        Produto produto = _mapper.Map<Produto>(produtoDto);
        produto.Status = (StatusProduto)(produto.QtdEstoque == 0 ? 0 : 1);
        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return CreatedAtAction(nameof(RecuperaProdutosPorId), new { Id = produto.Id }, produtoDto);
    }

    [HttpGet]
    public IEnumerable<ReadProdutoDto> ListarProdutos([FromQuery] int? categoriaId, [FromQuery] string? nome, [FromQuery] string? status)
    {
        var query = _context.Produtos.AsQueryable();

        if (categoriaId != null)
        {
            query = query.Where(produto => produto.Categoria.Id == categoriaId);
        }

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(produto => produto.Nome.Contains(nome));
        }

        if (!string.IsNullOrEmpty(status))
        {
            if (Enum.TryParse(status, out StatusProduto statusEnum))
            {
                query = query.Where(produto => produto.Status == statusEnum);
            }
            else
            {
                return new List<ReadProdutoDto>();
            }
        }

        return _mapper.Map<List<ReadProdutoDto>>(query.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaProdutosPorId(int id)
    {
        Produto produto = _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefault(produto => produto.Id == id);

        if (produto != null)
        {
            ReadProdutoDto produtoDto = _mapper.Map<ReadProdutoDto>(produto);
            return Ok(produtoDto);
        }
        return NotFound();
    }

    [Authorize(Roles = "Gerente,Funcionário")]
    [HttpPut("{id}")]
    public IActionResult AtualizarProduto(int id, [FromBody] UpdateProdutoDto produtoDto)
    {
        bool categoriaExiste = _context.Categorias.Any(c => c.Id == produtoDto.CategoriaId);
        if (!categoriaExiste)
        {
            return BadRequest(new { Message = "CategoriaId inválido. A categoria especificada não existe." });
        }

        Produto produto = _context.Produtos.FirstOrDefault(produto => produto.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        _mapper.Map(produtoDto, produto);

        produto.Status = (StatusProduto)(produto.QtdEstoque == 0 ? 0 : 1);

        _context.SaveChanges();

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Gerente")]
    public IActionResult ExcluirProduto(int id)
    {
        var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);

        if (produto == null)
        {
            return NotFound(new { Message = "Produto não encontrado." });
        }

        _context.Produtos.Remove(produto);
        _context.SaveChanges();

        return Ok(new { Message = "Produto excluído com sucesso." });
    }
}
