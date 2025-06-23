using E_BookApplication.Interface.Service;

namespace E_BookApplication.Implementation.Service
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _env;

		public FileService(IWebHostEnvironment env)
		{
			_env = env;
		}

		public async Task<string> SaveFileAsync(IFormFile file, string folderPath)
		{
			var uploadsFolder = Path.Combine(_env.WebRootPath, folderPath);
			if (!Directory.Exists(uploadsFolder))
				Directory.CreateDirectory(uploadsFolder);

			var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}
			return Path.Combine(folderPath, uniqueFileName).Replace("\\", "/");
		}
	}

}
