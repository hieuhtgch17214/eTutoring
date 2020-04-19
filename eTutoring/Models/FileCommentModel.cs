using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models
{
    public class FileCommentModel
    {
        public int ID { get; set; }
        public string Comment { get; set; }

        // Author
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        // Corresponding File
        public int FileUploadId { get; set; }
        public virtual FileUploadModel FileUpload { get; set; }
    }
}