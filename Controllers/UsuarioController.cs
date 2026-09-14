using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto_BolosJacquin.DTO;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;
using Projeto_BolosJacquin.Repositories;

namespace Projeto_BolosJacquin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarios _usuario;

        public UsuarioController(IUsuarios usuarioRepository)
        {
            _usuario = usuarioRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var usuarios = await _usuario.Listar();
                return Ok(usuarios);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var usuario = await _usuario.BuscarPorId(id);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                return Ok(usuario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar([FromBody] UsuarioDTO dto)
        {
            try
            {
                var usuario = new Usuarios
                {
                    Nome = dto.Nome,
                    Sobrenome = dto.Sobrenome,
                    Email = dto.Email,
                    SenhaHash = dto.Senha, 
                    Perfil = dto.Perfil,
                    Situacao = dto.Situacao,
                    DataCadastro = DateTime.UtcNow
                };

                await _usuario.Cadastrar(usuario);

                return StatusCode(201, usuario);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _usuario.Deletar(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Cliente")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioDTO dto)
        {
            try
            {
                var usuario = new Usuarios
                {
                    Nome = dto.Nome,
                    Sobrenome = dto.Sobrenome,
                    Email = dto.Email,
                    SenhaHash = !string.IsNullOrWhiteSpace(dto.Senha)  ? BCrypt.Net.BCrypt.HashPassword(dto.Senha) : string.Empty,
                    Perfil = dto.Perfil,
                    Situacao = dto.Situacao
                };

                await _usuario.Atualizar(id, usuario);

                return Ok();
            }   
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
