using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Blog.TagHelpers
{
    [HtmlTargetElement(Attributes = "list, count")]
    public class HidePagerHelper : TagHelper
    {
        public IEnumerable<object> List { get; set; }
        public int count { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (List.Count() <= count)
                output.SuppressOutput();
        }
    }
}
