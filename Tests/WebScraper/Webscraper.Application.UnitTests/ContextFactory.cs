﻿using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Infrastructure.Db;
using WebScraper.Infrastructure.Services;

namespace Webscraper.Application.UnitTests
{
    public class ContextFactory
    {
        public static DataContext Create()
        {
            var _dateTime = new DateTime(2001, 1, 14);
            var _dateTimeMock = new Mock<IDateTime>();
            _dateTimeMock.Setup(m => m.Now).Returns(_dateTime);

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new DataContext(options, _dateTimeMock.Object);

            context.Database.EnsureCreated();

            // populate db with test data


            context.SaveChanges();

            return context;
        }

        public static void Destroy(DataContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
