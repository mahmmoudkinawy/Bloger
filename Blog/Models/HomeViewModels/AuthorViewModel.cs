using PagedList.Core;

namespace Blog.Models.HomeViewModels
{
    public class AuthorViewModel
    {
        public ApplicationUser Author { get; set; }
        public IPagedList<BlogModel> Blogs { get; set; }
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
    }
}
