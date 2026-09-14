using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Interfaces
{
    public interface IAvaliacoes
    {
        Task Cadastrar(Avaliacoes avaliacao);
        Task<List<Avaliacoes>> Listar();
        Task<List<Avaliacoes>> ListarPorProduto(Guid produtoId);
        Task<Avaliacoes?> BuscarPorId(Guid id);
        Task<Avaliacoes?> BuscarPorUsuarioEProduto(Guid usuarioId, Guid produtoId);
        Task Atualizar(Guid id, Avaliacoes avaliacao);
        Task Deletar(Guid id);
    }
}