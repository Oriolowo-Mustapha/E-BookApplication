namespace E_BookApplication.Contract.Service
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderPath);
    }

}
