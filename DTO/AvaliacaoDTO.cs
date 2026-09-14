using System.ComponentModel.DataAnnotations;

namespace Projeto_BolosJacquin.DTO
{
    public class AvaliacaoDTO
    {
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public Guid ProdutoId { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória.")]
        [Range(1, 5, ErrorMessage = "A nota deve ser entre 1 e 5 estrelas.")]
        public int Nota { get; set; }

        [MaxLength(500, ErrorMessage = "O comentário não pode exceder 500 caracteres.")]
        public string? Comentario { get; set; }
    }

    public class ModeracaoAvaliacaoDTO
    {
        [Required(ErrorMessage = "A situação é obrigatória (ex: 'Oculto' ou 'Aprovado').")]
        public string Situacao { get; set; } = string.Empty;

        public string? MotivoOcultacao { get; set; }
    }
}