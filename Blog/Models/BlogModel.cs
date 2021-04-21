﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public ApplicationUser Creator { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool Published { get; set; }
        public bool Approved { get; set; }
        public ApplicationUser Approver { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }
}
