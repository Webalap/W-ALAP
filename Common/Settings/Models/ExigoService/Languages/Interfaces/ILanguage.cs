using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ILanguage
    {
        int LanguageID { get; set; }
        string LanguageDescription { get; set; }
        string CultureCode { get; set; }
    }
}