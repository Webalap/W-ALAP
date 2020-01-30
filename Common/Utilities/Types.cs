using System;

namespace Common
{
    public static partial class GlobalUtilities
    {
        public static TDestination Extend<TSource, TDestination>(TSource source, TDestination destination) where TDestination : TSource
        {
            var sourceType = source.GetType();
            foreach (var property in sourceType.GetProperties())
            {
                if (property.CanWrite)
                {
                    property.SetValue(destination, property.GetValue(source));
                }
            }

            return destination;
        }

        /// <summary>
        /// Attempts to parse the provided object as the provided type. If the parsing is unsuccessful, it will reutrn the provided default value.
        /// </summary>
        /// <typeparam name="T">The type to parse your string to.</typeparam>
        /// <param name="s">The object to parse.</param>
        /// <param name="defaultValue">The value that will be returned if parsing is unsuccessful.</param>
        /// <returns></returns>
        public static T TryParse<T>(object s, object defaultValue)
        {
            try
            {
                return (T)Convert.ChangeType(s, typeof(T));
            }
            catch
            {
                return (T)defaultValue;
            }
        }
        /// <summary>
        /// Converts Customer Type to Description
        /// </summary>
        /// <param name="customerTypeID"></param>
        public static string GetCustomerTypeDescription(int customerTypeID)
        {
            switch (customerTypeID)
            {
                case CustomerTypes.IndependentStyleAmbassador:
                    return "Ambassador";
                case CustomerTypes.AmbassadorProspect :
                    return "Ambassador Prospect";
                case CustomerTypes.CustomerProspect:
                    return "Customer Prospect";
                case CustomerTypes.GeneralProspect:
                    return "General Prospect";
                case CustomerTypes.GuestProspect:
                    return "Guest Prospect";
                case CustomerTypes.Host:
                    return "Host";
                case CustomerTypes.HostProspect:
                    return "Host Prospect";
                case CustomerTypes.Lead:
                    return "Lead";
                case CustomerTypes.PartyGuest:
                    return "Party Guest";
                case CustomerTypes.RetailCustomer:
                    return "Retail Customer";
                case CustomerTypes.Party:
                    return "Party";
                default:
                    return string.Empty;
            }

        }
    }
}