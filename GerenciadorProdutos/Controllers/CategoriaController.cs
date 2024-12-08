using AutoMapper;
using GerenciadorProdutos.Data;
using GerenciadorProdutos.Data.Dtos.Categoria;
using GerenciadorProdutos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProdutos.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriaController : ControllerBase
{
    private UnifiedDbContext _context;
    private IMapper _mapper;

    public CategoriaController(UnifiedDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IEnumerable<ReadCategoriaDto> ListarCategorias()
    {
        return _mapper.Map<List<ReadCategoriaDto>>(_context.Categorias);
    }

    [Authorize(Roles = "Gerente,Funcionário")]
    [HttpPost]
    public IActionResult CriarCategoria([FromBody] CreateCategoriaDto categoriaDto)
    {

        var categoriaExistente = _context.Categorias
        .FirstOrDefault(c => c.Nome.Equals(categoriaDto.Nome));

        if (categoriaExistente != null)
        {
            return BadRequest(new { mensagem = "O nome da categoria já está em uso." });
        }

        Categoria categoria = _mapper.Map<Categoria>(categoriaDto);
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaCategoriasPorId), new { Id = categoria.Id }, categoriaDto);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaCategoriasPorId(int id)
    {
        Categoria categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);
        if (categoria != null)
        {
            ReadCategoriaDto categoriaDto = _mapper.Map<ReadCategoriaDto>(categoria);
            return Ok(categoriaDto);
        }
        return NotFound();
    }

    [Authorize(Roles = "Gerente,Funcionário")]
    [HttpPut("{id}")]
    public IActionResult AtualizarCategoria(int id, [FromBody] UpdateCategoriaDto categoriaDto)
    {

        var categoriaExistente = _context.Categorias
            .FirstOrDefault(c => c.Nome.Equals(categoriaDto.Nome));

        if (categoriaExistente != null)
        {
            return BadRequest(new { mensagem = "O nome da categoria já está em uso." });
        }

        Categoria categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);
        if (categoria == null)
        {
            return NotFound();
        }
        _mapper.Map(categoriaDto, categoria);
        _context.SaveChanges();
        return Ok();
    }
}
