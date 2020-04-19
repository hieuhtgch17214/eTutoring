using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO
{
    public class FileUploadResponseModel
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string FileUri { get; set; }
        public IEnumerable<FileCommentResponseModel> Comments { get; set; }
    }
}