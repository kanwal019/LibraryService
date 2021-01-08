using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Library.Core.Helpers
{
    public interface IDynamoDbHelper
    {
        Task DynamoPutItem(Dictionary<string, AttributeValue> item, string tableName);
        Task<List<Dictionary<string, AttributeValue>>> DynamoScanAsync(string tableName);
    }
}
