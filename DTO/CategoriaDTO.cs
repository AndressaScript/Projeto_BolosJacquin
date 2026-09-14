using System.ComponentModel.DataAnnotations;

namespace Projeto_BolosJacquin.DTO
{
    public class CategoriaDTO
    {
            [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
            [StringLength(100, ErrorMessage = "Limite de 100 caracteres ultrapassado.")]
            public string Nome { get; set; } = null!;
    }
}
