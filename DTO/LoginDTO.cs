using System.ComponentModel.DataAnnotations;

namespace Projeto_BolosJacquin.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O email é obrigatório para autenticação!")]
        [EmailAddress(ErrorMessage = "Informe um email válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória para autenticação!")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 60 caracteres.")]
        public string Senha { get; set; } = string.Empty;
    }
}