using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Globalization;

namespace ExigoService
{
    public abstract class BasePropertyBag : IPropertyBag
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public string SessionID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Expires { get; set; }

        public virtual bool IsValid()
        {
            return true;
        }
        public virtual T OnBeforeUpdate<T>(T propertyBag) where T : IPropertyBag
        {
            return propertyBag;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}