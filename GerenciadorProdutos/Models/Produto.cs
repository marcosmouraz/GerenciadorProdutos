namespace GerenciadorProdutos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do produto deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [StringLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O status do produto é obrigatório.")]
    [Column(TypeName = "nvarchar(20)")]
    public StatusProduto Status { get; set; }

    [Required(ErrorMessage = "O preço do produto é obrigatório.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Preco { get; set; }

    [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade em estoque deve ser maior ou igual a 0.")]
    public int QtdEstoque { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public virtual Categoria Categoria { get; set; }
}

public enum StatusProduto
{
    Indisponivel,
    EmEstoque
}

