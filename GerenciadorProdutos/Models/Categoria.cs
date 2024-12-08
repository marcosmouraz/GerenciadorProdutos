namespace GerenciadorProdutos.Models;
using System.ComponentModel.DataAnnotations;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da categoria deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

}
