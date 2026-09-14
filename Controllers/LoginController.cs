using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Projeto_BolosJacquin.DTO;
using Projeto_BolosJacquin.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projeto_BolosJacquin.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação de usuários via JWT (JSON Web Token).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarios _usuario;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuarios usuario, IConfiguration configuration)
        {
            _usuario = usuario;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // 1. Busca o usuário pelo e-mail e valida a senha
            var usuarioEncontrado = await _usuario.BuscarPorEmailESenha(dto.Email, dto.Senha);

            // 2. Se as credenciais forem inválidas, retorna 401 (Unauthorized)
            if (usuarioEncontrado == null)
            {
                return Unauthorized("Credenciais inválidas");
            }

            // 3. Monta as Claims do usuário no token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioEncontrado.UsuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuarioEncontrado.Email),
                new Claim("nome", $"{usuarioEncontrado.Nome} {usuarioEncontrado.Sobrenome}"),
                new Claim(ClaimTypes.Role, usuarioEncontrado.Perfil), // Essencial para [Authorize(Roles = "...")]
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 4. Cria a chave de segurança usando a chave cadastrada no User Secrets
            var chaveSecreta = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            // 5. Define as credenciais de assinatura com HMAC SHA256
            var credenciais = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256);

            // 6. Constrói o JWT com os mesmos emissores definidos no Program.cs
            var token = new JwtSecurityToken(
                issuer: "BolosJacquin.WebAPI",
                audience: "BolosJacquin.WebAPI",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais
            );

            // 7. Serializa o token e retorna os dados
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                Expiracao = token.ValidTo,
                Usuario = new
                {
                    usuarioEncontrado.UsuarioId,
                    usuarioEncontrado.Nome,
                    usuarioEncontrado.Sobrenome,
                    usuarioEncontrado.Email,
                    usuarioEncontrado.Perfil
                }
            });
        }
    }
}