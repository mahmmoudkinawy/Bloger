using Blog.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.BusinessManagers.Interfaces
{
    public interface IHomeBusinessManager
    {
        ActionResult<AuthorViewModel> GetAuthorViewModel(string authorId, string searchString, int? page);
    }
}
