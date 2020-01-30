using Common.Api.ExigoWebService;
using System;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Gets the start date for an autoship with the provided frequency type.
        /// </summary>
        /// <param name="frequency">How often the autoship will run</param>
        /// <returns>The start date for an autoship</returns>
        public static DateTime GetAutoOrderStartDate(FrequencyType frequency)
        {
            DateTime autoshipstartDate = new DateTime();
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            switch (frequency)
            {
                case FrequencyType.Weekly: autoshipstartDate = now.AddDays(7); break;
                case FrequencyType.BiWeekly: autoshipstartDate = now.AddDays(14); break;
                case FrequencyType.EveryFourWeeks: autoshipstartDate = now.AddDays(28); break;
                case FrequencyType.Monthly: autoshipstartDate = now.AddMonths(1); break;
                case FrequencyType.BiMonthly: autoshipstartDate = now.AddMonths(2); break;
                case FrequencyType.Quarterly: autoshipstartDate = now.AddMonths(3); break;
                case FrequencyType.SemiYearly: autoshipstartDate = now.AddMonths(6); break;
                case FrequencyType.Yearly: autoshipstartDate = now.AddYears(1); break;
                default: autoshipstartDate = now; break;
            }

            // Ensure we are not returning a day of 29, 30 or 31.
            autoshipstartDate = GetNextAvailableAutoOrderStartDate(autoshipstartDate);

            return autoshipstartDate;
        }

        /// <summary>
        /// Gets the next available date for an autoship starting with the provided date.
        /// </summary>
        /// <param name="date">The original start date</param>
        /// <returns>The nearest available start date for an autoship</returns>
        public static DateTime GetNextAvailableAutoOrderStartDate(DateTime date)
        {
            // Ensure we are not returning a day of 29, 30 or 31.
            if (date.Day > 28)
            {
                date = new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).Date;
            }

            return date;
        }
    }
}