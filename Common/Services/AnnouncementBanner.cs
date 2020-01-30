using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Services
{
    public class AnnouncementBanner
    {
        public int AnnouncementBannerID { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Image { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int? SiteType { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
    public class AnnouncementBannerService
    {
        public static List<AnnouncementBanner> GetAnnouncementBanner(int siteType)
        {

            try
            {
                var listBanners = new List<AnnouncementBanner>();
                using (var Context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("GetAnnouncementBanners {0}", siteType);
                    listBanners = Context.Query<AnnouncementBanner>(sqlProcedure).ToList();

                }
                return listBanners;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}