﻿using Microsoft.Extensions.DependencyInjection;
using WebScraper.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;
using WebScraper.Core;
using System;
using WebScraper.Infrastructure;
using WebScraper.Application;
using MediatR;
using WebScraper.Application.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using WebScraper.Application.Shared;

namespace WebScraper.Console
{
    // Follow this
    // https://dzone.com/articles/dependency-injection-in-net-core-console-applicati
    public class Program
    {
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();

            var scrapeService = _serviceProvider.GetService<IScrapeService>();
            var app = new Application(scrapeService);
            app.Run();
           
            DisposeServices();
        }

        private static void RegisterServices()
        {
            ConfigureSerilog();


            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");


            var configuration = builder.Build();

            var collection = new ServiceCollection();
            collection.ConfigureMapper();
            collection.AddApplication(configuration);
            collection.AddPersistence(configuration, configuration["DefaultConnection"]);

           
            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

        private static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
			  .Enrich.FromLogContext()
			  .WriteTo.Console()
			  .CreateLogger();

            Log.Information("The global logger has been configured");
        }
    }
}