using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using EventPlus.WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Projeto_BolosJacquin.Interfaces;
using Projeto_BolosJacquin.Utils;

namespace Projeto_BolosJacquin.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var credenciais = options.Value;
            var account = new Account(credenciais.CloudName, credenciais.ApiKey, credenciais.ApiSecret);

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string> UploadImagem(IFormFile arquivo)
        {
            using var stream = arquivo.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(arquivo.FileName, stream),
                Folder = "bolos_jacquin/produtos"
            };

            var resultado = await _cloudinary.UploadAsync(uploadParams);

            return resultado.SecureUrl.AbsoluteUri;
        }
    }
}