using Common.Api.ExigoWebService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Common.ServicesEx
{
    public class ApplicationService
    {
        [Inject]
        public ExigoApi Api { get; set; }

        public static Dictionary<int, string> GetMonths()
        {
            var result = new Dictionary<int, string>();

            for (var i = 1; i <= 12; i++)
                result.Add(i, Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(i));

            return result;
        }

        public static Dictionary<int, string> GetYears(int years)
        {
            var result = new Dictionary<int, string>();

            for (var i = DateTime.Now.Year; i < DateTime.Now.AddYears(years).Year; i++)
                result.Add(i, i.ToString());

            return result;
        }
    }
}