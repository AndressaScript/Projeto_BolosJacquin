using System.ComponentModel.DataAnnotations;

namespace Projeto_BolosJacquin.DTO
{
    public class ProdutoDTO
    {
        [Required(ErrorMessage = "O id da categoria é obrigatório.")]
        public Guid CategoriaID { get; set; }

        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome execedeu o limite de 100 caracteres.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 99999999.99, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O endereço da imagem é obrigatório.")]
        [StringLength(300, ErrorMessage = "A URL da imagem não pode exceder 300 caracteres.")]
        public string EnderecoImagem { get; set; } = null!;

        [Required(ErrorMessage = "A descrição curta é obrigatória.")]
        [StringLength(100, ErrorMessage = "A descrição curta não pode exceder 100 caracteres.")]
        public string DescricaoCurta { get; set; } = null!;

        [Required(ErrorMessage = "A descrição longa é obrigatória.")]
        [StringLength(450, ErrorMessage = "A descrição longa não pode exceder 450 caracteres.")]
        public string DescricaoLonga { get; set; } = null!;

        public bool Disponibilidade { get; set; } = true;

        public bool Situacao { get; set; } = true;
    }
}
