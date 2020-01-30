﻿using System.Linq.Expressions;
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

public static class CollectionExtensions
{
    /// <summary>
    /// Flattens a nested collection into a single, linear collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="childSelector"></param>
    /// <returns></returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSelector)
    {
        // Do standard error checking here.

        // Create a stack for recursion.  Push all of the items
        // onto the stack.
        var stack = new Stack<T>(source);

        // While there are items on the stack.
        while (stack.Count > 0)
        {
            // Pop the item.
            T item = stack.Pop();

            // Yield the item.
            yield return item;

            // Push all of the children on the stack.
            foreach (T child in childSelector(item)) stack.Push(child);
        }
    }

    /// <summary>
    /// Checks if the provided value can be found in the provided collection.
    /// </summary>
    /// <typeparam name="T">THe type of object we are checking.</typeparam>
    /// <param name="source">The value to check</param>
    /// <param name="list"The collection of items></param>
    /// <returns>True if the provided item is found in the collection.</returns>
    public static bool In<T>(this T source, params T[] list)
    {
        if (null == source) throw new ArgumentNullException("source");
        return list.Contains(source);
    }
}