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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/Password.cshtml")]
    public partial class _Password : System.Web.Mvc.WebViewPage<String>
    {
        public _Password()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Shared\EditorTemplates\Password.cshtml"
  
    Layout = "~/Views/Shared/EditorTemplates/_FormField.cshtml";
    
    var htmlAttributes = Html.GetEditorHtmlAttributes(new
    { 
        @class              = "form-control",
        type                = "password",
        role                = "textbox",
        data_restrict_input = "password"
    });

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\Password.cshtml"
Write(Html.TextBoxFor(c => c, htmlAttributes));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
