#pragma checksum "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\Dashboard\UserDashboard.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ae2cc4fae31b8a25ca504d16540e7c6207007c7c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Dashboard_UserDashboard), @"mvc.1.0.view", @"/Views/Dashboard/UserDashboard.cshtml")]
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
#nullable restore
#line 1 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\_ViewImports.cshtml"
using SEO_Reporting_Portal;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\_ViewImports.cshtml"
using SEO_Reporting_Portal.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ae2cc4fae31b8a25ca504d16540e7c6207007c7c", @"/Views/Dashboard/UserDashboard.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"53c1fb7009c136a2a850448fc602b4778e3518cc", @"/Views/_ViewImports.cshtml")]
    public class Views_Dashboard_UserDashboard : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SEO_Reporting_Portal.ViewModels.UserDashboardViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("bg-light send-msg-form"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/userDashboard.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 5 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\Dashboard\UserDashboard.cshtml"
  
    ViewData["Title"] = "Dashboard";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""row"">
    <div class=""col-md-3 col-sm-6 col-xs-12"">
        <div class=""info-box"">
            <span class=""info-box-icon bg-aqua""><i class=""fas fa-file""></i></span>

            <div class=""info-box-content"">
                <span class=""info-box-text"">Total Reports</span>
                <span class=""info-box-number"">");
#nullable restore
#line 16 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\Dashboard\UserDashboard.cshtml"
                                         Write(Model.TotalReports);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
            </div>
            <!-- /.info-box-content -->
        </div>
        <!-- /.info-box -->
    </div>
    <div class=""col-md-3 col-sm-6 col-xs-12"">
        <div class=""info-box"">
            <span class=""info-box-icon bg-green""><i class=""far fa-clock""></i></span>

            <div class=""info-box-content"">
                <span class=""info-box-text"">Upcoming Report</span>
                <span class=""info-box-number"">");
#nullable restore
#line 28 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\Dashboard\UserDashboard.cshtml"
                                         Write(Model.UpcomingReportDays);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" Days</span>
            </div>
            <!-- /.info-box-content -->
        </div>
        <!-- /.info-box -->
    </div>
    <div class=""col-md-3 col-sm-6 col-xs-12"">
        <div class=""info-box"">
            <span class=""info-box-icon bg-yellow""><i class=""fas fa-calendar-plus""></i></span>

            <div class=""info-box-content"">
                <span class=""info-box-text"">Contract Start Date</span>
                <span class=""info-box-number"">");
#nullable restore
#line 40 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\Dashboard\UserDashboard.cshtml"
                                         Write(Model.ContractStartDate);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
            </div>
            <!-- /.info-box-content -->
        </div>
        <!-- /.info-box -->
    </div>
    <div class=""col-md-3 col-sm-6 col-xs-12"">
        <div class=""info-box"">
            <span class=""info-box-icon bg-red""><i class=""fas fa-calendar-times""></i></span>

            <div class=""info-box-content"">
                <span class=""info-box-text"">Contract End Date</span>
                <span class=""info-box-number"">");
#nullable restore
#line 52 "G:\SEO-Reporting-Portal\SEO Reporting Portal\Views\Dashboard\UserDashboard.cshtml"
                                         Write(Model.ContractEndDate);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
            </div>
            <!-- /.info-box-content -->
        </div>
        <!-- /.info-box -->
    </div>
</div>


<div class=""row no-gutters mb-4"">
    <div class=""col-xl-6 col-lg-6 col-md-6 col-sm-12 col-12"">
        <div class=""chat-card m-0"">
            <!-- Row start -->
            <div class=""chat-container"">
                <ul class=""chat-list chatContainerScroll"">
                </ul>

                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ae2cc4fae31b8a25ca504d16540e7c6207007c7c8003", async() => {
                WriteLiteral(@"
                    <div class=""form-group m-0"">
                        <div class=""input-group"">
                            <input type=""text"" id=""txt-message"" placeholder=""Type a message"" aria-describedby=""button-addon2"" class=""form-control rounded-0 border-0 py-4 bg-light messg"">
                            <div class=""input-group-append"">
                                <button type=""submit"" class=""send-message-btn btn btn-link""><i class=""fa fa-paper-plane""></i></button>
                            </div>
                        </div>
                    </div>
                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n    <!-- Row end -->\r\n</div>\r\n\r\n\r\n");
            DefineSection("scripts", async() => {
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ae2cc4fae31b8a25ca504d16540e7c6207007c7c10331", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SEO_Reporting_Portal.ViewModels.UserDashboardViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
