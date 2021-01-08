using Library.Core.Helpers;
using Library.Core.Repositories;
using Library.Core.Repositories.Implementation;
using Library.Service.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.DynamoDBv2.Model;
using System;
using Library.Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;

namespace Library.Service.Tests
{
    [TestFixture]
    [Category("TestSuite.Unit")]
    public class SearchBooksTests
    {
        private BookDetailsController bookDetailsController;
        private IBookRepository bookRepository;
        private Mock<IDynamoDbHelper> mockDynamoDbHelper;
        private Mock<ILogger<BookDetailsController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockDynamoDbHelper = new Mock<IDynamoDbHelper>();
            mockLogger = new Mock<ILogger<BookDetailsController>>();
            bookRepository = new BookRepository(mockDynamoDbHelper.Object);
            bookDetailsController = new BookDetailsController(mockLogger.Object, bookRepository);
        }

        [Test]
        public async Task SearchBooksTestReturnsOkWithAvailableBook()
        {
            mockDynamoDbHelper.Setup(m => m.DynamoScanAsync(It.IsAny<string>())).ReturnsAsync(new List<Dictionary<string, AttributeValue>>
            {
                new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = "id" }},
                    { "Author", new AttributeValue { S = "Author" }},
                    { "Name", new AttributeValue { S = "Book" }},
                    { "Status", new AttributeValue { S = BookStatus.Available }}
                },
                new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = "id" }},
                    { "Author", new AttributeValue { S = "Author" }},
                    { "Name", new AttributeValue { S = "Cook" }},
                    { "Status", new AttributeValue { S = BookStatus.Available }}
                }

            });

            var result = await bookDetailsController.SearchBooks("Book");
            var okObjectResult = result as OkObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((HttpStatusCode)okObjectResult.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(okObjectResult.Value is List<Book>);

            var resultValue = okObjectResult.Value as List<Book>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Count, 1);
            Assert.AreEqual(resultValue.FirstOrDefault().Name, "Book");
            Assert.AreEqual(resultValue.FirstOrDefault().Status, BookStatus.Available);
        }

        [Test]
        public async Task SearchBooksTestReturnsOkWithRentedBook()
        {
            mockDynamoDbHelper.Setup(m => m.DynamoScanAsync(It.IsAny<string>())).ReturnsAsync(new List<Dictionary<string, AttributeValue>>
            {
                new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = "id" }},
                    { "Author", new AttributeValue { S = "Author" }},
                    { "Name", new AttributeValue { S = "Book" }},
                    { "Status", new AttributeValue { S = BookStatus.Rented }}
                },
                new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = "id" }},
                    { "Author", new AttributeValue { S = "Author" }},
                    { "Name", new AttributeValue { S = "Cook" }},
                    { "Status", new AttributeValue { S = BookStatus.Rented }}
                }
            });

            var result = await bookDetailsController.SearchBooks("Book");
            var okObjectResult = result as OkObjectResult;

            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual((HttpStatusCode)okObjectResult.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(okObjectResult.Value is List<Book>);

            var resultValue = okObjectResult.Value as List<Book>;

            Assert.IsNotNull(resultValue);
            Assert.AreEqual(resultValue.Count, 1);
            Assert.AreEqual(resultValue.FirstOrDefault().Name, "Book");
            Assert.AreEqual(resultValue.FirstOrDefault().Status, BookStatus.Rented);
        }

        [Test]
        public async Task SearchBooksTestThrowsException()
        {
            mockDynamoDbHelper.Setup(m => m.DynamoScanAsync(It.IsAny<string>())).ThrowsAsync(new InvalidOperationException("id"));

            var result = await bookDetailsController.SearchBooks("Book");
            var okObjectResult = result as OkObjectResult;

            Assert.IsFalse(okObjectResult != null);
            Assert.IsFalse((HttpStatusCode)okObjectResult.StatusCode == HttpStatusCode.OK);
        }
    }
}
