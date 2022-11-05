using System.ComponentModel.DataAnnotations;

namespace BooksApi.Models {
    public class Genre {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = null!;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        public DateTime CreatedAt { get; set; } = DateTime.Today;
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        public DateTime? UpdatedAt { get; set; } = null!;
        public ICollection<Book> Books = null!;
    }
}