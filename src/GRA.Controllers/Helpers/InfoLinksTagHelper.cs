﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GRA.Domain.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GRA.Controllers.Helpers
{
    [HtmlTargetElement("infolinks", Attributes = "navPages")]
    public class InfoLinksTagHelper : TagHelper
    {
        private readonly PageService _pageService;
        private readonly IUrlHelperFactory _urlHelperFactory;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("navPages")]
        public bool navPages { get; set; }

        public InfoLinksTagHelper(IUrlHelperFactory urlHelperFactory,
            PageService pageService)
        {
            _urlHelperFactory = urlHelperFactory 
                ?? throw new ArgumentNullException(nameof(urlHelperFactory));
            _pageService = pageService 
                ?? throw new ArgumentNullException(nameof(pageService));
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var pages = await _pageService.GetAreaPagesAsync(navPages);
            if (pages.Any())
            {
                IUrlHelper url = _urlHelperFactory.GetUrlHelper(ViewContext);
                string activeStub = url.ActionContext.RouteData.Values["stub"] as string;
                var pageList = new List<string>();
                foreach (var page in pages)
                {
                    var link = url.Action("Index", "Info", new { id = page.PageStub });
                    if (navPages)
                    {
                        if (page.PageStub == activeStub)
                        {
                            pageList.Add($"<li class=\"active\"><a href=\"{link}\">{page.NavText}</a></li>");
                        }
                        else
                        {
                            pageList.Add($"<li><a href=\"{link}\">{page.NavText}</a></li>");
                        }
                    }
                    else
                    {
                        if (page.PageStub == activeStub)
                        {
                            pageList.Add($"<a class=\"active\" href=\"{link}\">{page.FooterText}</a>");
                        }
                        else
                        {
                            pageList.Add($"<a href=\"{link}\">{page.FooterText}</a>");
                        }
                    }
                }
                if (navPages)
                {
                    output.TagName = "";
                    output.Content.AppendHtml(string.Join(" ", pageList));
                }
                else
                {
                    output.TagName = "div";
                    output.Attributes.Add("class", "infolinks");
                    output.Content.AppendHtml(string.Join(" | ", pageList));
                    var session = ViewContext.HttpContext.Session;
                    if (!string.IsNullOrEmpty(session.GetString(SessionKey.UserBranchName)) &&
                        !string.IsNullOrEmpty(session.GetString(SessionKey.UserBranchUrl)))
                    {
                        var branchLink = new TagBuilder("a")
                        {
                            TagRenderMode = TagRenderMode.Normal
                        };
                        branchLink.InnerHtml.AppendHtml(
                            session.GetString(SessionKey.UserBranchName));
                        branchLink.MergeAttribute("href",
                            session.GetString(SessionKey.UserBranchUrl));
                        output.Content.AppendHtml("<br>");
                        output.Content.AppendHtml(branchLink);
                    }
                }
            }
            else
            {
                output.TagName = "";
            }
        }
    }
}
