using Amazon.DynamoDBv2.Model;
using Library.Common.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Library.Core.Helpers
{
    public interface IDynamoDbHelper
    {
        Task DynamoPutItem(Book book);
        Task<List<Dictionary<string, AttributeValue>>> DynamoScanAsync(string tableName);
    }
}
