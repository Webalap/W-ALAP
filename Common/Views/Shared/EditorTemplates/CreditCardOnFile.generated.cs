﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common.Views.Shared.EditorTemplates
{
    using Common;
    using Common.Helpers;
    using Common.HtmlHelpers;
    using Common.Services;
    using ExigoService;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/CreditCardOnFile.cshtml")]
    public partial class _CreditCardOnFile : System.Web.Mvc.WebViewPage<CreditCard>
    {
        public _CreditCardOnFile()
        {
        }
        public override void Execute()
        {
WriteLiteral("<input");

WriteLiteral(" type=\"hidden\"");

WriteAttribute("name", Tuple.Create(" name=\"", 41), Tuple.Create("\"", 123)
            
            #line 3 "..\..\Views\Shared\EditorTemplates\CreditCardOnFile.cshtml"
, Tuple.Create(Tuple.Create("", 48), Tuple.Create<System.Object, System.Int32>(Html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix
            
            #line default
            #line hidden
, 48), false)
, Tuple.Create(Tuple.Create("", 105), Tuple.Create(".PaymentMethodType", 105), true)
);

WriteAttribute("value", Tuple.Create(" value=\"", 124), Tuple.Create("\"", 159)
            
            #line 3 "..\..\Views\Shared\EditorTemplates\CreditCardOnFile.cshtml"
                                , Tuple.Create(Tuple.Create("", 132), Tuple.Create<System.Object, System.Int32>(Model.GetType().ToString()
            
            #line default
            #line hidden
, 132), false)
);

WriteLiteral(" />\r\n");

            
            #line 4 "..\..\Views\Shared\EditorTemplates\CreditCardOnFile.cshtml"
Write(Html.HiddenFor(c => c.Type));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
