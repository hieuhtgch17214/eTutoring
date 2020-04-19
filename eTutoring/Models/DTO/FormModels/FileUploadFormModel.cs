using MultipartDataMediaFormatter.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eTutoring.Models.DTO.FormModels
{
    public class FileUploadFormModel
    {
        [Required]
        public HttpFile File { get; set; }
    }
}