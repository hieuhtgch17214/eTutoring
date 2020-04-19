using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO.FormModels
{
    public class FileCommentFormModel
    {
        [Required]
        public int FileId { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}