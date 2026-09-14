using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projeto_BolosJacquin.DTO;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvaliacaoController : ControllerBase
    {
        private readonly IAvaliacoes _avaliacaoRepository;
        private readonly IModerationService _moderationService;

        public AvaliacaoController(IAvaliacoes avaliacaoRepository, IModerationService moderationService)
        {
            _avaliacaoRepository = avaliacaoRepository;
            _moderationService = moderationService;
        }

        
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Cadastrar([FromBody] AvaliacaoDTO dto)
        {
            try
            {
                
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out Guid usuarioLogadoId))
                {
                    return Unauthorized("Usuário não identificado.");
                }

                
                var avaliacaoExistente = await _avaliacaoRepository.BuscarPorUsuarioEProduto(usuarioLogadoId, dto.ProdutoId);
                if (avaliacaoExistente != null)
                {
                    return Conflict(new { mensagem = "Você já avaliou este produto. Atualize sua avaliação existente caso queira alterá-la." });
                }

                //Moderação de texto para verificar se o comentário é apropriado
                bool reprovado = false;
                if (!string.IsNullOrWhiteSpace(dto.Comentario))
                {
                    reprovado = await _moderationService.ModerarTexto(dto.Comentario);
                }

                var novaAvaliacao = new Avaliacoes
                {
                    AvaliacaoId = Guid.NewGuid(),
                    UsuarioId = usuarioLogadoId,
                    ProdutoId = dto.ProdutoId,
                    Nota = dto.Nota,
                    Comentario = dto.Comentario,
                    Situacao = reprovado ? "Oculto" : "Aprovado",
                    MotivoOcultacao = reprovado ? "Comentário reprovado pelo filtro de moderação automática" : null,
                    DataCriacao = DateTime.UtcNow
                };

                await _avaliacaoRepository.Cadastrar(novaAvaliacao);
                return StatusCode(201, novaAvaliacao);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }


        [HttpGet("produto/{produtoId}")]
        public async Task<IActionResult> ListarPorProduto(Guid produtoId)
        {
            try
            {
                var lista = await _avaliacaoRepository.ListarPorProduto(produtoId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListarTodas()
        {
            try
            {
                var lista = await _avaliacaoRepository.Listar();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var avaliacao = await _avaliacaoRepository.BuscarPorId(id);
                if (avaliacao == null)
                    return NotFound("Avaliação não encontrada.");

                return Ok(avaliacao);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        
        [HttpPut("{id}")]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AvaliacaoDTO dto)
        {
            try
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out Guid usuarioLogadoId))
                {
                    return Unauthorized("Usuário não identificado.");
                }

                var avaliacaoBanco = await _avaliacaoRepository.BuscarPorId(id);
                if (avaliacaoBanco == null)
                {
                    return NotFound("Avaliação não encontrada.");
                }

                // Impede que um usuário edite a avaliação de outro
                if (avaliacaoBanco.UsuarioId != usuarioLogadoId)
                {
                    return Forbid();
                }

                bool reprovado = false;
                if (!string.IsNullOrWhiteSpace(dto.Comentario))
                {
                    reprovado = await _moderationService.ModerarTexto(dto.Comentario);
                }

                avaliacaoBanco.Nota = dto.Nota;
                avaliacaoBanco.Comentario = dto.Comentario;
                avaliacaoBanco.Situacao = reprovado ? "Oculto" : "Aprovado";
                avaliacaoBanco.MotivoOcultacao = reprovado ? "Comentário reprovado pelo filtro de moderação automática" : null;

                await _avaliacaoRepository.Atualizar(id, avaliacaoBanco);
                return Ok(new { mensagem = "Avaliação atualizada com sucesso.", avaliacao = avaliacaoBanco });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // Ocultar ou restaurar avaliação 
        [HttpPatch("{id}/moderacao")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Moderar(Guid id, [FromBody] ModeracaoAvaliacaoDTO dto)
        {
            try
            {
                var avaliacaoBanco = await _avaliacaoRepository.BuscarPorId(id);
                if (avaliacaoBanco == null)
                {
                    return NotFound("Avaliação não encontrada.");
                }

                avaliacaoBanco.Situacao = dto.Situacao;
                avaliacaoBanco.MotivoOcultacao = dto.MotivoOcultacao;

                await _avaliacaoRepository.Atualizar(id, avaliacaoBanco);
                return Ok(new { mensagem = $"Avaliação alterada para a situação '{dto.Situacao}'." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // Excluir avaliação 
        [HttpDelete("{id}")]
        [Authorize(Roles = "Cliente,Admin")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(idClaim) || !Guid.TryParse(idClaim, out Guid usuarioLogadoId))
                {
                    return Unauthorized("Usuário não identificado.");
                }

                var avaliacaoBanco = await _avaliacaoRepository.BuscarPorId(id);
                if (avaliacaoBanco == null)
                {
                    return NotFound("Avaliação não encontrada.");
                }

                bool ehAdmin = User.IsInRole("Admin");
                if (!ehAdmin && avaliacaoBanco.UsuarioId != usuarioLogadoId)
                {
                    return Forbid();
                }

                await _avaliacaoRepository.Deletar(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}