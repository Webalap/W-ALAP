using Backoffice.Models;
using ExigoService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Backoffice.Services
{
    public class SuccessRocketAPI
    {
        public string Login(int sponserId)
        {
           
            string url = Settings.GetSuccessRocketApiUrl + sponserId + "/token?api_key=" + Settings.GetSuccessRocketApiKey;
            HttpWebRequest req = default(HttpWebRequest);
            CookieContainer cookies = new CookieContainer();
            req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "GET";
            req.ContentType = "application/json";
            req.CookieContainer = cookies;
            try
            {
                StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
                string responseData = responseReader.ReadToEnd();
                return responseData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string LogOut(int sponserId)
        {
            string url = Settings.GetSuccessRocketApiUrl + sponserId + "/logout?api_key=" + Settings.GetSuccessRocketApiKey;
            HttpWebRequest req = default(HttpWebRequest);
            CookieContainer cookies = new CookieContainer();
            req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "GET";
            req.ContentType = "application/json";
            req.CookieContainer = cookies;
            try
            {
                StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
                string responseData = responseReader.ReadToEnd();
                return responseData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public string Create(int sponserId)
        {


            string result = "";
            string json = "";
            // get Customer info
            string coaches="";
            int RankID = 0;
            using (var context = Exigo.Sql())
            {
                string sqlQuery = string.Format("SELECT [Email],[Date1],RankID as HighestAchievedRankId EnrollerId FROM [dbo].[Customers] WHERE  customerid={0}", sponserId);
                Customer customer = context.Query<Customer>(sqlQuery).FirstOrDefault();
                // get Coaches 30 is the rankId
                string enroller = customer.EnrollerID == null ? "2" : customer.EnrollerID.ToString();
                RankID = customer.HighestAchievedRankId;
                string upline = UplineDirector(30, sponserId).ToString();

                if (upline == enroller)
                {
                    coaches = customer.EnrollerID.ToString();
                }
                else
                {
                    coaches = upline + "," + enroller;
                }
                string name = Identity.Current.FullName;
                string email = customer.Email;
                string dateOfBirth = string.Format("{0:MM/dd/yyyy}", customer.Date1);
                string title = "Indiahicks";

                json = "{\"name\":\"" + name + "\"," +
                         "\"email\":\"" + email + "\"," +
                          "\"title\":\"" + title + "\"," +
                           "\"sponsor_id\":\"" + sponserId + "\"," +
                            "\"coaches\":\"" + coaches + "\"," +
                             "\"join_date\":\"" + dateOfBirth + "\"}";
            }
            string url = Settings.GetSuccessRocketApiUrl + sponserId + "?api_key=" + Settings.GetSuccessRocketApiKey;
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    result = client.UploadString(url, "POST", json);
                }
              
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
            string CourseId = "1";
            // if User Rank Greater than 30 and user Complete first 2  stages than enroll user into course id 3 
            if(RankID >= 30){
            SuccessRocketBadges badges = CustomerBadges(sponserId);
                if(badges.BagesCount>2)
                {
                CourseId ="3";                
                }
            }
            // Enroll User in A course 
            try
            {
                url = Settings.GetSuccessRocketApiUrl + sponserId + "/courses/" + CourseId + "?api_key=" + Settings.GetSuccessRocketApiKey;
                HttpWebRequest req = default(HttpWebRequest);
                CookieContainer cookies = new CookieContainer();
                req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "POST";
                req.ContentType = "application/json";
                req.CookieContainer = cookies;
                StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
                result = responseReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                return ex.Message; 
            }


            // Assign Coaches to Users Against Enrolled Course 
            try
            {
                url = Settings.GetSuccessRocketApiUrl + sponserId + "/courses/" + CourseId + "/coaches?api_key=" + Settings.GetSuccessRocketApiKey;
                json = "{\"coaches\":\"" + coaches + "\"}";
                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        result = client.UploadString(url, "POST", json);
                    }
            }
            catch (Exception ex )
            {
                return ex.Message; 
            }

            result = Login(sponserId);
            return result;
        }
        public SuccessRocketBadges CustomerBadges(int CustomerID)
        {
            SuccessRocketBadges Badges = new SuccessRocketBadges();
            Badges.results = new List<Badges>();
            try
            {
                string loginmessage = Login(CustomerID);
                if (loginmessage.Contains("(400) Bad Request"))
                {
                    Badges.Message = "Adventure of a Lifetime.<br />Happy Climbing.";
                    Badges.BagesCount = 0;
                }
                else
                {
                    string url = Settings.GetSuccessRocketApiUrl + CustomerID + "/badges?api_key=" + Settings.GetSuccessRocketApiKey;
                    HttpWebRequest req = default(HttpWebRequest);
                    CookieContainer cookies = new CookieContainer();
                    req = WebRequest.Create(url) as HttpWebRequest;
                    req.Method = "GET";
                    req.ContentType = "application/json";
                    req.CookieContainer = cookies;
                    StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
                    string responseData = responseReader.ReadToEnd();
                    if (responseData.Contains("(400) Bad Request"))
                    {
                        //Post method here
                        Badges.Message = "Adventure of a Lifetime.<br />Happy Climbing.";
                        Badges.BagesCount = 0;
                      
                    }
                    // i cant find any badges in jsonp result theirfore i create a dummy json for code checkin just comment below 2 line you code fine for live jsonp results
                    //   string json2 = "[{\"ID\":null,\"post_type\":null,\"points\":\"9\",\"date_earned\":\"1/1/2015\",\"timestamp_earned\":\"timestamp\",\"title\":\"string\",\"link\":\"url\",\"image\":{\"url\":'http://indiahicks.mysrdev.xyz/wp-content/uploads/sites/29/2017/01/Badge-design-your-own-300x300.jpg',\"width\":\"9\",\"height\":\"9\"}},{\"ID\":null,\"post_type\":null,\"points\":\"9\",\"date_earned\":\"1/1/2016\",\"timestamp_earned\":\"timestamp\",\"title\":\"string\",\"link\":\"url\",\"image\":{\"url\":'http://indiahicks.mysrdev.xyz/wp-content/uploads/sites/29/2017/01/avatar-2-150x150.jpg',\"width\":\"9\",\"height\":\"9\"}}]";
                    //   responseData=responseData.Replace("[]",json2);
                    else if (responseData.Contains("[]"))
                    {
                        Badges.Message = "Adventure of a Lifetime.<br />Happy Climbing.";
                        Badges.BagesCount = 0;
                    }
                    else
                    {
                        SuccessRocketBadges JsonResponseData = (SuccessRocketBadges)Newtonsoft.Json.JsonConvert.DeserializeObject(responseData, typeof(SuccessRocketBadges));
                        Badges.results = JsonResponseData.results.OrderBy(c => c.date_earned).ToList();
                        Badges.BagesCount = Badges.results.Count;
                        Badges.Message = "";
                    }
                }
 //If Badges Count Less Than 3 Than Add dummy badges in to list 
                while (Badges.results.Count != 3)
                {
                    Badges badge = new Badges();
                    badge.date_earned = DateTime.Now;
                    BadgesImages image = new BadgesImages();
                    image.url = @"data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABkAAD/4QMraHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjMtYzAxMSA2Ni4xNDU2NjEsIDIwMTIvMDIvMDYtMTQ6NTY6MjcgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDUzYgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjdCMUMxNzE0RUNGQzExRTY4OTI1RTE0QkYxQzg4RTI5IiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjdCMUMxNzE1RUNGQzExRTY4OTI1RTE0QkYxQzg4RTI5Ij4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6N0IxQzE3MTJFQ0ZDMTFFNjg5MjVFMTRCRjFDODhFMjkiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6N0IxQzE3MTNFQ0ZDMTFFNjg5MjVFMTRCRjFDODhFMjkiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7/7gAmQWRvYmUAZMAAAAABAwAVBAMGCg0AAAqTAAAUrQAAGskAACKV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";
                    badge.image = image;
                    Badges.results.Add(badge);
                }
            }
            catch (Exception ex)
            {
                Badges.Message = ex.Message;
            }
            return Badges;
        }

        private int UplineDirector(int RankID,int CustomerID)
        {
            int DirestorId = 2;
            try
            {
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("UplineLeader {0},{1}", RankID, CustomerID);
                UplineDirectors Director = context.Query<UplineDirectors>(sqlProcedure).FirstOrDefault();
                DirestorId = Director.UplineLeader;
            }
            }
            catch (Exception)
            {
            }
            return DirestorId;
        
        
        
        }

    }
}