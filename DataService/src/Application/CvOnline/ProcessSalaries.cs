﻿using System;
using System.Collections.Generic;
using System.Text;
using Application.Shared;
using Core.Enums;
using Core.Shared;
using Infrastructure.Repositories;

namespace Application.CvOnline
{
	public class ProcessSalaries : BaseCommand
	{
		public ProcessSalaries(IUnitOfWork unitOfWork, IAnalyser analyser, ScrapeClient scrapeClient)
		: base(unitOfWork, analyser, scrapeClient)
		{

		}

		public void Do(JobPortals jobPortal)
		{
			this.ProcessSalaries(jobPortal);
		}
	}
}
