﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Reddit.Model;

namespace Reddit.Service.Contracts
{
    internal interface IRedditService
    {
        Task<List<RedditItem>> GetPage(string afterId = null);
    }
}