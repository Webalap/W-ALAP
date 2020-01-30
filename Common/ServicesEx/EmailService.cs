using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;

namespace Common.ServicesEx
{
    public class EmailService 
    {
        public static string SendEmail(string email, string subject, string templateName, object liveObject)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(GlobalSettings.Emails.NoReplyEmail);
                msg.To.Add(new MailAddress(GlobalSettings.Exigo.Api.SandboxID == 0 ?
                    email :
                    "devtest@indiahicks.com"));
                msg.IsBodyHtml = true;
                msg.BodyTransferEncoding = TransferEncoding.EightBit;
                msg.Priority = MailPriority.High;
                msg.Subject = subject;// +"+ " + email;
                msg.Body = LoadHtmlBoady(templateName, liveObject);
                var smtpConfiguration = GlobalSettings.Emails.SMTPConfigurations.Default;
                SmtpClient c = new SmtpClient(smtpConfiguration.Server);
                c.Port = smtpConfiguration.Port;
                c.DeliveryFormat = SmtpDeliveryFormat.International;
                c.Credentials = new NetworkCredential(smtpConfiguration.Username, smtpConfiguration.Password);
                c.EnableSsl = smtpConfiguration.EnableSSL;
                c.DeliveryMethod = SmtpDeliveryMethod.Network;
                c.Send(msg);
                return string.Empty;
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }


        private static string LoadHtmlBoady(string templateName, object liveObject)
        {
            using (WebClient webClient = new WebClient())
            {
                string url = (HttpRuntime.AppDomainAppPath + templateName).Replace('/', '\\');

                using (Stream stream = webClient.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string emailBody = reader.ReadToEnd();
                        Match match = Regex.Match(emailBody, @"([)([A-Z])\w+(])");
                        List<string> matches = new List<string>();
                        while (match.Success)
                        {
                            if (!matches.Contains(match.Value))
                            {
                                matches.Add(match.Value);
                            }
                            match = match.NextMatch();
                        }
                        Dictionary<string, string> replacements = GetDictionary(liveObject, matches);

                        if (null != replacements)
                        {
                            foreach (var item in replacements)
                            {
                                emailBody = emailBody.Replace(item.Key, item.Value);
                            }
                        }

                        return emailBody;
                    }
                }
            }
        }
        private static Dictionary<string, string> GetDictionary(object liveobject, List<string> fields)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            var _objectdict = liveobject.ToDictionary();
            decimal number = 0;
            foreach (var field in fields)
            {
                var value = _objectdict.Where(i => i.Key.Equals(field.Replace(@"[", string.Empty).Replace(@"]", string.Empty))).FirstOrDefault().Value;
                if (value != null)
                {
                    if (decimal.TryParse(value.ToString(), out number))
                    {
                        _dict.Add(field, Math.Floor(number).ToString());
                    }
                    else
                    {
                        _dict.Add(field, value.ToString());
                    }
                }
               
            }

            return _dict;
        }
    }
}