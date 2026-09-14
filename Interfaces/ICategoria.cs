using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Interfaces
{
    public interface ICategoria
    {

        Task Cadastrar(Categoria categoria);

        Task<List<Categoria>> Listar();

        Task<Categoria?> BuscarPorId(Guid id);

        Task Atualizar(Guid id, Categoria categoria);

        Task Deletar(Guid id);

        Task<List<Produtos>> PossuiProdutosVinculados(Guid id);

    }
}
