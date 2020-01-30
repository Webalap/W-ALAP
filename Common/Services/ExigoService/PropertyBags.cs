using Common.Api.ExigoWebService;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static class PropertyBags
        {
            public static T Get<T>(string description) where T : IPropertyBag
            {
                // Attempt to load the bag from the cookie
                var cookie = HttpContext.Current.Request.Cookies[description];
                if (cookie == null)
                {
                    return Create<T>(description);
                }


                // Get the session from Exigo
                var sessionData = Exigo.WebService().GetSession(new GetSessionRequest()
                {
                    SessionID = cookie.Value
                }).SessionData;

                if (string.IsNullOrEmpty(sessionData))
                {
                    return Create<T>(description);
                }

                try
                {
                    // Deserialize the session data and get our bag.
                    dynamic bag = Deserialize<T>(sessionData);

                    // If the customer ID in the bag doesn't match the current customer ID, stop here.
                    if (!bag.IsValid())
                    {
                        return Create<T>(description);
                    }

                    // If we got here, we have a valid property bag. Populate it into the current object.
                    return bag;
                }
                catch
                {
                    return Create<T>(description);
                }
            }
            public static T Create<T>(string description) where T : IPropertyBag
            {
                dynamic bag = Activator.CreateInstance(typeof(T));

                bag.SessionID = HttpContext.Current.Server.UrlEncode(Guid.NewGuid().ToString());
                bag.Description = description;
                bag.CreatedDate = DateTime.Now;

                bag = bag.OnBeforeUpdate(bag);

                Update<T>(bag);

                return bag;
            }
            public static T Update<T>(T propertyBag) where T : IPropertyBag
            {
                // Set the session
                Exigo.WebService().SetSession(new SetSessionRequest
                {
                    SessionID = propertyBag.SessionID,
                    SessionData = Serialize<T>(propertyBag)
                });


                // Set the cookie
                var cookie = new HttpCookie(propertyBag.Description, propertyBag.SessionID);
                if (propertyBag.Expires > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(propertyBag.Expires);
                    cookie.HttpOnly = true;
                }
                HttpContext.Current.Response.Cookies.Add(cookie);
                return propertyBag;
            }
            public static T Delete<T>(T propertyBag) where T : IPropertyBag
            {
                // gwb:20141222 - We should also need to clear the session in Exigo since there is
                // no API to delete session.
                Exigo.WebService().SetSession(new SetSessionRequest
                {
                    SessionID = propertyBag.SessionID,
                    SessionData = ""
                });

                // gwb:20141222 - Now we can create the new session and cookie
                var bag = Create<T>(propertyBag.Description);

                return bag;
            }

            public static bool SessionExists<T>(string description) where T : IPropertyBag
            {
                var cookie = HttpContext.Current.Request.Cookies[description];
                if (null != cookie)
                {
                    // Get the session from Exigo
                    var sessionData = Exigo.WebService().GetSession(new GetSessionRequest()
                    {
                        SessionID = cookie.Value
                    }).SessionData;

                    if (!string.IsNullOrWhiteSpace(sessionData))
                    {
                        return true;
                    }
                }

                return false;
            }

            public static string Serialize<T>(T propertyBag) where T : IPropertyBag
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

                return JsonConvert.SerializeObject(propertyBag, settings);
            }
            public static T Deserialize<T>(string sessionData) where T : IPropertyBag
            {
                var settings = new JsonSerializerSettings
                {
                    //TypeNameHandling = TypeNameHandling.Objects
                    TypeNameHandling = TypeNameHandling.All
                };

                return (T)JsonConvert.DeserializeObject(sessionData, typeof(T), settings);
            }
        }
    }
}