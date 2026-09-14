namespace Projeto_BolosJacquin.Interfaces
{
    public interface IModerationService
    {
        //Retorna True se o texto foi reprovado
        Task<bool> ModerarTexto(string texto);

    }
}
