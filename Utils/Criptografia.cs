namespace Projeto_BolosJacquin.Utils
{
    public class Criptografia
    {
        public static string GerarHash(string senha) //admin 1234 
        {
            return BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public static bool CompararHash(string senhaInformada, string senhaBanco)
        {
            if (string.IsNullOrEmpty(senhaInformada) || string.IsNullOrEmpty(senhaBanco))
            {
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(senhaInformada, senhaBanco);
            }

            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}
