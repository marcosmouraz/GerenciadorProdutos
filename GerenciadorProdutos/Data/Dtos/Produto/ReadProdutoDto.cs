using GerenciadorProdutos.Data.Dtos.Categoria;
using GerenciadorProdutos.Models;
namespace GerenciadorProdutos.Data.Dtos.Produto;

public class ReadProdutoDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public StatusProduto Status { get; set; }
    public decimal Preco { get; set; }
    public int QtdEstoque { get; set; }
    public ReadCategoriaDto Categoria { get; set; }
}
