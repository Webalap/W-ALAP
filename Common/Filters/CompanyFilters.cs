using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using ExigoService;
using Common.Api.ExigoWebService;
using System.Web.Mvc;
using Common.Filters;

namespace Common.Filters
{
    public static class CompanyFilters
    {
        /// <summary>
        /// Exigo-specific API credentials and configurations
        /// </summary>
        public static void Register(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExigoErrorAttribute());
            filters.Add(new ValidateAntiForgeryTokenOnAllPostsAttribute());
        }
    }
}