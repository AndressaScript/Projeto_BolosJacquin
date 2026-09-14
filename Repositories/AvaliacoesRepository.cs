using Microsoft.EntityFrameworkCore;
using Projeto_BolosJacquin.BdContext;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Repositories
{
    public class AvaliacoesRepository : IAvaliacoes
    {
        private readonly JacquinContext _context;

        public AvaliacoesRepository(JacquinContext context)
        {
            _context = context;
        }

        public async Task Cadastrar(Avaliacoes avaliacao)
        {
            await _context.Avaliacoes.AddAsync(avaliacao);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Avaliacoes>> Listar()
        {
            return await _context.Avaliacoes
                .AsNoTracking()
                .Include(a => a.Usuario)
                .Include(a => a.Produto)
                .ToListAsync();
        }

        public async Task<List<Avaliacoes>> ListarPorProduto(Guid produtoId)
        {
            return await _context.Avaliacoes
                .AsNoTracking()
                .Where(a => a.ProdutoId == produtoId && a.Situacao == "Aprovado")
                .Include(a => a.Usuario)
                .ToListAsync();
        }

        public async Task<Avaliacoes?> BuscarPorId(Guid id)
        {
            return await _context.Avaliacoes
                .Include(a => a.Usuario)
                .Include(a => a.Produto)
                .FirstOrDefaultAsync(a => a.AvaliacaoId == id);
        }

        public async Task<Avaliacoes?> BuscarPorUsuarioEProduto(Guid usuarioId, Guid produtoId)
        {
            return await _context.Avaliacoes
                .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId && a.ProdutoId == produtoId);
        }

        public async Task Atualizar(Guid id, Avaliacoes avaliacaoAtualizada)
        {
            var existente = await _context.Avaliacoes.FindAsync(id);

            if (existente != null)
            {
                existente.Nota = avaliacaoAtualizada.Nota;
                existente.Comentario = avaliacaoAtualizada.Comentario;
                existente.Situacao = avaliacaoAtualizada.Situacao;
                existente.MotivoOcultacao = avaliacaoAtualizada.MotivoOcultacao;

                _context.Avaliacoes.Update(existente);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Deletar(Guid id)
        {
            var existente = await _context.Avaliacoes.FindAsync(id);

            if (existente != null)
            {
                _context.Avaliacoes.Remove(existente);
                await _context.SaveChangesAsync();
            }
        }
    }
}