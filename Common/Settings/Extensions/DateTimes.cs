using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

public static class DateTimeExtensions
{
    /// <summary>
    /// Gets the first week day following a date.
    /// </summary> 
    /// <param name="date">The date.</param> 
    /// <param name="dayOfWeek">The day of week to return.</param> 
    /// <returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns> 
    public static DateTime GetNextWeekDay(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    ///<summary>Gets the first week day following a date.</summary>
    ///<param name="date">The date.</param>
    ///<param name="dayOfWeek">The day of week to return.</param>
    ///<returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns>
    public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
    {
        return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the hour of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfHour(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the day of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfDay(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the month of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, date.Kind);
    }

    /// <summary>
    /// Get a DateTime that represents the beginning of the year of the provided date.
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns></returns>
    public static DateTime BeginningOfYear(this DateTime date)
    {
        return new DateTime(date.Year, 1, 1, 0, 0, 0, 0, date.Kind);
    }
}