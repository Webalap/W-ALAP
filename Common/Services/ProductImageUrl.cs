using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Services
{

    public class ProducImageUrl
    {
        public static string GetProducImageUrl(string ItemCode)
        {


            
           List<string> url =new List<string>();
            try
            {
                using (var context = Exigo.Sql())
                {

                    var SqlProcedure = string.Format("GetProducImageUrl '{0}'", ItemCode);

                    url = context.Query<string>(SqlProcedure).ToList();
                    return url.First().ToString();
                }
            }
            catch (Exception)
            {
                return "";
            }  
        }
    }
}