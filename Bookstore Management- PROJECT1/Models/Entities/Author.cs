namespace Bookstore_Management__PROJECT1.Models.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }

    }
}
