﻿using AutoMapper;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebScraper.Core.Entities;
using WebScraper.Core.Factories;
using WebScraper.Infrastructure.Db;

namespace WebScraper.Application.Services
{
    public class ScrapingService : IScrapingService
    {
        private readonly IDataContext _context;
        private readonly IScraperFactory _scraperFactory;
        private readonly IMapper _mapper;

        public ScrapingService(IDataContext context, IScraperFactory factory, 
             IMapper mapper)
        {
            _context = context;
            _scraperFactory = factory;
            _mapper = mapper;
        }

        public void Run()
        {
            Log.Information("CVOnline: Starting to scrape data");
            ScrapeCvOnlineData();
            //Log.Information("CVBankas: Starting to scrape data");
            //ScrapeCvBankasData();
            //Log.Information("CvLt: Starting to scrape data");
            //ScrapeCvLtData();
        }

        public void UpdateJobInfo(JobInfo jobInfo)
        {
            
            JobInfo entity;
            var exists = _context.JobInfos.Any(j => j.HtmlCode == jobInfo.HtmlCode);

            if (exists)
            {
                entity = _context.JobInfos.SingleOrDefault(j => j.HtmlCode == jobInfo.HtmlCode);
            }
            else
            {
                entity = new JobInfo();
                _context.JobInfos.Add(entity);
            }

            entity.HtmlCode = jobInfo.HtmlCode;
            entity.JobUrlId = jobInfo.JobUrlId;

            _context.SaveChanges();

        }

        public void UpdateUrls(IList<JobUrl> jobUrls)
        {
            foreach(var jobUrl in jobUrls)
            {
                var urlsInDb = _context.JobUrls;
                var exists = urlsInDb.Any(j => j.Url == jobUrl.Url);

                if (!exists)
                {
                    _context.JobUrls.Add(jobUrl);
                    _context.SaveChanges();
                    Log.Information("{url} has been added", jobUrl.Url);
                }
                else
                {
                    Log.Information("{url} already exists", jobUrl.Url);
                }
            }
        }

        public void ScrapeCvBankasData()
        {
            var scraper = _scraperFactory.BuildScraper("cvbankas");

            var collectedUrls = scraper.ScrapePageUrls();

            UpdateUrls(collectedUrls.ToList());
        }

        public void ScrapeCvLtData()
        {
            var scraper = _scraperFactory.BuildScraper("CvLt");

            var collectedUrls = scraper.ScrapePageUrls();

            Log.Information("CvLtScraper - saving urls to db");
            UpdateUrls(collectedUrls.ToList());
        }

        public void ScrapeCvOnlineData()
        {
            Log.Information("CvOnline - starting to scraper CvOnline");
            var scraper = _scraperFactory.BuildScraper("CvOnline");


            var collectedUrls = scraper.ScrapePageUrls().ToList();

            var cvOnlineFilter = _scraperFactory.BuildUrlFilter("CvOnline");
            cvOnlineFilter.Apply(ref collectedUrls);

            UpdateUrls(collectedUrls);
               


            Log.Information("CvOnline - CvOnline Urls saved");

            // Get Htmls
            var urlsInDb = _context.JobUrls;

            var htmlResults = scraper.ScrapeJobHtmls(urlsInDb.ToList());

            foreach(var html in htmlResults)
            {
                UpdateJobInfo(html);
            }
            
            /*
            // Parse Infos from Html

            var htmlEntities = _context.JobInfos;

            var parser = _scraperFactory.BuildParser("cvonline");

            foreach (var htmlEntity in htmlEntities)
            {
                var parseResult = parser.ParseInfo(htmlEntity);

                var entity = _context.JobInfos.Find(parseResult.Id);

                entity.Title = parseResult.Title;
                
            }**/
        }
    }
}
