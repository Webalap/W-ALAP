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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/BankAccountOnFile.cshtml")]
    public partial class _BankAccountOnFile : System.Web.Mvc.WebViewPage<BankAccount>
    {
        public _BankAccountOnFile()
        {
        }
        public override void Execute()
        {
WriteLiteral("<input");

WriteLiteral(" type=\"hidden\"");

WriteAttribute("name", Tuple.Create(" name=\"", 42), Tuple.Create("\"", 124)
            
            #line 3 "..\..\Views\Shared\EditorTemplates\BankAccountOnFile.cshtml"
, Tuple.Create(Tuple.Create("", 49), Tuple.Create<System.Object, System.Int32>(Html.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix
            
            #line default
            #line hidden
, 49), false)
, Tuple.Create(Tuple.Create("", 106), Tuple.Create(".PaymentMethodType", 106), true)
);

WriteAttribute("value", Tuple.Create(" value=\"", 125), Tuple.Create("\"", 160)
            
            #line 3 "..\..\Views\Shared\EditorTemplates\BankAccountOnFile.cshtml"
                                , Tuple.Create(Tuple.Create("", 133), Tuple.Create<System.Object, System.Int32>(Model.GetType().ToString()
            
            #line default
            #line hidden
, 133), false)
);

WriteLiteral(" />\r\n");

            
            #line 4 "..\..\Views\Shared\EditorTemplates\BankAccountOnFile.cshtml"
Write(Html.HiddenFor(c => c.Type));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
