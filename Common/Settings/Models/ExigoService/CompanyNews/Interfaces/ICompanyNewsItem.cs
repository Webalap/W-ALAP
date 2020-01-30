using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICompanyNewsItem
    {
        int NewsItemID { get; set; }
        string Title { get; set; }
        string Body { get; set; }
        DateTime CreatedDate { get; set; }
    }
}