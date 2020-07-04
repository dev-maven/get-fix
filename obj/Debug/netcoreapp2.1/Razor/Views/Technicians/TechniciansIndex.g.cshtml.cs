#pragma checksum "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f7971c843e64742099e467849064f5d856ae8ed5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Technicians_TechniciansIndex), @"mvc.1.0.view", @"/Views/Technicians/TechniciansIndex.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Technicians/TechniciansIndex.cshtml", typeof(AspNetCore.Views_Technicians_TechniciansIndex))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\admin\source\repos\Technicians\Views\_ViewImports.cshtml"
using Technicians;

#line default
#line hidden
#line 2 "C:\Users\admin\source\repos\Technicians\Views\_ViewImports.cshtml"
using Technicians.Models;

#line default
#line hidden
#line 3 "C:\Users\admin\source\repos\Technicians\Views\_ViewImports.cshtml"
using Technicians.ViewModels;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f7971c843e64742099e467849064f5d856ae8ed5", @"/Views/Technicians/TechniciansIndex.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"66b2a54a6fad9341fca2e3245128d816ca2c6eaa", @"/Views/_ViewImports.cshtml")]
    public class Views_Technicians_TechniciansIndex : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PaginatedList<Technicians.Models.Technician>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Details", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 1 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
  
    Layout = "~/Views/Shared/_AdminLayout.cshtml";

#line default
#line hidden
            BeginContext(59, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(114, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 7 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
  
    ViewData["Title"] = "Technicians";

#line default
#line hidden
            BeginContext(163, 40, true);
            WriteLiteral("\r\n<h2>List of Technicians</h2>\r\n<br>\r\n\r\n");
            EndContext();
#line 14 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
 using (Html.BeginForm("Index", "Technicians", FormMethod.Get))
{

#line default
#line hidden
            BeginContext(269, 128, true);
            WriteLiteral("<div class=\"row\">\r\n        <div class=\"col-md-6\">\r\n            <div class=\"input-group\">\r\n                Find by First Name:   ");
            EndContext();
            BeginContext(398, 94, false);
#line 18 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                                 Write(Html.TextBox("SearchString", ViewBag.CurrentFilter as string, new { @class = "form-control" }));

#line default
#line hidden
            EndContext();
            BeginContext(492, 108, true);
            WriteLiteral("\r\n                <input type=\"submit\" value=\"Search\" />\r\n            </div>\r\n        </div>\r\n\r\n    </div>\r\n");
            EndContext();
#line 24 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"

}

#line default
#line hidden
            BeginContext(605, 142, true);
            WriteLiteral("    <br>\r\n\r\n    <table class=\"table table-striped table-hover\">\r\n        <thead>\r\n            <tr>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(748, 119, false);
#line 32 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
               Write(Html.ActionLink("First Name", "Index", new { sortOrder = ViewBag.NameSortParm, currentFilter = ViewBag.CurrentFilter }));

#line default
#line hidden
            EndContext();
            BeginContext(867, 67, true);
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(935, 52, false);
#line 35 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.First().LastName));

#line default
#line hidden
            EndContext();
            BeginContext(987, 67, true);
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(1055, 55, false);
#line 38 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.First().PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(1110, 67, true);
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(1178, 56, false);
#line 41 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
               Write(Html.DisplayNameFor(model => model.First().EmailAddress));

#line default
#line hidden
            EndContext();
            BeginContext(1234, 67, true);
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ");
            EndContext();
            BeginContext(1302, 124, false);
#line 44 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
               Write(Html.ActionLink("Date Registered", "Index", new { sortOrder = ViewBag.DateSortParm, currentFilter = ViewBag.CurrentFilter }));

#line default
#line hidden
            EndContext();
            BeginContext(1426, 152, true);
            WriteLiteral("\r\n                </th>\r\n                <th>\r\n                    ACTION\r\n                </th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n");
            EndContext();
#line 52 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
            BeginContext(1635, 72, true);
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        ");
            EndContext();
            BeginContext(1708, 44, false);
#line 56 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.FirstName));

#line default
#line hidden
            EndContext();
            BeginContext(1752, 79, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
            EndContext();
            BeginContext(1832, 43, false);
#line 59 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.LastName));

#line default
#line hidden
            EndContext();
            BeginContext(1875, 79, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
            EndContext();
            BeginContext(1955, 46, false);
#line 62 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.PhoneNumber));

#line default
#line hidden
            EndContext();
            BeginContext(2001, 79, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
            EndContext();
            BeginContext(2081, 47, false);
#line 65 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.EmailAddress));

#line default
#line hidden
            EndContext();
            BeginContext(2128, 79, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
            EndContext();
            BeginContext(2208, 49, false);
#line 68 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                   Write(Html.DisplayFor(modelItem => item.DateRegistered));

#line default
#line hidden
            EndContext();
            BeginContext(2257, 79, true);
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
            EndContext();
            BeginContext(2336, 69, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c3a32dacbcaa41d98058470d2e71fbc6", async() => {
                BeginContext(2394, 7, true);
                WriteLiteral("Details");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 71 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                                                  WriteLiteral(item.TechnicianId);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2405, 28, true);
            WriteLiteral(" |\r\n                        ");
            EndContext();
            BeginContext(2433, 67, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a829c638e1c44fb49180a24a60c9b833", async() => {
                BeginContext(2490, 6, true);
                WriteLiteral("Delete");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#line 72 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
                                                 WriteLiteral(item.TechnicianId);

#line default
#line hidden
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(2500, 52, true);
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
            EndContext();
#line 75 "C:\Users\admin\source\repos\Technicians\Views\Technicians\TechniciansIndex.cshtml"
            }

#line default
#line hidden
            BeginContext(2567, 32, true);
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PaginatedList<Technicians.Models.Technician>> Html { get; private set; }
    }
}
#pragma warning restore 1591
