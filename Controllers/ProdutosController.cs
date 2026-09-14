using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto_BolosJacquin.DTO;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {

        private readonly IProdutos _produto;

        public ProdutosController(IProdutos produto)
        {
            _produto = produto;
        }

        [HttpGet]

        public async Task<IActionResult> Listar()
        {
            try
            {
                var lista = await _produto.Listar();
                return Ok(lista);  
            }

            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Cadastrar([FromBody] ProdutoDTO dto)
        {
            try
            {
                var produto = new Produtos
                {
                    CategoriaId = dto.CategoriaID,
                    Nome = dto.Nome,
                    Preco = dto.Preco,
                    EnderecoImagem = dto.EnderecoImagem,
                    DescricaoCurta = dto.DescricaoCurta,
                    DescricaoLonga = dto.DescricaoLonga,
                    Disponibilidade = dto.Disponibilidade,
                    Situacao = dto.Situacao
                };
                await _produto.Cadastrar(produto);
                return Ok();
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
                await _produto.Deletar(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id:guid}")]

        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var produtoBuscado = await _produto.BuscarPorId(id);

                if(produtoBuscado == null)
                {
                    return NotFound("Produto não encontrado");
                }

                return Ok(produtoBuscado);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ProdutoDTO dto)
        {
            try
            {
                var produto = new Produtos
                {
                    CategoriaId = dto.CategoriaID,
                    Nome = dto.Nome,
                    DescricaoCurta = dto.DescricaoCurta,
                    DescricaoLonga = dto.DescricaoLonga,
                    EnderecoImagem = dto.EnderecoImagem,
                    Situacao = dto.Situacao,
                    Disponibilidade = dto.Disponibilidade,
                    Preco = dto.Preco
                };

                await _produto.Atualizar(id, produto);

                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
