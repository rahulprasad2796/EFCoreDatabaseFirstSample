using EFCoreDatabaseFirstSample.Models.Repository;
using Microsoft.EntityFrameworkCore;

namespace EFCoreDatabaseFirstSample.Models.DataManager
{
    public class AuthorDataManager : IDataRepository<Author>
    {
        readonly BookStoreContext _bookStoreContext;
        public AuthorDataManager(BookStoreContext bookStoreContext)
        {
            _bookStoreContext = bookStoreContext;
        }
        public IEnumerable<Author> GetAll()
        {
            return _bookStoreContext.Authors.Include(author => author.AuthorContact).ToList();
        }

        public Author Get(long id) 
        {
            return _bookStoreContext.Authors.First(author => author.Id == id);
        }

        public void Add(Author author) 
        {
            _bookStoreContext.Add(author);
            _bookStoreContext.SaveChanges();
        }

        public void Update(Author entityToUpdate, Author entity)
        {
            entityToUpdate = _bookStoreContext.Authors
                .Include(a => a.Books)
                .Include(a => a.AuthorContact)
                .Single(b => b.Id == entityToUpdate.Id);

            entityToUpdate.Name = entity.Name;

            if(entity.AuthorContact != null)
            {
                AuthorContact authorContact = new AuthorContact();
                authorContact.Address = entity.AuthorContact.Address;
                authorContact.ContactNumber = entity.AuthorContact.ContactNumber;

                entityToUpdate.AuthorContact = authorContact;
            }

            var deletedBooks = entityToUpdate.Books.Except(entity.Books).ToList();
            var addedBooks = entity.Books.Except(entityToUpdate.Books).ToList();

            deletedBooks.ForEach(bookToDelete =>
                entityToUpdate.Books.Remove(
                    entityToUpdate.Books
                        .First(b => b.Id == bookToDelete.Id)));

            foreach (var addedBook in addedBooks)
            {
                _bookStoreContext.Entry(addedBook).State = EntityState.Added;
            }

            _bookStoreContext.SaveChanges();
        }

        public void Delete(Author author)
        {
            _bookStoreContext.Remove(author);
            _bookStoreContext.SaveChanges();
        }

        public AuthorDto GetDto(long id)
        {
            _bookStoreContext.ChangeTracker.LazyLoadingEnabled = true;

            using (var context = new BookStoreContext())
            {
                var author = context.Authors
                       .SingleOrDefault(b => b.Id == id);
                return AuthorDtoMapper.MapToDto(author);
            }
        }

        public class AuthorDto
        {
            public AuthorDto()
            {
            }

            public long Id { get; set; }

            public string Name { get; set; }

            public AuthorContactDto AuthorContact { get; set; }
        }

        public class AuthorContactDto
        {
            public AuthorContactDto()
            {
            }

            public long AuthorId { get; set; }
            public string? Address { get; set; }
            public string? ContactNumber { get; set; }
        }

        public static class AuthorDtoMapper
        {
            public static AuthorDto MapToDto(Author author)
            {
                return new AuthorDto()
                {
                    Id = author.Id,
                    Name = author.Name,

                    AuthorContact = new AuthorContactDto()
                    {
                        AuthorId = author.Id,
                        Address = author.AuthorContact?.Address,
                        ContactNumber = author.AuthorContact?.ContactNumber
                    }
                };
            }
        }
    }
}
