using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models {
    public class Book {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public int FormatId { get; set; }
        public int Pages { get; set; } = 0;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        public DateTime CreatedAt { get; set; } = DateTime.Today;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        public DateTime? UpdatedAt { get; set; } = null!;

        public Author Author { get; set; } = null!;

        public Genre Genre { get; set; } = null!;

        public Format Format { get; set; } = null!;
    }
}