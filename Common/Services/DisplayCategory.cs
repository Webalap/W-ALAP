using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Services
{
    public class DisplayCategory
    {
        public int DisplayCategoryID { get; set; }
        public int ID { get; set; }
        public string ImageURL { get; set; }
        public string Name { get; set; }
        public string SiteType { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool? ShowOnCorporateSite { get; set; }
        public int SortOrder  { get; set; }
    }
    public class DisplayCategoryService
    {
        public static List<DisplayCategory> GetDisplayCategories(string SiteType)
        {
            try
            {
                //var context = Exigo.CreateODataContext<RewardsContext>(GlobalSettings.Exigo.Api.SandboxID);
                //var listCategories = context.DisplayCategories.Where(
                //    c => c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && c.SiteType == siteType)
                //    .OrderBy(c => c.SortOrder)
                //    .ToList();
                //return listCategories.Select(cat => new DisplayCategory
                //{
                //    DisplayCategoryID = cat.DisplayCategoryID, Id = cat.ID, ImageUrl = cat.ImageURL, Name = cat.Name, SiteType = cat.SiteType, Action = cat.Action, Controller = cat.Controller, StartDate = cat.StartDate, EndDate = cat.EndDate, SortOrder = cat.SortOrder, ShowOnCorporateSite = cat.ShowOnCorporateSite
                //}).ToList();
                var CategoriesList = new List<DisplayCategory>();
                using (var Context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("GetWebCategoriesBySiteType '{0}'", SiteType.Replace("'", "''"));
                    CategoriesList = Context.Query<DisplayCategory>(sqlProcedure).ToList();
                }
                return CategoriesList;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}