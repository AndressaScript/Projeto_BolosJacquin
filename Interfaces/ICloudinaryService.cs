namespace EventPlus.WebAPI.Interfaces
{
    public interface ICloudinaryService
    {
        //IFormFile: arquivo binário que chega no multipart/form-data
        Task<string> UploadImagem(IFormFile arquivo);

    }
}
