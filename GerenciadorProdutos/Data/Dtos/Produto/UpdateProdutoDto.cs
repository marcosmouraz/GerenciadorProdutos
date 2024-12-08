using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GerenciadorProdutos.Data.Dtos.Produto;

public class UpdateProdutoDto
{
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [MaxLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
    public string Descricao { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque deve ser maior ou igual a 0.")]
    public int QtdEstoque { get; set; }

    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public int CategoriaId { get; set; }
}
