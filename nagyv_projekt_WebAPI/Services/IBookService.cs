using nagyv_projekt.Entities;

namespace nagyv_projekt.Services;

public interface IBookService
{
    Task AddBookAsync(string title, string author, string publisher, int year);
    
    Task<Books> GetBookByIdAsync(Guid id);
    
    Task<List<Books>> GetAllBooksAsync();
    
    Task<List<Books>> GetBooksByTitleAsync(string title);
    
    Task<List<Books>> GetBooksByAuthorAsync(string author);
    
    Task<List<Books>> GetBooksByPublisherAsync(string publisher);
    
    Task<List<Books>> GetBooksByYearAsync(int year);
    
    Task UpdateBookAsync(Books book); //TODO ?
    
    Task DeleteBookAsync(Guid id);
}