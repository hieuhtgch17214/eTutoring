using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eTutoring.Models
{
    [Table("BlogPosts")]
    public class BlogPostModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // The author
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}