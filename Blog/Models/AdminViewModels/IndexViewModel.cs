using System.Collections.Generic;

namespace Blog.Models.AdminViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<BlogModel> Blogs { get; set; }
    }
}
