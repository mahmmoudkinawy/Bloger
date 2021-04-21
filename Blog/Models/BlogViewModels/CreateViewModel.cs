using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Blog
{
    public class CreateViewModel
    {
        [Required,Display(Name = "Header Image")]
        public IFormFile BlogHeaderImage { get; set; }
        public BlogModel Blog { get; set; }
    }
}
