using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public BlogModel Blog { get; set; }
        public ApplicationUser Poser { get; set; }
        public string Content { get; set; }
        public Post Parent { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; }

    }
}
