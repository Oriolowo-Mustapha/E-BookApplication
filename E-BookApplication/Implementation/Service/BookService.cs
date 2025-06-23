
using AutoMapper;
using E_BookApplication.Contract.Repository;
using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Service
{
	public class BookService : IBookService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IFileService _fileService;

		public BookService(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_fileService = fileService;
		}

		public async Task<BookDTO> GetBookByIdAsync(Guid bookId)
		{
			var book = await _unitOfWork.Books.GetBookWithDetailsAsync(bookId);
			return book != null ? _mapper.Map<BookDTO>(book) : null;
		}

		public async Task<PagedResultDTO<BookDTO>> SearchBooksAsync(BookSearchDTO searchDto)
		{
			var result = await _unitOfWork.Books.SearchBooksAsync(searchDto);
			return new PagedResultDTO<BookDTO>
			{
				Items = _mapper.Map<IEnumerable<BookDTO>>(result.Items),
				TotalCount = result.TotalCount,
				Page = result.Page,
				PageSize = result.PageSize,
				TotalPages = result.TotalPages
			};
		}

		public async Task<IEnumerable<BookDTO>> GetBooksByVendorAsync(string vendorId)
		{
			var books = await _unitOfWork.Books.GetBooksByVendorAsync(vendorId);
			return _mapper.Map<IEnumerable<BookDTO>>(books);
		}

		public async Task<IEnumerable<BookDTO>> GetTrendingBooksAsync(int count = 10)
		{
			var books = await _unitOfWork.Books.GetTrendingBooksAsync(count);
			return _mapper.Map<IEnumerable<BookDTO>>(books);
		}

		public async Task<IEnumerable<BookDTO>> GetRecommendedBooksAsync(string userId, int count = 10)
		{
			var books = await _unitOfWork.Books.GetRecommendedBooksAsync(userId, count);
			return _mapper.Map<IEnumerable<BookDTO>>(books);
		}

		public async Task<IEnumerable<BookDTO>> GetBooksByGenreAsync(string genre)
		{
			var books = await _unitOfWork.Books.GetBooksByGenreAsync(genre);
			return _mapper.Map<IEnumerable<BookDTO>>(books);
		}

		public async Task<BookDTO> CreateBookAsync(BookCreateDTO bookCreateDto, string vendorId)
		{
			var book = _mapper.Map<Book>(bookCreateDto);
			book.VendorId = vendorId;
			book.CreatedAt = DateTime.UtcNow;

			if (bookCreateDto.CoverImagePath != null)
			{
				var imagePath = await _fileService.SaveFileAsync(bookCreateDto.CoverImage, "images/books");
				book.CoverImageUrl = imagePath;
			}

			await _unitOfWork.Books.AddAsync(book);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<BookDTO>(book);
		}

		public async Task<BookDTO> UpdateBookAsync(Guid bookId, BookUpdateDTO bookUpdateDto, string vendorId)
		{
			var book = await _unitOfWork.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.VendorId == vendorId);
			if (book == null) return null;

			_mapper.Map(bookUpdateDto, book);
			book.UpdatedAt = DateTime.UtcNow;


			if (bookUpdateDto.CoverImage != null)
			{

				if (!string.IsNullOrWhiteSpace(book.CoverImageUrl))
				{
					var oldImagePath = Path.Combine("wwwroot", book.CoverImageUrl);
					if (File.Exists(oldImagePath))
						File.Delete(oldImagePath);
				}

				var imagePath = await _fileService.SaveFileAsync(bookUpdateDto.CoverImage, "images/books");
				book.CoverImageUrl = imagePath;
			}

			_unitOfWork.Books.Update(book);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<BookDTO>(book);
		}

		public async Task<bool> DeleteBookAsync(Guid bookId, string vendorId)
		{
			var book = await _unitOfWork.Books.FirstOrDefaultAsync(b => b.Id == bookId && b.VendorId == vendorId);
			if (book == null) return false;


			_unitOfWork.Books.Update(book);
			await _unitOfWork.SaveChangesAsync();

			return true;
		}

		public async Task<IEnumerable<string>> GetGenresAsync()
		{
			var books = await _unitOfWork.Books.GetAllAsync();
			return books.Select(b => b.Genre).Distinct().Where(g => !string.IsNullOrEmpty(g));
		}

		public async Task<IEnumerable<string>> GetAuthorsAsync()
		{
			var books = await _unitOfWork.Books.GetAllAsync();
			return books.Select(b => b.Author).Distinct().Where(a => !string.IsNullOrEmpty(a));
		}
	}

}
