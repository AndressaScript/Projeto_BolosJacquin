using Microsoft.EntityFrameworkCore;
using Projeto_BolosJacquin.BdContext;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Repositories
{
    public class ProdutoRepository : IProdutos
    {
        //injeção de dependência do contexto do banco de dados

        private readonly JacquinContext _context;

        public ProdutoRepository(JacquinContext context)
        {
            _context = context;
        }


        public async Task Atualizar(Guid id, Produtos produto)
        {
            var atualizarProduto = await _context.Produtos.FindAsync(id);

            if (atualizarProduto != null)
            {
                atualizarProduto.CategoriaId = produto.CategoriaId;
                atualizarProduto.Nome = produto.Nome;
                atualizarProduto.Preco = produto.Preco;
                atualizarProduto.EnderecoImagem = produto.EnderecoImagem;
                atualizarProduto.DescricaoLonga = produto.DescricaoLonga;
                atualizarProduto.DescricaoCurta = produto.DescricaoCurta;
                atualizarProduto.Situacao = produto.Situacao;
                atualizarProduto.Disponibilidade = produto.Disponibilidade;

                _context.Produtos.Update(atualizarProduto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Produtos> BuscarPorId(Guid id)
        {
            return await _context.Produtos.FirstOrDefaultAsync(t => t.ProdutoId == id);
        }

        public async Task Cadastrar(Produtos produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var ProdutoBuscado = await _context.Produtos.FindAsync(id);

            if(ProdutoBuscado != null)
            {
                _context.Produtos.Remove(ProdutoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Produtos>> Listar()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<List<Produtos>> ListarPorCategoria(Guid categoriaId)
        {
            throw new NotImplementedException();
        }
    }
}
