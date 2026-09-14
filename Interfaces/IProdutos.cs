using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Interfaces
{
    public interface IProdutos
    {

        Task<List<Produtos>> Listar();

        Task<List<Produtos>> ListarPorCategoria(Guid categoriaId);

        Task Deletar(Guid id);

        Task Atualizar(Guid id, Produtos produto);

        Task<Produtos> BuscarPorId(Guid id);

        Task Cadastrar(Produtos produto);

    }
}
