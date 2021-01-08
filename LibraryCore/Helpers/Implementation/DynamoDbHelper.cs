using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Library.Common.Models;

namespace Library.Core.Helpers.Implementation
{
    public class DynamoDbHelper : IDynamoDbHelper
    {
        private readonly AmazonDynamoDBClient client;

        public DynamoDbHelper()
        {
            client = new AmazonDynamoDBClient(Amazon.RegionEndpoint.APSouth1);
        }

        public async Task DynamoPutItem(Dictionary<string, AttributeValue> item, string tableName)
        {
            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item
            };

            var response = await client.PutItemAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException(response.ResponseMetadata.RequestId);
            }
        }

        public async Task DynamoGetItemsAsync()
        {
            var request = new QueryRequest
            {
                TableName = "Books",
                ProjectionExpression = "Id, Author, Name, Status"
            };

            var response = await client.QueryAsync(request);
            _ = response;
        }

        public async Task<List<Dictionary<string, AttributeValue>>> DynamoScanAsync(string tableName)
        {
            var request = new ScanRequest
            {
                TableName = tableName
            };

            var response = await client.ScanAsync(request);
            
            if(response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InvalidOperationException(response.ResponseMetadata.RequestId);
            }

            return response.Items;
        }
    }
}
