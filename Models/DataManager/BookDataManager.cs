using EFCoreDatabaseFirstSample.Models.Repository;

namespace EFCoreDatabaseFirstSample.Models.DataManager
{
    public class BookDataManager : IDataRepository<Book>
    {
        readonly BookStoreContext _bookStoreContext;
        public BookDataManager(BookStoreContext bookStoreContext)
        {
            _bookStoreContext = bookStoreContext;
        }
        public IEnumerable<Book> GetAll()
        {
            return _bookStoreContext.Books.ToList();
        }

        public Book Get(long id)
        {
            Book? book = _bookStoreContext.Books.SingleOrDefault(x => x.Id == id);

            if (book == null) return null;

            _bookStoreContext.Entry(book).Collection(b => b.Authors).Load();

            _bookStoreContext.Entry(book).Reference(b => b.Publisher).Load();

            _bookStoreContext.Entry(book).Reference(b => b.Category).Load();

            return book;
        }

        public void Add(Book book)
        {
            _bookStoreContext.Add(book);
            _bookStoreContext.SaveChanges();
        }

        public void Update(Book bookToUpdate, Book book)
        {
            bookToUpdate.Title = book.Title;
            bookToUpdate.CategoryId = book.CategoryId;
            bookToUpdate.PublisherId = book.PublisherId;
            bookToUpdate.Category = book.Category;
            bookToUpdate.Publisher = book.Publisher;
            bookToUpdate.Authors = book.Authors;

            _bookStoreContext.SaveChanges();
        }

        public void Delete(Book book)
        {
            _bookStoreContext.Remove(book);
            _bookStoreContext.SaveChanges();
        }
    }
}
