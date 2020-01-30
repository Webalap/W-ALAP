using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace ExigoService
{
    public interface IPropertyBag
    {
        string Version { get; set; }
        string Description { get; set; }
        string SessionID { get; set; }
        DateTime CreatedDate { get; set; }
        int Expires { get; set; }

        bool IsValid();
        T OnBeforeUpdate<T>(T propertyBag) where T : IPropertyBag;
    }
}