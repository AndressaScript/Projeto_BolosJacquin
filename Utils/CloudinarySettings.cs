namespace Projeto_BolosJacquin.Utils
{
    public class CloudinarySettings
    {
        public string CloudName { get; set; } = string.Empty;

        //chave publica de identificação da api
        public string ApiKey { get; set; } = string.Empty;

        //chave secreta que assina/autentica as requisições
        public string ApiSecret { get; set; } = string.Empty;
    }
}
