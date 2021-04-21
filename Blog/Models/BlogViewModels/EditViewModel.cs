using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.BlogViewModels
{
    public class EditViewModel
    {
        [Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public BlogModel Blog { get; set; }
    }
}
