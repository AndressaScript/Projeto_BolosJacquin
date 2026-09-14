using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.Interfaces
{
    public interface IUsuarios
    {
        Task Cadastrar(Usuarios usuario);

        Task<List<Usuarios>> Listar();

        Task<Usuarios?> BuscarPorId(Guid id);

        Task<Usuarios?> BuscarPorEmailESenha(string email, string senha);

        Task Atualizar(Guid id, Usuarios novoUsuario);

        Task Deletar(Guid id);
    }
}
