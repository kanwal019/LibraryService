using Amazon.DynamoDBv2.Model;
using Library.Common.Models;
using Library.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Core.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly IDynamoDbHelper _dynamoDbHelper;
        private readonly string _tableName;

        public BookRepository(IDynamoDbHelper dynamoDbHelper)
        {
            _dynamoDbHelper = dynamoDbHelper;
            _tableName = "Books";
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();
            var response = await _dynamoDbHelper.DynamoScanAsync(_tableName);
            
            foreach(var item in response)
            {
                books.Add(ParseToBook(item));
            }
            
            return books.OrderBy(b => b.Name).ToList();
        }

        public async Task<List<Book>> SearchBooks(string query)
        {
            var books = await GetAllBooksAsync();
            return books.Where(b => b.Author.Contains(query) || b.Name.Contains(query)).Select(b => b).ToList();
        }

        public async Task AddBook(AddBooksInput input)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Name = input.BookName,
                Author = input.BookAuthor,
                Status = input.IsAvailable ? BookStatus.Available : BookStatus.Rented
            };

            await _dynamoDbHelper.DynamoPutItem(book);
        }

        private Book ParseToBook(Dictionary<string, AttributeValue> item)
        {
            var book = new Book();

            foreach(var pair in item)
            {
                switch (pair.Key)
                {
                    case "Id":
                        book.Id = pair.Value.S == null ? Guid.Empty : Guid.Parse(pair.Value.S);
                        break;
                    case "Name":
                        book.Name = pair.Value.S == null ? String.Empty : pair.Value.S;
                        break;
                    case "Author":
                        book.Author = pair.Value.S == null ? String.Empty : pair.Value.S;
                        break;
                    case "Status":
                        book.Status = pair.Value.S == null ? BookStatus.Available : pair.Value.S;
                        break;
                    default:
                        break;
                }
            }

            return book;
        } 
    }
}
