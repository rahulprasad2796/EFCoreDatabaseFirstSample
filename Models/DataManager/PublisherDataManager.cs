using EFCoreDatabaseFirstSample.Models.Repository;

namespace EFCoreDatabaseFirstSample.Models.DataManager
{
    public class PublisherDataManager : IDataRepository<Publisher>
    {
        readonly BookStoreContext _bookStoreContext;
        public PublisherDataManager(BookStoreContext bookStoreContext) 
        {
            _bookStoreContext = bookStoreContext;
        }

        public IEnumerable<Publisher> GetAll()
        {
            return _bookStoreContext.Publishers.ToList();
        }

        public Publisher Get(long id)
        {
            return _bookStoreContext.Publishers.SingleOrDefault(x => x.Id == id);
        }

        public void Add(Publisher publisher)
        {
            _bookStoreContext.Add(publisher);
            _bookStoreContext.SaveChanges();
        }

        public void Update(Publisher publisherToUpdate, Publisher publisher) 
        {
            publisherToUpdate.Name = publisher.Name;
            publisherToUpdate.Books = publisher.Books;

            _bookStoreContext.Update(publisherToUpdate);
            _bookStoreContext.SaveChanges();
        }

        public void Delete(Publisher publisher) 
        {
            _bookStoreContext.Remove(publisher);
            _bookStoreContext.SaveChanges();
        }
    }
}
