namespace SingalrAPI.Services.BlobService
{
    public interface IBlobService
    {
        Task<List<string>> GetAllBlobs();
        Task DeleteBlobAsync(string blobName);
        Task<Stream> DownloadBlobAsync(string blobName);
        Task UploadBlobAsync(string blobName, Stream content);
    }
}