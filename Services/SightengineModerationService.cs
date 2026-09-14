using System.Text.Json;
using Microsoft.Extensions.Options;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Utils;

namespace Projeto_BolosJacquin.Services
{
    public class SightengineModerationService : IModerationService
    {
        private readonly HttpClient _http;
        private readonly string _apiUser;
        private readonly string _apiSecret;

        //acima desse limiar a categoria é considerada violação
        private const double Limiar = 0.5;

        public SightengineModerationService(HttpClient http, IOptions<SightengineSettings> options)
        {
            _http = http;
            _apiUser = options.Value.ApiUser;
            _apiSecret = options.Value.ApiSecret;
        }
        public async Task<bool> ModerarTexto(string texto)
        {
            //vindo da documentação
            //FormUrlEncodedContent é usado para definir as credencias que viram através fo corpo da requisição
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["text"] = texto,
                ["lang"] = "pt",
                ["mode"] = "ml",
                ["api_user"] = _apiUser,
                ["api_secret"] = _apiSecret,
            });

            //"text/check.json" : endpoint da api externa
            //form : dados quue serão enviados junto a requisição (texto a ser moderado, etc...)
            var resposta = await _http.PostAsync("text/check.json", form);

            //verifica se a resposta (HTTP post) foi bem sucedido 
            //se o status for erro: retorna uma exception
            resposta.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(
                await resposta.Content.ReadAsStringAsync()
           );
            //obtém o elemento raiz do json
            //acesso as propriedades do json (arrays e etc)
            var root = doc.RootElement;

            //obtém a propriedade status do json e verifica se o valor é diferente de sucess
            if (root.GetProperty("status").GetString() != "success")
            {
                //tenta obter o prop "error", e dentro dela a mensagem de erro 
                //caso não retorbe, utilizamos uma mensagem de erro desconhecido
                var msg = root.TryGetProperty("error", out var err) && err.TryGetProperty("message", out var m) ? m.GetString() : "Erro desconhecido";

                throw new Exception($"Sightengine: {msg}");
            }

            var classes = root.GetProperty("moderation_classes");

            foreach (var prop in classes.EnumerateObject())
            {
                if (prop.Name == "available") continue;
                if (prop.Value.ValueKind == JsonValueKind.Number && prop.Value.GetDouble() >= Limiar)
                    return true; //reprovado porque passou do limiar
            }

            return false; //não passou do limiar, não é uma violação
        }
    }
}


//Pendencias: Cadastro na plataforma de imagens,
//users secrets das credencias apiUser e apiSecrets,        
//cadastrar uma presença para o usuario,
//crud completo, cadastrar um comentário,
//terminar todos os cruds.