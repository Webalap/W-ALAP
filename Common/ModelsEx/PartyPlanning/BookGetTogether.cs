using Backoffice.Models.EventPlanning;
using Common.Api.ExigoWebService;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Common.ModelsEx.PartyPlanning
{
    public static class BookGetTogether
    {
        public static void CreateCredentials(Customer _hostCustomer)
        {
            WebAliasName webAliasName = WebAliasName.NoName;
            try
            {
               var resUpdatePassword = Exigo.WebService().UpdateCustomer(new Common.Api.ExigoWebService.UpdateCustomerRequest
                {
                    CustomerID = _hostCustomer.CustomerID,
                    LoginPassword = GenrateRandomPassword(10),
                    CustomerType = CustomerTypes.Host,
                    CanLogin = true
                });
                var resGetWebAlias = Exigo.WebService().GetCustomerSite(new Common.Api.ExigoWebService.GetCustomerSiteRequest
                {
                    WebAlias = _hostCustomer.FirstName.ToLower().Trim()
                });
                webAliasName = WebAliasName.FirstName;
                if (resGetWebAlias != null)
                {
                    resGetWebAlias = Exigo.WebService().GetCustomerSite(new Common.Api.ExigoWebService.GetCustomerSiteRequest
                    {
                        WebAlias = _hostCustomer.FirstName.ToLower().Trim() + "-" + _hostCustomer.LastName.ToLower().Trim()
                    });
                }
                webAliasName = WebAliasName.FullName;
                GenrateEMail(_hostCustomer);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("CustomerSite not found\n"))
                {
                    switch (webAliasName)
                    {
                        case WebAliasName.NoName:
                            var resWebAlis = Exigo.WebService().SetCustomerSite(new Common.Api.ExigoWebService.SetCustomerSiteRequest
                            {
                                CustomerID = _hostCustomer.CustomerID,
                                WebAlias = _hostCustomer.FirstName.ToLower().Trim()
                            });
                            break;
                        case WebAliasName.FirstName:
                            resWebAlis = Exigo.WebService().SetCustomerSite(new Common.Api.ExigoWebService.SetCustomerSiteRequest
                            {
                                CustomerID = _hostCustomer.CustomerID,
                                WebAlias = _hostCustomer.FirstName.ToLower().Trim() + "-" + _hostCustomer.LastName.ToLower().Trim()
                            });
                            break;
                        case WebAliasName.FullName :
                            // What to do if both first name and last name both exist.
                            break;
                    }

                }
            }
        }
        private static string GenrateRandomPassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        private static bool GenrateEMail(Customer _customer) 
        {
            Dictionary<string, string> replacements=new Dictionary<string,string>();
            Exigo.SendEmail(new SendEmailRequest
            {
                From = GlobalSettings.Emails.NoReplyEmail,
                //To = new[] { _customer.Email },
                To = new[] { 
                    //"amirza@indiahicks.com",
                    "qhunain@indiahicks.com" },
                ReplyTo = new[] { GlobalSettings.Emails.NoReplyEmail },
                Subject = "India Hicks Credential has been Made",
                //Template to change
                Body = LoadFromHtmlTemplate("IH-welcome-email-template-4-embed-BLANK.html", replacements),
                SMTPConfiguration = GlobalSettings.Emails.SMTPConfigurations.Default,
                IsHtml = true,
                Priority = System.Net.Mail.MailPriority.High
            });
            return true;
        }
        private static string LoadFromHtmlTemplate(string htmlFileName, Dictionary<string, string> replacements = null)
        {
            using (WebClient webClient = new WebClient())
            {
                string backOfficeSiteDomain = ConfigurationManager.AppSettings["BackofficeSiteDomain"];

                string url = backOfficeSiteDomain + "Content/templates/" + htmlFileName;

                using (Stream stream = webClient.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string template = reader.ReadToEnd();

                        if (null != replacements)
                        {
                            foreach (var item in replacements)
                            {
                                template = template.Replace("<%" + item.Key + "%>", item.Value);
                            }
                        }

                        return template;
                    }
                }
            }
        }
    }
   
}