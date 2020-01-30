using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI.WebControls;

public static class DictionaryExtensions
{
    public static Dictionary<string, object> ToDictionary(this object data)
    {
        if (data == null) return null; // Or throw an ArgumentNullException if you want

        BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        foreach (PropertyInfo property in
                 data.GetType().GetProperties(publicAttributes))
        {
            if (property.CanRead)
            {
                dictionary.Add(property.Name, property.GetValue(data, null));
            }
        }
        return dictionary;
    }

    public static List<SelectListItem> ToSelectList<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, bool includeFirstNull = true, bool selectFirst = false)
    {
        return dictionary.ToSelectList(includeFirstNull ? "Select a value" : string.Empty, selectFirst);
    }

    public static List<SelectListItem> ToSelectList<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string selectText, bool selectFirst = false)
    {
        var d = dictionary.Select(s => new SelectListItem { Value = s.Key.ToString(), Text = s.Value.ToString() }).ToList();
        if (!string.IsNullOrEmpty(selectText))
            d.Insert(0, new SelectListItem { Text = selectText, Value = "" });
        if (selectFirst &&
            d.Count != 0)
            d[0].Selected = true;
        return d;
    }
}