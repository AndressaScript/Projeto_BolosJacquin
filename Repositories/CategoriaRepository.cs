using Microsoft.EntityFrameworkCore;
using Projeto_BolosJacquin.BdContext;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Repositories
{
    public class CategoriaRepository : ICategoria

    {
        //injeção de dependência do contexto do banco de dados

        private readonly JacquinContext _context;

        public CategoriaRepository(JacquinContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, Categoria categoria)
        {
            var atualizarCategoria = await _context.Categoria.FindAsync(id);

            if (atualizarCategoria != null)
            {
                atualizarCategoria.Nome = categoria.Nome;

                _context.Categoria.Update(atualizarCategoria);
                await _context.SaveChangesAsync();
            }
        }      

        public async Task Cadastrar(Categoria categoria)
        {
            await _context.Categoria.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var categoriaBuscada = await _context.Categoria.FindAsync(id);

            if (categoriaBuscada == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            // Verifica antes se há dependência no banco
            var possuiProdutos = await _context.Produtos.AnyAsync(p => p.CategoriaId == id);
            if (possuiProdutos)
            {
                throw new Exception("Não é possível excluir esta categoria pois existem produtos associados a ela.");
            }

            _context.Categoria.Remove(categoriaBuscada);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Categoria>> Listar()
        {
            return await _context.Categoria.ToListAsync();
        }

        public async Task<List<Produtos>> PossuiProdutosVinculados(Guid id)
        {
            return await _context.Produtos.AsNoTracking().Where(p => p.CategoriaId == id).ToListAsync();
        }

        public async Task<Categoria?> BuscarPorId(Guid id)
        {
            return await _context.Categoria.FirstOrDefaultAsync(t => t.CategoriaId == id);
        }
    }
}
