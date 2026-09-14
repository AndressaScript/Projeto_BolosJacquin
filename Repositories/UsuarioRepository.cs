using Microsoft.EntityFrameworkCore;
using Projeto_BolosJacquin.BdContext;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Models;
using Projeto_BolosJacquin.Utils;


namespace Projeto_BolosJacquin.Repositories
{
    public class UsuarioRepository : IUsuarios
    {
        // injeção de dependência do contexto do banco de dados
        private readonly JacquinContext _context;

        public UsuarioRepository(JacquinContext context)
        {
            _context = context;
        }

        public async Task Atualizar(Guid id, Usuarios novoUsuario)
        {
            var AtualizarUsuario = await _context.Usuarios.FindAsync(id);

            if (AtualizarUsuario != null)
            {
                AtualizarUsuario.Nome = novoUsuario.Nome;
                AtualizarUsuario.Sobrenome = novoUsuario.Sobrenome;
                AtualizarUsuario.Email = novoUsuario.Email;
                AtualizarUsuario.Perfil = novoUsuario.Perfil;
                AtualizarUsuario.Situacao = novoUsuario.Situacao;

                if (!string.IsNullOrEmpty(novoUsuario.SenhaHash))
                {
                    AtualizarUsuario.SenhaHash = Criptografia.GerarHash(novoUsuario.SenhaHash);
                }

                _context.Usuarios.Update(AtualizarUsuario);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuarios?> BuscarPorEmailESenha(string email, string senha)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null)
            {
                return null;
            }

            // Verifica se a senha digitada corresponde ao hash salvo no banco
            bool senhaValida = Criptografia.CompararHash(senha, usuario.SenhaHash);

            if (!senhaValida)
            {
                return null;
            }

            return usuario;
        }

        public async Task<Usuarios?> BuscarPorId(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == id);
        }

        public async Task Cadastrar(Usuarios usuario)
        {
            // Criptografia da senha antes de salvar no banco de dados
            usuario.SenhaHash = Criptografia.GerarHash(usuario.SenhaHash);

            await _context.Usuarios.AddAsync(usuario);

            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var UsuarioBuscado = await _context.Usuarios.FindAsync(id);

            if (UsuarioBuscado != null)
            {
                _context.Usuarios.Remove(UsuarioBuscado);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Usuarios>> Listar()
        {
            return await _context.Usuarios.AsNoTracking().ToListAsync();
        }
    }
}