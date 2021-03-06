﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Core.CvBankas;
using WebScraper.Core.CvOnline;
using WebScraper.Core.Enums;
using WebScraper.Core.Factories;
using WebScraper.Core.Shared;

namespace WebScraper.Core.UnitTests.Scraper
{
    [TestFixture]
    public class FactoryTests
    {
        private readonly IScraperFactory _factory;
        public FactoryTests()
        {
            _factory = new ScraperFactory();
        }

        [Test]
        public void BuildScraper_GivenCvOnline_returnCvOnlineScraper()
        {
            var scraper = _factory.BuildScraper(JobPortals.CvOnline);

            scraper.Should().BeOfType<CvOnlineScraper>();
        }

        [Test]
        public void BuildScraper_GivenCvBankas_returnCvBankasScraper()
        {
            var scraper = _factory.BuildScraper(JobPortals.CvBankas);

            scraper.Should().BeOfType<CvBankasScraper>();
        }

        [Test]
        public void BuildAnalyser_GivenAnyWebsite_returnBaseAnalyser()
        {
            var cvBankasAnalyser = _factory.BuildAnalyser(JobPortals.CvBankas);
            var cvOnlineAnalyser = _factory.BuildAnalyser(JobPortals.CvOnline);

            cvBankasAnalyser.Should().BeOfType<BaseAnalyser>();
            cvOnlineAnalyser.Should().BeOfType<BaseAnalyser>();
        }
    }
}
