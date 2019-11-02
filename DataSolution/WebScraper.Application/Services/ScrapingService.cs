﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Application.JobInfos.Queries.GetJobInfos;
using WebScraper.Application.JobUrls.GetJobsUrls;
using WebScraper.Core.Factories;
using WebScraper.Infrastructure.Db;

namespace WebScraper.Application.Services
{
    public class ScrapingService : IScrapingService
    {
        private readonly IJobDbContext _context;
        private readonly IScraperFactory _scraperFactory;
        private IMediator _mediator;



        public ScrapingService(IJobDbContext context, IScraperFactory factory, IMediator mediator)
        {
            _context = context;
            _scraperFactory = factory;
            _mediator = mediator;
        }

        public async void ImportInitialCvOnlineData()
        {
            var scraper = _scraperFactory.BuildScraper("CvOnline");

            // Get Urls


            var getJobListQuery = new GetJobUrlsQuery();

            var result = _mediator.Send(new GetJobUrlsQuery()).Result;

            var jobInfos = _mediator.Send(new GetJobInfosQuery()).Result;

            var test = "test";
        }
    }
}