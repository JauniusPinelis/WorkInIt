﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Application.JobUrls.Queries.GetJobUrls
{
    public class GetJobUrlsQuery : IRequest<JobUrlsVm>
    {
    }
}
