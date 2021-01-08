using Library.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Core.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<List<Book>> SearchBooks(string query);
        Task AddBook(AddBooksInput input);
    }
}
