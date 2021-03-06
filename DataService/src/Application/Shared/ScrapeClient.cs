﻿using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Core.Entities;
using Core.Shared;

namespace Application
{
    public class ScrapeClient
    {
        private readonly HttpClient _httpClient;
        private readonly IScraper _scraper;

        protected const int _sleepTime = 300;
        protected const int _scrapeLimit = 200;

        public ScrapeClient(HttpClient httpClient, IScraper scraper)
        {
            _httpClient = httpClient;
            _scraper = scraper;
        }

        public List<JobUrl> ExtractPageUrls(int pageLimit = 20)
        {

            int pageCounter = 0;
            var continueParsing = true;
            var results = new List<JobUrl>();


            while (continueParsing && pageCounter < pageLimit)
            {
                string html;
                try
                {
                    //Have some delay in parsing
                    Thread.Sleep(_sleepTime);

                    pageCounter += 1;
                    Log.Information("Scraping page {pageIndex}", pageCounter);

					// make this async
					html =  _httpClient.GetStringAsync(_httpClient.BaseAddress.ToString() + pageCounter).Result;
                }
                catch (Exception ex)
                {
                    // html is empty
                    html = "";
                }

                var pageResults = _scraper.ExtractPageUrls(html);

                // no more found - stop parsing
                if (!pageResults.Any())
                {
                    Log.Information("Finished scraping page urls");
                    continueParsing = false;
                }

                results.AddRange(pageResults);
            }

            return results;
        }

        public string ScrapeJobHtml(string url, string contentId)
        {
            try
            {

                var html = _httpClient.GetStringAsync(url).Result;
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var resultNode = htmlDocument.DocumentNode.SelectSingleNode($"//div[contains(@id, '{contentId}')]");

                if (resultNode != null)
                    return resultNode.InnerHtml;
                return "";
            }

            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
