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

public static class StringExtensions
{
    /// <summary>
    /// A shortcut call for string.Format(), it formats strings. 
    /// </summary>
    /// <param name="format">The format of the string</param>
    /// <param name="args">The arguments to merge into the provided format</param>
    /// <returns>The formatted, merged string.</returns>
    public static string FormatWith(this string format, params object[] args)
    {
        if (format == null)
            throw new ArgumentNullException("format");

        return string.Format(format, args);
    }

    /// <summary>
    /// A shortcut call for string.Format(), it formats strings. 
    /// </summary>
    /// <param name="provider">The format provider to use</param>
    /// <param name="format">The format of the string</param>
    /// <param name="args">The arguments to merge into the provided format</param>
    /// <returns>The formatted, merged string.</returns>
    public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
    {
        if (format == null)
            throw new ArgumentNullException("format");

        return string.Format(provider, format, args);
    }

    /// <summary>
    /// Return either the provided value, or the provided fallback
    /// </summary>
    /// <param name="value"></param>
    /// <param name="fallback"></param>
    /// <returns></returns>
    public static string IfNullOrEmpty(this string value, string fallback)
    {
        if (!string.IsNullOrEmpty(value)) return value;
        else return fallback;
    }

    /// <summary>
    /// Shortcut to determine if a string is empty
    /// </summary>
    /// <returns></returns>
    public static bool IsEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Shortcut to determine if a string is null or empty
    /// </summary>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Shortcut to determine if a string is not empty
    /// </summary>
    /// <returns></returns>
    public static bool IsNotEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Shortcut to determine if a string is not null or empty
    /// </summary>
    /// <returns></returns>
    public static bool IsNotNullOrEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    /// <summary>
    /// Parse the provided decimal as the currency of the provided culture
    /// </summary>
    /// <param name="value">The amount</param>
    /// <param name="cultureName">The culture code to use when formatting</param>
    /// <returns>The formatted string result</returns>
    public static string ToCurrency(this decimal value, string cultureName)
    {
        CultureInfo currentCulture = new CultureInfo(cultureName);
        return (string.Format(currentCulture, "{0:C}", value));
    }

    /// <summary>
    /// Masks the string with the provided mask (default: '*'), leaving a provided number of unmasked characters remaining (default: 4).
    /// </summary>
    /// <param name="unmaskedCharCount">The number of characters to leave unmasked. Defaults to 4.</param>
    /// <param name="mask">The character used as the mask</param>
    /// <returns>The masked string, or the original string if there were not enough characters to leave unmasked.</returns>
    public static string Mask(this string str, int unmaskedCharCount = 4, char mask = '*')
    {
        if (str.Length <= unmaskedCharCount) return str;
        return str.Substring(str.Length - unmaskedCharCount).PadLeft(str.Length, mask);
    }
}