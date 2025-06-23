namespace E_BookApplication.Interface.Service
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderPath);
    }

}
