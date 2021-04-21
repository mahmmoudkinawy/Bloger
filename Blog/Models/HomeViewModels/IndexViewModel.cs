using PagedList.Core;

namespace Blog.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IPagedList<BlogModel> Blogs { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
    }
}
