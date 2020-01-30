using Common.Api.ExigoWebService;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common
{
    public static class GlobalSettings
    {
        /// <summary>
        /// Exigo-specific API credentials and configurations
        /// </summary>
        public static class Exigo
        {
            /// <summary>
            /// Web service, OData and SQL API credentials and configurations
            /// </summary>
            public static class Api
            {
                public const string LoginName = "API_BO";
                public const string Password = "7iOiJz5dgk2E1Tv1f9WtvA44";
                public const string CompanyKey = "IndiaHicks";

                //public static bool UseSandboxGlobally  = false;
                public static int SandboxID { get { return int.Parse(ConfigurationManager.AppSettings["ExigoEnvironment"]); } }  //return (UseSandboxGlobally) ? 1 : 0; } }


                public static string GetSubdomain(int? sandboxID = null)
                {
                    if (!sandboxID.HasValue)
                    {
                        sandboxID = SandboxID;
                    }

                    if (sandboxID > 0)
                    {
                        return "sandboxapi" + sandboxID;
                    }

                    return "indiahicks-api";
                }

                /// <summary>
                /// Replicated SQL connection strings and configurations
                /// </summary>
                public static class Sql
                {
                    public static class ConnectionStrings
                    {
                        public static string GetConnectionString()
                        {
                            if (0 == SandboxID)
                            {
                                return ExigoProduction;
                            }
                            if (1 == SandboxID)
                            {
                                return ExigoSandbox1;
                            }
                            if (2 == SandboxID)
                            {
                                return ExigoSandbox2;
                            }
                            //BE-Comment
                            //ExigoSandbox3
                            if (3 == SandboxID)
                            {
                                return ExigoSandbox3;
                            }

                            throw new ApplicationException("Invalid or missing ExigoEnvironment configuration.");
                        }

                        public static string ExigoProduction
                        {
                            get { return ConfigurationManager.ConnectionStrings["ExigoProduction"].ConnectionString; }
                        }

                        public static string ExigoSandbox1
                        {
                            get { return ConfigurationManager.ConnectionStrings["ExigoSandbox1"].ConnectionString; }
                        }
                        public static string ExigoSandbox2
                        {
                            get { return ConfigurationManager.ConnectionStrings["ExigoSandbox2"].ConnectionString; }
                        }
                        //BE-Comment
                        //ExigoSandbox3
                        public static string ExigoSandbox3
                        {
                            get { return ConfigurationManager.ConnectionStrings["ExigoSandbox3"].ConnectionString; }
                        }
                    }
                }
            }

            /// <summary>
            /// Payment API credentials
            /// </summary>
            public static class PaymentApi
            {
                //public const string LoginName = "exigodemo_w77ipWL41";
                //public const string Password = "594bIToTPmPe7W574IcoGzTg";
                public const string LoginName = "indiahicks_5XZFiY1iU";
                public const string Password = "7iOiJz5dgk2E1Tv1f9WtvA44";
            }
        }

        /// <summary>
        /// Default backoffice settings
        /// </summary>
        public static class Backoffices
        {
            public static int SessionTimeout = 30; // In minutes

            /// <summary>
            /// Silent login URL's and configurations
            /// </summary>
            public static class SilentLogins
            {
                public static string SilentLoginDistributorBackofficeUrl = ConfigurationManager.AppSettings["BackofficeSiteDomain"].ToString() + "authentication/silentlogin/?token={0}&IsPendingOrder={1}";
                public static string SilentLoginReplicatedUrl = ConfigurationManager.AppSettings["ReplicatedSiteDomain"].ToString() + "rep/india/account/silentlogin/?token={0}&IsPendingOrder={1}";
            }

            /// <summary>
            /// Waiting room configurations
            /// </summary>
            public static class WaitingRooms
            {
                /// <summary>
                /// The number of days a customer can be placed in a waiting room after their initial enrollment.
                /// </summary>
                public static int GracePeriod = 30; // In days
            }
        }
        public static class LandedCost {

            public static string LandedCostUrl = ConfigurationManager.AppSettings["LandedCostUrl"].ToString();

            public static string LandedCostKey = ConfigurationManager.AppSettings["LandedCostKey"].ToString();
        }
        public static class IpStack
        {
            public static string IpStackUrl = ConfigurationManager.AppSettings["IpStackUrl"].ToString();
            public static string IpStackKey = ConfigurationManager.AppSettings["IpStackKey"].ToString();
        }

        /// <summary>
        /// Default replicated site settings
        /// </summary>
        public static class ReplicatedSites
        {
            public static string DefaultWebAlias = "india"; //"www"; //"CorpOrphan";
            public static int IdentityRefreshInterval = 15; // In minutes
            // for pending order silent login test doamin
            public static string SilentLoginReplicatedUrl = ConfigurationManager.AppSettings["ReplicatedSiteDomain"].ToString() + "rep/india/account/silentlogin/?token={0}&IsPendingOrder={1}";


        }

        /// <summary>
        /// Market configurations used for orders, autoships, products and more
        /// </summary>
        public static class Markets
        {
            public static List<Market> AvailableMarkets = new List<Market> ()
                                                        {
                                                            new UnitedStatesMarket(),
                                                            new CanadaMarket(),
                                                        };
        }

        /// <summary>
        /// Language and culture code configurations
        /// </summary>
        public static class Globalization
        {
            public static List<Language> AvailableLanguages = new List<Language>
                                                                {
                                                                    new Language(Languages.English, "en-US")
                                                                };
        }

        /// <summary>
        /// Language and culture code configurations
        /// </summary>
        public static class AutoOrders
        {
            public static List<int> AvailableFrequencyTypeIDs         = new List<int>
                                                                        {
                                                                            FrequencyTypes.Monthly,
                                                                            FrequencyTypes.Quarterly,
                                                                            FrequencyTypes.Yearly
                                                                        };
            public static List<FrequencyType> AvailableFrequencyTypes = AvailableFrequencyTypeIDs.Select(c => ExigoService.Exigo.GetFrequencyType(c)).ToList();
        }

        /// <summary>
        /// Customer avatar configurations
        /// </summary>
        public static class Avatars
        {
            public static string DefaultAvatarAsBase64 = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAEsASwDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD0WiiigAooooAKKKKACiiigBO1LRRQAneloooAKKKKAEpaKKACiiigApO1LRQAUUUUAFFFFABRRRQAUUUUAJ3paKKACiiigAoopO1AC0UUUAFFFFABRSd6WgAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAopKWgBOKWiigAooooAKKTvS0AJS0UUAFJS0UAFFFFABRRRQAUnelooAKKKKACiiigApO9LRQAnaloooAKKKKACiiigBKKWigAopO9LQAUlLRQAUUUUAFJS0UAJS0UUAFFFFACUtFFABSUtFABSUtJQAtFFFABRRRQAUUUUAFFFFABRRRQAUUUnegBaKKKACinRxvK4SNSzHsK17bQJGwbiTYP7q8mgDGpyo7/AHULfQV1cOl2cH3YQx9W5q4FVRhQB9KAOMFpdHpbSn/gBprW06fehkH1Q121FAHCkYNFdrJbwyjEkSN9VrPn0O1lyY90Te3IoA5qir11pVzaguV3oP4lqjQAUUneloAKKKKACiiigAooooAKKKKACiiigAooooAKKTvS0AFFFFABRRRQAUnalooAKKKKACiiigAq9YaZJetuOUiHVvX6U/S9NN3J5knEKn/vr2rp1VUUKoAUcADtQBFbWkNomyFMep7mp6KKACiiigAooooAKKKKACsq/wBHiuQXhAjl/Rq1aKAOIlieCQxyKVYdQaZXXX9hHfRYPEgHyt6Vyk0TwStHIuGWgBlFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFACd6WiigAooooAKKKKACk5paKACrFlaNeXKxDherH0FV66jRrUW9mJCP3kvzH6dqAL8USQxLGgwqjAFPoooAKKKKACikpaACiiigAooooAKKKKACszV7D7TD5qD96g7fxD0rTooA4Wir+r2gtbzKDCSfMPaqFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQBPZQ/abyOLszc/SuyAwMDpXO+H4g11JIf4VwPxro6ACiiigAooooAKKO1HagAooooAO9HakpaACiiigAooooAzdat/OsGcfejO4f1rl67eRBJE6HoykGuJZdrFT1BwaAEooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAoopO1AHQ+Hl/cTN6uBW1WP4e/49JR/00/pWxQAUUUUAFFFJ3oAWiiigAooooAKKKKACiiigAooooAK4y+XbfTr/tmuzrjtROdRuP8AfNAFaiiigAooooAKKKKACiiigAooooAKKKKACkpaKACiiigAooooAKKKKAN3w6/+vj+jVu1y2izeVqKqTxICtdTQAUUUUAFFJS0AFFFFABRRRQAUUUUAFFFFABRRRQAVxVw/mXMr/wB5yf1rrL6b7PZTSdwpx9a46gAooooAKKKKACiiigAooooAKKKKAE70tFFABRRRQAUUUnegBaKKKACik7UtACo5jkV14ZTkV2dvMLi3SVejDNcXW1oV7tc2rn5W5T60AdBRRRQAUUdqKACikpaACiiigAooooAO1FFFABRRUU0yQQtK5wqjJoAyNfucLHbA9fmb+lYNS3E73M7zP95j+VRUAFJ2paTvQAtFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFKrFWDA4I5BpKKAOq0zUFvIcMcTL94evvWhXEQzPBKssbbWXvXT6fqUV6m0kLMOq+v0oA0KKKKACiiigAooooAKKKKACiimswRSzEBR1JoAUkAZNczq+ofapPJiP7lD1/vGpNU1bz8wW5Ij6M396sigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACk70tFABRRRQAUUUnegBaKKKAClVmRgykhh0Iq5Z6XcXnzAbI/7zD+Vb1ppVta4bb5kn95qAINMvLudQs0DFe0vStaiigAopKWgAooooAKKKKAGSu0cZZELkdFB61y+o311cSFJlaJR/BjFdXUUsEc6bZUDr7igDiqK3bvQOr2r/APAG/wAaxZI3hcpIhVh2IoAZRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUVJBBJcSrHEu5jQAxEeRwiKWY8ACugsNFSLElzh36hewq3YadHZJnhpT95sfyq9QAgAAwKWiigAooooAKKSloAKKKKACiiigAooo7UAFV7mzhu02ypn0PcVYooA5K+02WybP34j0cD+dUq7hlV1KsAVPUGuc1PSTbZmgBMXcf3aAMqiiigAooooAKKKKACiiigAooooAKKKTtQAtFFFABRSUtABRRRgmgB8UTzSrHGu5m4ArqtPsUsYcDmRvvNUOk6eLWLzZB++f8A8dHpWnQAUUUd6ACjtRRQAUlFLQAUUUUAFFFFABRRRQAUUUUAFFFFABSEAjB6UtFAHNarpn2ZjNEP3THkf3ayq7h0WRCrgMp4INcnqNi1lcYHMbcqf6UAU6KKKACiiigAooooAKKKKACiik7UALRRRQAlFLRQAVr6LY+dL9okHyIflHqazLeB7idIU+8xxXY28K28CRIAFUYoAlooooAKKKKACiiigAooooAKO1FFABRRRQAUUUUAFFFFABRRRQAUUUUAFVr21S8tmibg9VPoas0UAcPJG0UjRuMMpwRTa3des+l0g9n/AKGsKgAooooAKKKKACiiigAooooAKKKKACiiljRpJFRclmOBQBu6BagK10w5Pyp/WtyoreFbeBIl6KMVLQAUUUUAFFFHagApKWigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAGSxrNE0bjKsMGuMuImt7h4m6qcV21c/4gttskdwo4b5W+tAGLRRRQAUUUUAFFFFACd6WiigAopKWgArT0O382+8wj5Yxu/HtWZXSaDDssmkPWRv0FAGtRR2ooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAqpqMH2mxlTGTjK/UVbooA4Wip72HyL2aPsG4+lQUAFFFFABRRRQAUUUUAFFFFABXZWUfk2UMfogz9a5GBPMuI0/vOBXa0ALRRRQAUlLRQAUUUUAFFFFABRRSUAHeloooAKKKKACiiigAo7UUUAFFFFABR2oooAKKKKACiiigDmtfjC3qPj76frWVXQeIY8wQyf3WxXP0AFFFFABRRRQAUUUUAFFJ2ooAuaWu/U4B/tZ/KuvrldF51SP2B/lXVUAFHaiigA70UlLQAUUd6O9ABRRRQAlLRR3oAKKKKACiiigAooooAKKKKACiiigAoo70UAFFFFABRRR3oAzdcXdpjn+6wNcvXW6tzpc/0H865KgAooooAKKTvS0Af//Z";
        }

        /// <summary>
        /// Error logging configuration
        /// </summary>
        public static class ErrorLogging
        {
            public static bool ErrorLoggingEnabled = true;
            public static string[] EmailRecipients = { "errors@indiahicks.com" };
        }

        /// <summary>
        /// Email configurations
        /// </summary>
        public static class Emails
        {
            public static string NoReplyEmail   = "India Hicks <noreply@indiahicks.com>";
            public static string VerifyEmailUrl = "http://backoffice.indiahicks.com/verifyemail"; //changed from http://localhost:53576/verifyemail

            public static class SMTPConfigurations
            {
                public static SMTPConfiguration Default = new SMTPConfiguration
                                                            {
                                                                Server = "smtp.mandrillapp.com",
                                                                Port = 587,
                                                                Username = "robert@jcibox.com",
                                                                Password = "uOdxtP0RBKLb0_JqxOjA2w",
                                                                EnableSSL = false
                                                            };
                //public static SMTPConfiguration Default = new SMTPConfiguration
                //                                            {
                //                                                Server    = "smtp.mandrillapp.com",
                //                                                Port      = 587,
                //                                                Username = "qhunain@indiahicks.com",
                //                                                Password = "uYqTYnkiRPEqX4TfrvVE3g",
                //                                                EnableSSL = false
                //                                            };
            }
        }

        /// <summary>
        /// Company information
        /// </summary>
        public static class Company
        {
            public static string Name     = "India Hicks";
            public static Address Address = new Address
            {
                                                Address1 = "3130 Wilshire Blvd, Suite 150",
                                                Address2 = "",
                                                City     = "Santa Monica",
                                                State    = "CA",
                                                Zip      = "90403",
                                                Country  = "US"
                                            };
            public static string Phone    = "(844)456-1456";
            public static string Email    = "support@indiahicks.com";
            public static string Twitter  = "indiahicksstyle";
            public static string Facebook = "India-Hicks/112391725447647";
        }

        /// <summary>
        /// Encryptions used for silent logins and other AES encryptions
        /// </summary>
        public static class Encryptions
        {
            public static class General
            {
                public static string Key = Exigo.Api.CompanyKey + "token";
                public static string IV  = "xxxxxxxxxxxxxxxx"; // Must be 16 characters long
            }                               

            public static class SilentLogins
            {
                public static string Key = Exigo.Api.CompanyKey + "silentlogin";
                public static string IV  = "xxxxxxxxxxxxxxxx"; // Must be 16 characters long
            }
        }

        /// <summary>
        /// Regular expressions used throughout all websites
        /// </summary>
        public static class RegularExpressions
        {
            public const string EmailAddresses = "[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?";
            public const string LoginName      = "^[a-zA-Z0-9]+$";
            public const string Password       = "^.{1,50}$";
        }
    }

    public enum MarketName
    {
        UnitedStates,
        Canada
    }    
    public enum AvatarType
    {
        Tiny,
        Small,
        Default,
        Large
    }
}