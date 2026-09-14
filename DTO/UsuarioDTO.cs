using System.ComponentModel.DataAnnotations;

namespace Projeto_BolosJacquin.DTO
{
    public class UsuarioDTO
    {

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = null!;

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O sobrenome não pode exceder 100 caracteres.")]
        public string Sobrenome { get; set; } = null!;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(100, ErrorMessage = "O e-mail não pode exceder 100 caracteres.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve conter no mínimo 6 caracteres.")]
        public string Senha { get; set; } = null!;

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        [StringLength(20, ErrorMessage = "O perfil não pode exceder 20 caracteres.")]
        public string Perfil { get; set; } = "Comum"; 

        public bool Situacao { get; set; } = true;
    }

    }

