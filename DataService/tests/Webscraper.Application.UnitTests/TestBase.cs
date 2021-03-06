﻿using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.Infrastructure.Db;
using WebScraper.Infrastructure.Repositories;

namespace Webscraper.Application.UnitTests
{
    public class TestBase : IDisposable
    {
        protected readonly DataContext _context;
        protected readonly IUnitOfWork _unitOfWork;
        protected IHttpClientFactory _httpClientFactory;

        public TestBase()
        {
            _context = ContextFactory.CreateTestDataContext();
            _unitOfWork = ContextFactory.CreateTestUnitOfWork();
            _httpClientFactory = GenerateTestHttpClientFactory("");
        }

        public void Dispose()
        {
            ContextFactory.Destroy(_context);
        }

        public void SetCvOnlineContent()
        {
            var content = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestData\\CvOnlineTestPageData.txt");
            _httpClientFactory = GenerateTestHttpClientFactory(content);
        }

        public void SetCvBankasContent()
        {
            var content = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestData\\CvBankasTestPageData.txt");
            _httpClientFactory = GenerateTestHttpClientFactory(content);
        }

        public void SetCvLtContent()
        {
            var content = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\TestData\\CvLtTestPageData.txt");
            _httpClientFactory = GenerateTestHttpClientFactory(content);
        }
            
        private IHttpClientFactory GenerateTestHttpClientFactory(string content)
        {

            var mockFactory = new Mock<IHttpClientFactory>();

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(content),
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://test.com"),
            };

            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return mockFactory.Object;
        }
    }
}
