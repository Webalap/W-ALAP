using ExigoService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Common.Api.ExigoWebService
{
    public partial class CompanyNewsResponse
    {
        public static explicit operator ExigoService.CompanyNewsItem(CompanyNewsResponse newsItem)
        {
            var model = new ExigoService.CompanyNewsItem();
            if (newsItem == null) return model;

            model.NewsItemID  = newsItem.NewsID;
            model.Title       = newsItem.Description;
            model.Body        = string.Empty;
            model.CreatedDate = newsItem.CreatedDate;

            return model;
        }
    }
}