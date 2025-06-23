using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BookApplication.DTOs
{

    public class BookDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime PublicationDate { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        //public string CoverImagePath { get; set; }
    }

    public class BookDetailsViewModel
    {
        public BookDTO Book { get; set; } 

        public IEnumerable<ReviewDTO> Reviews { get; set; } 

        public ReviewDTO UserReview { get; set; } 
        public double AverageRating { get; set; } 
    }


    public class BookCreateDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [StringLength(50)]
        public string Genre { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(13)]
        public string ISBN { get; set; }

        public string Description { get; set; }

        [StringLength(500)]
        public IFormFile CoverImage { get; set; }

        public DateTime PublicationDate { get; set; }

        public string CoverImagePath { get; set; }

    }

    public class BookUpdateDTO : BookCreateDTO
    {
        public int Id { get; set; }
    }

    public class BookSearchDTO
    {
        public string SearchTerm { get; set; }
        public string Genre { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Author { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "Title"; 
        public bool SortDescending { get; set; } = false;
    }


    public class PagedResultDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

}
