using Common.Api.ExigoOData.Rewards;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Common.Services
{

    public class JoinUsService
    {
        public static List<JoinUs> GetJoinUs(JoinUs model)
        {

            try
            {
                List<JoinUs> listJoinUs = new List<JoinUs>();
                using (var context = Exigo.Sql())
                {
                    var SqlProcedure = string.Format("GetJoinsUS");
                    var styleAmbassadorRewardSettings = context.Query<Common.Api.ExigoOData.Rewards.StyleAmbassadorReward>(SqlProcedure).FirstOrDefault();
                    return listJoinUs;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool Insert(JoinUs joinUs)
        {
            try
            {
                using (var context = Exigo.Sql())
                {
                    var SqlProcedure = string.Format("InsertJoinsUS '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',{10},'{11}','{12}','{13}','{14}','{15}','{16}','{17}'",
                        joinUs.FirstName.Replace("'", "''"),
                        joinUs.LastName.Replace("'", "''"),
                        joinUs.Email, 
                        joinUs.Phone,
                        joinUs.Zip,
                        joinUs.City.Replace("'", "''"),
                        joinUs.State,
                        joinUs.BecomingAnAmbassador,
                        joinUs.GettingMoreInformation,
                        joinUs.HostingaGetToGether,
                        joinUs.AmbassadorId,
                        joinUs.Notes.Replace("'", "''"),
                        joinUs.UTMMedium.Replace("'", "''"),
                        joinUs.UTMSource.Replace("'", "''"),
                        joinUs.UTMCampaign.Replace("'", "''"), 
                        joinUs.UTMContent.Replace("'", "''"), 
                        joinUs.UTMTerm.Replace("'", "''"), 
                        joinUs.DateCreated);
                    var styleAmbassadorRewardSettings = context.Query<StyleAmbassadorReward>(SqlProcedure).FirstOrDefault();
                }
                return true;
            }
            catch (Exception e) { }
            return false;
        }

    }
}