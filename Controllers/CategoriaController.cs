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
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoria _categoria;

        public CategoriaController(ICategoria categoria)
        {
            _categoria = categoria;
        }

        [HttpGet]

        public async Task<IActionResult> Listar()
        {
            try
            {
                var lista = await _categoria.Listar();
                return Ok(lista);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cadastrar([FromBody] CategoriaDTO dto)
        {
            try
            {
                var categoria = new Categoria
                {
                    Nome = dto.Nome
                };
                await _categoria.Cadastrar(categoria);
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
                await _categoria.Deletar(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //LISTAGEM DE PRODUTOS ASSOCIADOS A CATEGORIA
        [HttpGet("{id:guid}/possui-produtos")]
        public async Task<IActionResult> PossuiProdutosVinculados(Guid id)
        {
            try
            {
                var produtos = await _categoria.PossuiProdutosVinculados(id);

                return Ok(new
                {
                    possuiProdutos = produtos.Any(),
                    total = produtos.Count,
                    produtos = produtos
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            try
            {
                var produtoBuscado = await _categoria.BuscarPorId(id);

                if (produtoBuscado == null)
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
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] CategoriaDTO dto)
        {
            try
            {
                var categoria = new Categoria
                {
                    Nome = dto.Nome
                };

                await _categoria.Atualizar(id, categoria);

                return NoContent(); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}



